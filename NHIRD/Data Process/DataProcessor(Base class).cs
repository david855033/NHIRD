using System;
using System.Collections.Generic;
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

        // step 1
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
