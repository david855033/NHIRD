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
        string selectedFileType;
        IEnumerable<File> rawDataFileList;

        string outputDir;
        List<StringDataFormat> stringDataFormats = new List<StringDataFormat>();
        List<NumberDataFormat> numberDataFormats = new List<NumberDataFormat>();
        public void Do(List<RawDataFormat> rawDataFormats, string selectedFileType, IEnumerable<File> list_file, string outputDir)
        {
            ///使用rawDataFormats初始化資料清單(分為數值資料跟字串資料)
            this.selectedFileType = selectedFileType;
            this.rawDataFormats = rawDataFormats;
            this.outputDir = outputDir;
            this.rawDataFileList = list_file;
            initializeDataSet();

            //逐個檔案執行讀寫
            ReadJudgeWriteFiles();
        }
        /// <summary>
        /// 載入指定selectedFileType的格式資料，並且儲存在numberDataFormats及stringDataFormats中
        /// </summary>
        /// <param name="rawDataFormats"></param>
        void initializeDataSet()
        {
            var queryStr =
                from r in rawDataFormats
                where r.FileName == selectedFileType && r.DataType == "C"
                select r;
            foreach (var q in queryStr)
            {
                var toAdd = new StringDataFormat()
                {
                    name = q.ColumnName,
                    position = q.Postion,
                    length = q.Lengths,
                    start_year = q.start_year,
                    end_year = q.end_year
                };
                stringDataFormats.Add(toAdd);
            }


            var queryNum =
               from r in rawDataFormats
               where r.FileName == selectedFileType && r.DataType == "N"
               select r;
            foreach (var q in queryNum)
            {
                var toAdd = new NumberDataFormat()
                {
                    name = q.ColumnName,
                    position = q.Postion,
                    length = q.Lengths,
                    start_year = q.start_year,
                    end_year = q.end_year
                };
                numberDataFormats.Add(toAdd);
            }
        }
        /// <summary>
        /// 逐個檔案進行讀寫+判斷
        /// </summary>
        void ReadJudgeWriteFiles()
        {

            //先分析總共有多少組別
            var groupQuery = (from q in rawDataFileList where q.selected == true select q.@group).Distinct();
            //逐個組別進行(若是有比較的資料 如ID / SEQ，則先載入比較用的list)
            foreach (string g in groupQuery)
            {
                var current_list_file = from q in rawDataFileList where q.@group == g select q;
                if (CriteriaList.Any(x => x.colname == "IDLIST") == true)//有使用"IDLIST"的條件，需要載入IDLIST
                {
                    var NewIDCriteriaList = new DistinctList<IDData>();
                    //搜尋同組的File
                    var FilesOfTheGroup = from q in CriteriaList.Find(x => x.colname == "IDLIST").IDCriteriaFileList
                                          where q.@group == g
                                          select q;
                    //載入同組File進入IDList，儲存在該筆criteria中
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
                                string Birthday = linesplit[indexBirthday];
                                string ID = linesplit[indexID];
                                NewIDCriteriaList.AddDistinct(new IDData { ID = ID, Birthday = Birthday });
                            }
                        }
                    }
                    CriteriaList.Find(x => x.colname == "IDLIST").IDCriteriaList = NewIDCriteriaList;
                }
                // 逐個檔案進行
                foreach (File currentfile in current_list_file)
                {
                    //依照年分挑出正確的Dataformat，傳入Readfile及Writefile
                    var queryStringDataFormats =
                         (from q in stringDataFormats
                          where currentfile.MKyear >= q.start_year && currentfile.MKyear <= q.end_year
                          select q).ToList();
                    var queryNumberDataFormats =
                      (from q in numberDataFormats
                       where currentfile.MKyear >= q.start_year && currentfile.MKyear <= q.end_year
                       select q).ToList();
                    //建立資料清單，開始讀寫
                    var dataRowList = new List<DataRow>();
                    ReadFile(currentfile, dataRowList,
                        queryStringDataFormats, queryNumberDataFormats);

                    JudgeFile(currentfile, dataRowList,
                       queryStringDataFormats, queryNumberDataFormats);

                    WriteFile(currentfile, dataRowList,
                        queryStringDataFormats, queryNumberDataFormats);
                }
            }
        }

        void JudgeFile(File currentfile, List<DataRow> dataRowList,
        List<StringDataFormat> queryStringDataFormats, List<NumberDataFormat> queryNumberDataFormats)
        {
            //逐個criteria確定要判斷的欄位index
            foreach (var currentCriteria in CriteriaList)
            {
                //重置criteriaList內之indexList(可對應到當前檔案)
                currentCriteria.indexStrDatalist = new List<int>();
                currentCriteria.indexNumData = -1;

                //將想要判斷的資料之index找出來，丟入criteria的物件中
                //ID list
                if (currentCriteria.colname == "IDLIST")
                {
                    currentCriteria.indexBirthday = queryStringDataFormats.FindIndex(x => x.name.IndexOf("BIRTHDAY") >= 0);
                    currentCriteria.indexID = queryStringDataFormats.FindIndex(x => x.name == "ID");
                }
                //年齡資料
                else if (currentCriteria.colname == "AGE")
                {
                    currentCriteria.indexBirthday = queryStringDataFormats.FindIndex(x => x.name.IndexOf("BIRTHDAY") >= 0);
                    currentCriteria.indexEventday = queryStringDataFormats.FindIndex(x => x.name == "FUNC_DATE");
                    if (currentCriteria.indexEventday < 0)
                        currentCriteria.indexEventday = queryStringDataFormats.FindIndex(x => x.name == "IN_DATE");
                }
                else
                {
                    //字串資料(同時比較複數欄位)
                    foreach (var currentColName in
                        from q in queryStringDataFormats
                        where q.name.IndexOf(currentCriteria.colname) >= 0
                        select q.name)
                    {
                        int r = queryStringDataFormats.FindIndex(x => x.name == currentColName);
                        if (r >= 0)
                        {
                            currentCriteria.indexStrDatalist.Add(r);
                        }
                    }
                    if (currentCriteria.indexStrDatalist.Count > 0) continue;
                    //數字資料(單一欄位)
                    var qColName =
                        (from q in queryNumberDataFormats
                         where q.name.IndexOf(currentCriteria.colname) >= 0
                         select q.name);
                    if (qColName.Count() > 0)
                    {
                        int R = queryNumberDataFormats.FindIndex(x => x.name == qColName.First());
                        if (R >= 0)
                        {
                            currentCriteria.indexNumData = R;
                        }
                    }
                }
            }

            //逐筆資料比對(使用criteria.DoCheck)，只要有一項不相符就將該筆DataRow.IsMatch設定為neg，並跳過該筆資料
            foreach (var currentDataRow in dataRowList)
            {
                foreach (var currentCriteria in CriteriaList)
                {
                    if (currentDataRow.IsMatch == false) continue;
                    currentDataRow.IsMatch = currentCriteria.DoCheck(currentDataRow);
                }
            }

        }

        void ReadFile(File currentfile, List<DataRow> dataRowList,
        List<StringDataFormat> queryStringDataFormats,
          List<NumberDataFormat> queryNumberDataFormats)
        {
            using (var sr = new StreamReader(currentfile.path))
            {

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    var newDataRow = new DataRow(queryStringDataFormats.Count(),
                        queryNumberDataFormats.Count());

                    for (int i = 0; i < queryStringDataFormats.Count(); i++)
                    {
                        newDataRow.stringData[i] =
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
                            newDataRow.numberData[i] = null;
                        }
                        else
                        {
                            newDataRow.numberData[i] = Convert.ToDouble(data);
                        }
                    }

                    dataRowList.Add(newDataRow);
                }
            }
        }

        void WriteFile(File currentfile, List<DataRow> dataRowList,
        List<StringDataFormat> queryStringDataFormats,
            List<NumberDataFormat> queryNumberDataFormats)
        {
            //檢查目錄是否存在
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string outpath = outputDir + "\\" + currentfile.name.Split('.')[0] + ".EXT";
            using (var sw = new StreamWriter(outpath))
            {
                string title = "";
                foreach (var q in queryStringDataFormats)
                {
                    title += q.name + '\t';
                }

                foreach (var q in queryNumberDataFormats)
                {
                    title += q.name + '\t';
                }
                sw.WriteLine(title.TrimEnd('\t'));

                foreach (var currentData in dataRowList)
                {
                    if (!currentData.IsMatch) continue;
                    string line = "";
                    foreach (string s in currentData.stringData)
                    {
                        line += s + '\t';
                    }
                    foreach (double? db in currentData.numberData)
                    {
                        line += db.ToString() + '\t';
                    }
                    sw.WriteLine(line.TrimEnd('\t'));
                }
            }
        }

        public List<Criteria> CriteriaList = new List<Criteria>();
        /// <summary>
        /// colname:目標欄位名稱, Criteria: 條件
        /// </summary>
        public class Criteria
        {
            public string colname;
            public List<string> StringList;
            public List<int> indexStrDatalist;
            public double CriteriaNumUpper, CriteriaNumLower;
            public int indexNumData;
            public int indexBirthday, indexEventday, indexID;
            public IEnumerable<File> IDCriteriaFileList;
            public List<IDData> IDCriteriaList;
            //實際執行判斷的方法
            public bool DoCheck(DataRow InputDataRow)
            {
                if (colname == "IDLIST")
                {
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
                else if (colname == "AGE")
                {
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
                    foreach (var index in indexStrDatalist)
                    {
                        foreach (string criteria in StringList)
                            if (InputDataRow.stringData[index].Substring(0, criteria.Length) == criteria)
                            {
                                return true;
                            }
                    }
                    return false;
                }
                else
                {
                    if ((InputDataRow.numberData[indexNumData] < CriteriaNumUpper || CriteriaNumUpper == 0) &&
                        (InputDataRow.numberData[indexNumData] >= CriteriaNumLower || CriteriaNumLower == 0))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #region Extract Data使用的class
        public class StringDataFormat
        {
            public string name;
            public int start_year;
            public int end_year;
            public int position;
            public int length;
        }

        public class NumberDataFormat
        {
            public string name;
            public int start_year;
            public int end_year;
            public int position;
            public int length;
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
