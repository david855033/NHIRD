using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NHIRD
{
    class ExtractData
    {
        List<RawDataFormat> rawDataFormats;
        string[] selectedFileTypes;
        IEnumerable<File> rawDataFileList;

        string outputDir;
        List<StringDataFormat> stringDataFormats = new List<StringDataFormat>();
        List<NumberDataFormat> numberDataFormats = new List<NumberDataFormat>();

        public void Do(List<RawDataFormat> rawDataFormats, IEnumerable<string> selectedFileTypes, IEnumerable<File> list_file, string outputDir)
        {
            ///使用rawDataFormats初始化資料清單(分為數值資料跟字串資料)
            this.selectedFileTypes = selectedFileTypes.ToArray();
            this.rawDataFormats = rawDataFormats;
            this.outputDir = outputDir;
            this.rawDataFileList = list_file;

            initializeDataFormats();

            //逐個檔案執行讀寫
            ReadJudgeWriteFiles();
        }

        // step 1
        void initializeDataFormats()
        {
            var queryStr =
                from r in rawDataFormats
                where selectedFileTypes.Any(x => x == r.FileType) && r.DataType == "C"
                select r;
            foreach (var q in queryStr)
            {
                var toAdd = new StringDataFormat()
                {
                    key = q.ColumnName,
                    position = q.Postion,
                    length = q.Lengths,
                    start_year = q.start_year,
                    end_year = q.end_year,
                    FileType = q.FileType
                };
                stringDataFormats.Add(toAdd);
            }


            var queryNum =
               from r in rawDataFormats
               where selectedFileTypes.Any(x => x == r.FileType) && r.DataType == "N"
               select r;
            foreach (var q in queryNum)
            {
                var toAdd = new NumberDataFormat()
                {
                    key = q.ColumnName,
                    position = q.Postion,
                    length = q.Lengths,
                    start_year = q.start_year,
                    end_year = q.end_year,
                    FileType = q.FileType
                };
                numberDataFormats.Add(toAdd);
            }
        }

        // step 2 依序質性下面五個function
        void ReadJudgeWriteFiles()
        {

            //先分析總共有多少組別
            var groupQuery = (from q in rawDataFileList where q.selected == true select q.@group).Distinct();
            //逐個組別進行(若是有比較的資料 如ID / SEQ，則先載入比較用的list)
            foreach (string g in groupQuery)
            {
                var current_list_file = from q in rawDataFileList where q.@group == g select q;
                if (CriteriaList.Any(x => x.key == "IDLIST") == true)//有使用"IDLIST"的條件，需要載入IDLIST
                    initializeIDCriteria(g);
                // 逐個檔案進行
                foreach (File currentfile in current_list_file)
                {
                    if (CriteriaList.Any(x => x.key == "ACTIONLIST") == true)
                        initializeActionCriteria(currentfile);
                    //依照年分及filetype挑出正確的Dataformat，傳入Readfile及Writefile
                    var queryStringDataFormats =
                         (from q in stringDataFormats
                          where (currentfile.MKyear >= q.start_year || q.start_year == 0) &&
                                (currentfile.MKyear <= q.end_year || q.end_year == 0) &&
                                 q.FileType == currentfile.FileType
                          select q).ToList();
                    var queryNumberDataFormats =
                      (from q in numberDataFormats
                       where (currentfile.MKyear >= q.start_year || q.start_year == 0) &&
                             (currentfile.MKyear <= q.end_year || q.end_year == 0) &&
                              q.FileType == currentfile.FileType
                       select q).ToList();

                    //檢查目錄是否存在並且產生輸出檔名
                    if (!Directory.Exists(outputDir))
                        Directory.CreateDirectory(outputDir);
                    string outpath = outputDir + "\\" + currentfile.name.Split('.')[0] + ".EXT";

                    //產生屬於該筆檔案criteria對應的index, 並且儲存到criteriaList中
                    initiateCriteriaIndex(currentfile, queryStringDataFormats, queryNumberDataFormats);

                    //使用sr及sw依序讀入、判斷、寫入
                    using (var sr = new StreamReader(currentfile.path, System.Text.Encoding.Default))
                    using (var sw = new StreamWriter(outpath, false, System.Text.Encoding.Default))
                    {
                        // --產生title
                        string title = "";
                        foreach (var q in queryStringDataFormats)
                        {
                            title += q.key + '\t';
                        }
                        foreach (var q in queryNumberDataFormats)
                        {
                            title += q.key + '\t';
                        }
                        sw.WriteLine(title.TrimEnd('\t'));
                        // --產生內容
                        while (!sr.EndOfStream)
                        {
                            DataRow dataRow = new DataRow(queryStringDataFormats.Count(), queryNumberDataFormats.Count());

                            try
                            {
                                ReadRow(sr, dataRow, queryStringDataFormats, queryNumberDataFormats);
                                JudgeRow(dataRow);
                                WriteRow(sw, dataRow, queryStringDataFormats, queryNumberDataFormats);
                            }
                            catch
                            {
                                //* bad data error
                            }
                        }
                    }
                }
            }
        }

        // step 2.1 - 2.6
        void initializeIDCriteria(string CurrentGroup)
        {
            var NewIDCriteriaList = new DistinctList<IDData>();
            //搜尋同組的Criteria File
            var FilesOfTheGroup = from q in CriteriaList.Find(x => x.key == "IDLIST").IDCriteriaFileList
                                  where q.@group == CurrentGroup
                                  select q;
            //載入同組File中的ID+Birthday進入IDList，儲存在該筆criteria中
            foreach (var f in FilesOfTheGroup)
            {
                using (var sr = new StreamReader(f.path))
                {
                    string[] titles = sr.ReadLine().Split('\t');
                    var indexBirthday = Array.FindIndex(titles, x => x.IndexOf("BIRTHDAY") >= 0);
                    var indexID = Array.FindIndex(titles, x => x == "ID");
                    while (!sr.EndOfStream)
                    {
                        string[] linesplit = sr.ReadLine().Split('\t');
                        NewIDCriteriaList.AddDistinct(new IDData
                        {
                            ID = linesplit[indexID],
                            Birthday = linesplit[indexBirthday]
                        });
                    }
                }
            }
            CriteriaList.Find(x => x.key == "IDLIST").IDCriteriaList = NewIDCriteriaList;
        }
        void initializeActionCriteria(File CurrentFile)
        {
            var NewActionCriteriaList = new DistinctList<ActionData>();
            //搜尋同組的Criteria File
            var FilesOfTheGroup = from q in CriteriaList.Find(x => x.key == "ACTIONLIST").ActionCriteriaFileList
                                  where q.@group == CurrentFile.@group && q.year == CurrentFile.year
                                  select q;
            //載入同組File中的ID+Birthday進入IDList，儲存在該筆criteria中
            foreach (var f in FilesOfTheGroup)
            {
                using (var sr = new StreamReader(f.path))
                {
                    string[] titles = sr.ReadLine().Split('\t');
                    var indexFEE_YM = Array.FindIndex(titles, x => x == "FEE_YM");
                    var indexAPPL_TYPE = Array.FindIndex(titles, x => x == "APPL_TYPE");
                    var indexHOSP_ID = Array.FindIndex(titles, x => x == "HOSP_ID");
                    var indexAPPL_DATE = Array.FindIndex(titles, x => x == "APPL_DATE");
                    var indexCASE_TYPE = Array.FindIndex(titles, x => x == "CASE_TYPE");
                    var indexSEQ_NO = Array.FindIndex(titles, x => x == "SEQ_NO");
                    while (!sr.EndOfStream)
                    {
                        string[] linesplit = sr.ReadLine().Split('\t');
                        NewActionCriteriaList.AddDistinct(new ActionData
                        {
                            FEE_YM = linesplit[indexFEE_YM],
                            APPL_TYPE = linesplit[indexAPPL_TYPE],
                            HOSP_ID = linesplit[indexHOSP_ID],
                            APPL_DATE = linesplit[indexAPPL_DATE],
                            CASE_TYPE = linesplit[indexCASE_TYPE],
                            SEQ_NO = linesplit[indexSEQ_NO]
                        }
                        );
                    }
                }
            }
            CriteriaList.Find(x => x.key == "ACTIONLIST").ActionCriteriaList = NewActionCriteriaList;
        }
        void initiateCriteriaIndex(File currentfile, List<StringDataFormat> queryStringDataFormats, List<NumberDataFormat> queryNumberDataFormats)
        {
            //逐個criteria確定要判斷的欄位index
            foreach (var currentCriteria in CriteriaList)
            {
                //重置criteriaList內之indexList(可對應到當前檔案)
                currentCriteria.indexStrDatalist = new List<int>();
                currentCriteria.indexNumData = -1;

                //將想要判斷的資料之index找出來，丟入criteria的物件中
                //ID list
                if (currentCriteria.key == "IDLIST")
                {
                    currentCriteria.indexBirthday = queryStringDataFormats.FindIndex(x => x.key.IndexOf("BIRTHDAY") >= 0);
                    currentCriteria.indexID = queryStringDataFormats.FindIndex(x => x.key == "ID");
                }
                //年齡資料
                else if (currentCriteria.key == "ACTIONLIST")
                {
                    currentCriteria.indexFEE_YM = queryStringDataFormats.FindIndex(x => x.key == "FEE_YM");
                    currentCriteria.indexAPPL_DATE = queryStringDataFormats.FindIndex(x => x.key == "APPL_DATE");
                    currentCriteria.indexAPPL_TYPE = queryStringDataFormats.FindIndex(x => x.key == "APPL_TYPE");
                    currentCriteria.indexHOSP_ID = queryStringDataFormats.FindIndex(x => x.key == "HOSP_ID");
                    currentCriteria.indexSEQ_NO = queryNumberDataFormats.FindIndex(x => x.key == "SEQ_NO");
                    currentCriteria.indexCASE_TYPE = queryStringDataFormats.FindIndex(x => x.key == "CASE_TYPE");
                }
                else if (currentCriteria.key == "AGE")
                {
                    currentCriteria.indexBirthday = queryStringDataFormats.FindIndex(x => x.key.IndexOf("BIRTHDAY") >= 0);
                    currentCriteria.indexEventday = queryStringDataFormats.FindIndex(x => x.key == "FUNC_DATE");
                    if (currentCriteria.indexEventday < 0)
                        currentCriteria.indexEventday = queryStringDataFormats.FindIndex(x => x.key == "IN_DATE");
                }
                else
                {
                    //字串資料(同時比較複數欄位)
                    foreach (var currentKeyName in
                        from q in queryStringDataFormats
                        where q.key.IndexOf(currentCriteria.key) >= 0
                        select q.key)
                    {
                        int r = queryStringDataFormats.FindIndex(x => x.key == currentKeyName);
                        if (r >= 0)
                        {
                            currentCriteria.indexStrDatalist.Add(r);
                        }
                    }
                    if (currentCriteria.indexStrDatalist.Count > 0) continue;
                    //數字資料(單一欄位)
                    var qKeyName =
                        (from q in queryNumberDataFormats
                         where q.key.IndexOf(currentCriteria.key) >= 0
                         select q.key);
                    if (qKeyName.Count() > 0)
                    {
                        int index = queryNumberDataFormats.FindIndex(x => x.key == qKeyName.First());
                        if (index >= 0)
                        {
                            currentCriteria.indexNumData = index;
                        }
                    }
                }
            }
        }
        void ReadRow(StreamReader sr, DataRow dataRow, List<StringDataFormat> queryStringDataFormats, List<NumberDataFormat> queryNumberDataFormats)
        {
            string line = sr.ReadLine();
            for (int i = 0; i < queryStringDataFormats.Count(); i++)
            {
                dataRow.stringData[i] =
                    line.Substring(queryStringDataFormats[i].position,
                    queryStringDataFormats[i].length);
            }
            for (int i = 0; i < queryNumberDataFormats.Count(); i++)
            {
                var data =
                    line.Substring(queryNumberDataFormats[i].position,
                    queryNumberDataFormats[i].length);
                if (data == "")
                {
                    dataRow.numberData[i] = null;
                }
                else
                {
                    dataRow.numberData[i] = Convert.ToDouble(data);
                }
            }
        }
        void JudgeRow(DataRow dataRow)
        {
            //比對(使用criteria.DoCheck)，只要有一項不相符就將該筆DataRow.IsMatch設定為false(預設值為true)，並跳過該筆資料
            foreach (var currentCriteria in CriteriaList)
            {
                if (dataRow.IsMatch == false) continue;
                dataRow.IsMatch = currentCriteria.DoCheck(dataRow);
            }
        }
        void WriteRow(StreamWriter sw, DataRow dataRow, List<StringDataFormat> queryStringDataFormats, List<NumberDataFormat> queryNumberDataFormats)
        {
            if (!dataRow.IsMatch) return;
            string line = "";
            foreach (string s in dataRow.stringData)
            {
                line += s + '\t';
            }
            foreach (double? db in dataRow.numberData)
            {
                line += db.ToString() + '\t';
            }
            sw.WriteLine(line.TrimEnd('\t'));
        }


        public List<Criteria> CriteriaList = new List<Criteria>();
        /// <summary>
        /// colname:目標欄位名稱, Criteria: 條件
        /// </summary>
        public class Criteria
        {
            public string key;                        //此criteria比較的目標
            public List<string> StringIncludeList, StringExcludeList;
            public double CriteriaNumUpper, CriteriaNumLower;
            public List<int> indexStrDatalist;                   //對應字串用
            public int indexNumData;                             //對應數字用
            public int indexBirthday, indexEventday, indexID;   //對應ID用
            public IEnumerable<File> IDCriteriaFileList;      //暫時儲存該組(Rgroup)對應的檔案
            public List<IDData> IDCriteriaList;               //將上述檔案內容提取入IDList
            public int indexFEE_YM, indexHOSP_ID, indexAPPL_TYPE, indexAPPL_DATE, indexCASE_TYPE, indexSEQ_NO; //對應 Action用
            public IEnumerable<File> ActionCriteriaFileList;   //暫時儲存該組(Rgroup)對應的檔案
            public List<ActionData> ActionCriteriaList;  //將上述檔案內容提取入ActionList

            //實際執行判斷的方法
            public bool DoCheck(DataRow InputDataRow)
            {
                if (key == "IDLIST")
                {
                    if (indexID == -1 || indexBirthday == -1) return true;
                    var thisID = new IDData { ID = InputDataRow.stringData[indexID], Birthday = InputDataRow.stringData[indexBirthday] };
                    if (IDCriteriaList.BinarySearch(thisID) >= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (key == "ACTIONLIST")
                {
                    if (indexHOSP_ID == -1 || indexAPPL_DATE == -1 || indexAPPL_TYPE == -1
                        || indexCASE_TYPE == -1 || indexSEQ_NO == -1 || indexFEE_YM == -1)
                        return true;
                    var thisAction = new ActionData
                    {
                        FEE_YM = InputDataRow.stringData[indexFEE_YM],
                        APPL_DATE = InputDataRow.stringData[indexAPPL_DATE],
                        APPL_TYPE = InputDataRow.stringData[indexAPPL_TYPE],
                        CASE_TYPE = InputDataRow.stringData[indexCASE_TYPE],
                        HOSP_ID = InputDataRow.stringData[indexHOSP_ID],
                        SEQ_NO = InputDataRow.numberData[indexSEQ_NO].ToString()
                    };
                    if (ActionCriteriaList.BinarySearch(thisAction) >= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (key == "AGE")
                {
                    if (indexBirthday == -1 || indexEventday == -1) return true;
                    DateTime birthday = InputDataRow.stringData[indexBirthday].StringToDate();
                    DateTime eventday = InputDataRow.stringData[indexEventday].StringToDate();
                    double age = eventday.Subtract(birthday).TotalDays / 365;
                    if ((age < CriteriaNumUpper || CriteriaNumUpper == 0) && (age >= CriteriaNumLower || CriteriaNumLower == 0))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (indexStrDatalist.Count > 0)
                {
                    bool include = false, exclude = false;
                    foreach (var index in indexStrDatalist)
                    {
                        foreach (string criteria in StringIncludeList)
                        {
                            if (InputDataRow.stringData[index].Substring(0, criteria.Length) == criteria)
                            {
                                include = true;
                            }
                        }
                        if (StringExcludeList != null && StringExcludeList.Count > 0)
                        {
                            foreach (string criteria in StringExcludeList)
                            {
                                if (InputDataRow.stringData[index].Substring(0, criteria.Length) == criteria)
                                {
                                    exclude = true;
                                }
                            }
                        }
                        if (include && !exclude)
                            return true;
                    }
                    return false;
                }
                else if (indexNumData >= 0)
                {
                    if ((InputDataRow.numberData[indexNumData] < CriteriaNumUpper || CriteriaNumUpper == 0) &&
                        (InputDataRow.numberData[indexNumData] >= CriteriaNumLower || CriteriaNumLower == 0))
                    {
                        return true;
                    }
                }
                else  //此條件沒有找到index => 跳過
                {
                    return true;
                }
                return false;
            }
        }

        #region Extract Data使用的class
        public class StringDataFormat
        {
            public string key;
            public int start_year;
            public int end_year;
            public int position;
            public int length;
            public string FileType;
        }

        public class NumberDataFormat
        {
            public string key;
            public int start_year;
            public int end_year;
            public int position;
            public int length;
            public string FileType;
        }

        public class DataRow
        {
            public string[] stringData;
            public double?[] numberData;
            public bool IsMatch;
            public DataRow(int strDataCount, int numDataCount)
            {
                stringData = new string[strDataCount];
                numberData = new double?[numDataCount];
                IsMatch = true;
            }
        }
        #endregion
    }
}
