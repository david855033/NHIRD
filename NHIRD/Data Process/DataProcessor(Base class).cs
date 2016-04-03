using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class DataProcessor
    {
        public List<RawDataFormat> rawDataFormats;
        public string[] selectedFileTypes;
        public IEnumerable<File> rawDataFileList;

        public string outputDir;
        public List<StringDataFormat> stringDataFormats = new List<StringDataFormat>();
        public List<NumberDataFormat> numberDataFormats = new List<NumberDataFormat>();

        protected List<StringDataFormat> getStringDataFormatsWithRightYear(File currentFile)
        {
            return (from q in stringDataFormats
                    where
                        (currentFile.MKyear >= q.start_year || q.start_year == 0) &&
                        (currentFile.MKyear <= q.end_year || q.end_year == 0) &&
                        q.FileType == currentFile.FileType
                    select
                        q).ToList();
        }
        protected List<NumberDataFormat> getNumberDataFormatsWithRightYear(File currentFile)
        {
            return (from q in numberDataFormats
                    where
                        (currentFile.MKyear >= q.start_year || q.start_year == 0) &&
                        (currentFile.MKyear <= q.end_year || q.end_year == 0) &&
                        q.FileType == currentFile.FileType
                    select
                        q).ToList();
        }

        public void initializeDataFormats()
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

        protected DataRow ReadRow(StreamReader sr, List<StringDataFormat> stringDataFormatsForCurrentFile, List<NumberDataFormat> numberDataFormatsForCurrentFile)
        {
            string line = sr.ReadLine();
            int stringDataCount = stringDataFormatsForCurrentFile.Count, numberDataCount = numberDataFormatsForCurrentFile.Count;
            var dataRow = new DataRow(stringDataCount, numberDataCount);
            dataRow.dataLine = line;
            for (int i = 0; i < stringDataCount; i++)
            {
                dataRow.stringData[i] =
                    line.Substring(stringDataFormatsForCurrentFile[i].position,
                    stringDataFormatsForCurrentFile[i].length);
            }
            for (int i = 0; i < numberDataCount; i++)
            {
                var data =
                    line.Substring(numberDataFormatsForCurrentFile[i].position,
                    numberDataFormatsForCurrentFile[i].length);
                if (data == "")
                {
                    dataRow.numberData[i] = null;
                }
                else
                {
                    dataRow.numberData[i] = Convert.ToDouble(data);
                }
            }
            return dataRow;
        }


        #region 內部使用的class
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
            public string dataLine;
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
