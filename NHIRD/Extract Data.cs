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
                //****這邊加入判斷式
                WriteFile(currentfile, dataRowList,
                    queryStringDataFormats, queryNumberDataFormats);
            }
        }

        public delegate bool ICDComparer(IEnumerable<string> ICDExclusions);
        List<ICDComparer> ICDIncludes, ICDExcludes;

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
                    string line = "";
                    foreach (string  s in currentData.stringData)
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

    }
}
