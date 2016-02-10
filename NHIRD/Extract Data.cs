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
        IEnumerable<File> list_file;
        string outputDir;
        List<StringDataFormat> stringDataFormats = new List<StringDataFormat>();
        List<NumberDataFormat> numberDataFormats = new List<NumberDataFormat>();
        public void Do(List<RawDataFormat> rawDataFormats, string selectedFileType, IEnumerable<File> list_file, string outputDir)
        {

            ///使用rawDataFormats初始化資料清單(分為數值資料跟字串資料)
            this.selectedFileType = selectedFileType;
            this.rawDataFormats = rawDataFormats;
            this.outputDir = outputDir;
            this.list_file = list_file;
            initializeDataSet();

            //逐個檔案執行讀寫
            ReadWriteFiles();
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
        void ReadWriteFiles()
        {

            // 逐個檔案進行
            foreach (File currentfile in list_file)
            {
                //依照年分挑出正確的Dataformat，傳入Readfile及Writefile
                int CurrentFileYear = Convert.ToInt32(currentfile.year) - 1911;
                var queryStringDataFormats =
                     (from q in stringDataFormats
                      where CurrentFileYear >= q.start_year && CurrentFileYear <= q.end_year
                      select q).ToList();
                var queryNumberDataFormats =
                  (from q in numberDataFormats
                   where CurrentFileYear >= q.start_year && CurrentFileYear <= q.end_year
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
            public int indexBirthday, indexEventday;
            public bool DoCheck(DataRow InputDataRow)
            {
                if (colname == "AGE")
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

        void JudgeFile(File currentfile, List<DataRow> dataRowList,
        List<StringDataFormat> queryStringDataFormats, List<NumberDataFormat> queryNumberDataFormats)
        {
            //逐個criteria
            foreach (var currentCriteria in CriteriaList)
            {
                //重置criteriaList內之indexList(可對應到當前檔案)
                currentCriteria.indexStrDatalist = new List<int>();
                currentCriteria.indexNumData = -1;

                //尋找:符合criteria內指定的colname的index並且更新indexList
                //年齡資料
                if (currentCriteria.colname == "AGE")
                {
                    currentCriteria.indexBirthday = queryStringDataFormats.FindIndex(x => x.name.IndexOf("BIRTHDAY") >= 0);
                    currentCriteria.indexEventday = queryStringDataFormats.FindIndex(x => x.name == "FUNC_DATE");
                    if (currentCriteria.indexEventday < 0)
                        currentCriteria.indexEventday = queryStringDataFormats.FindIndex(x => x.name == "IN_DATE");
                }
                //字串資料
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
                //數字資料
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

            //逐筆資料比對
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
