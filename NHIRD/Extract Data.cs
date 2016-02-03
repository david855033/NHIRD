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
            initializeDataSet(rawDataFormats);

            //逐個檔案執行讀寫
            ReadWriteFiles();
        }

        void initializeDataSet(List<RawDataFormat> rawDataFormats)
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
                WriteFile(outputDir, dataRowList,
                    queryStringDataFormats, queryNumberDataFormats);
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

                    var newDataRow = new DataRow(stringDataFormats.Count(),
                        numberDataFormats.Count());

                    for (int i = 0; i < stringDataFormats.Count(); i++)
                    {
                        newDataRow.stringData[i] =
                            line.Substring(stringDataFormats[i].position,
                            stringDataFormats[i].length);
                    }

                    for (int i = 0; i < numberDataFormats.Count(); i++)
                    {
                        var data =
                            line.Substring(numberDataFormats[i].position,
                            numberDataFormats[i].length);
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

        void WriteFile(string dirPath, List<DataRow> dataRowList,
        List<StringDataFormat> stringDataFormats,
            List<NumberDataFormat> numberDataFormats)
        {

        }

    }
}
