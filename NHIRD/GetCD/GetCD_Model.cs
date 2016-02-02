using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using System.IO;
namespace NHIRD
{
    public class GetCD_Model
    {
        GetCD_Window parentWindow;
        GetCD_ViewModel parentVM;
        public GetCD_Model(GetCD_Window parent, GetCD_ViewModel parentVM)
        {
            parentWindow = parent;
            this.parentVM = parentVM;
        }

        public string str_inputDir { get; set; }
        public string str_outputDir { get; set; }
        public ObservableCollection<File> list_file = new ObservableCollection<File>();
        public ObservableCollection<Year> list_year = new ObservableCollection<Year>();
        public ObservableCollection<Group> list_group = new ObservableCollection<Group>();
        public string str_filestatus { get; set; }
        public string message { get; set; }


        public void checkFileByCriteria()
        {
            foreach (File f in list_file)
            {
                f.selected = false;
            }
            var query = (
                            from p in list_file
                            where list_year.Where(x => x.selected == true)
                            .Select(x => x.str_year)
                            .Contains(p.year) &&
                            list_group.Where(x => x.selected == true)
                            .Select(x => x.str_group)
                            .Contains(p.@group)
                            select p
                          )
                         .ToList();
            foreach (File f in query)
            {
                f.selected = true;
            }

        }

        public void ExtractData()
        {
            var stringDataFormats = new List<StringDataFormat>();
            var numberDataFormats = new List<NumberDataFormat>();
            initializeDataSet(parentWindow.parentWindow.rawDataFormats,
                stringDataFormats, numberDataFormats);
            var dataRowList = new List<DataRow>();
            ReadWriteFiles(list_file, dataRowList,
                    stringDataFormats, numberDataFormats);
        }

        public void ReadWriteFiles(IEnumerable<File> list_file,
            List<DataRow> dataRowList,
            List<StringDataFormat> stringDataFormats,
            List<NumberDataFormat> numberDataFormats)
        {
            foreach (File currentfile in list_file)
            {
                int CurrentFileYear = Convert.ToInt32(currentfile.year) - 1911;

                var queryStringDataFormats =
                     (from q in stringDataFormats
                      where CurrentFileYear >= q.start_year && CurrentFileYear <= q.end_year
                      select q).ToList();

                var queryNumberDataFormats =
                  (from q in numberDataFormats
                   where CurrentFileYear >= q.start_year && CurrentFileYear <= q.end_year
                   select q).ToList();

                ReadFile(currentfile, dataRowList,
                    queryStringDataFormats, queryNumberDataFormats);

            }
        }

        public void WriteFile(string dirPath)
        {

        }

        public void initializeDataSet(List<RawDataFormat> rawDataFormats,
            List<StringDataFormat> stringDataFormats,
            List<NumberDataFormat> numberDataFormats)
        {

            var queryStr =
                from r in rawDataFormats
                where r.FileName == "CD" && r.DataType == "C"
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
               where r.FileName == "CD" && r.DataType == "N"
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

            var data = new List<DataRow>();
        }

        public void ReadFile(File file,
            List<DataRow> dataRowList,
             List<StringDataFormat> stringDataFormats,
            List<NumberDataFormat> numberDataFormats)
        {
            using (var sr = new StreamReader(file.path))
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
            public DataRow(int strDataCount, int numDataCount)
            {
                stringData = new string[strDataCount];
                numberData = new double?[numDataCount];
            }
        }

    }

}
