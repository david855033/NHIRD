using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NHIRD
{
    class StandarizeID : DataProcessor
    {
        private int _birthYearUpperLimit;
        public int birthYearUpperLimit
        {
            get { return _birthYearUpperLimit; }
            set { _birthYearUpperLimit = value; }
        }
        private int _birthYearLowerLimit;
        public int birthYearLowerLimit
        {
            get { return _birthYearLowerLimit; }
            set { _birthYearLowerLimit = value; }
        }

        public StandarizeID()
        {

            ; hashTableElementCount = 16 * 16 * 16 * 16;
        }

        public void Do(List<RawDataFormat> rawDataFormats, IEnumerable<File> files, string outputDir)
        {
            ///使用rawDataFormats初始化資料清單(分為數值資料跟字串資料)
            this.selectedFileTypes = new string[] { "ID" };
            this.rawDataFormats = rawDataFormats;
            this.outputDir = outputDir;
            this.rawDataFileList = files;

            initializeDataFormats();
            ReadAndStandarizeIDFile();
        }

        private void ReadAndStandarizeIDFile()
        {
            var distinctGroupQuery = (from q in rawDataFileList where q.selected == true select q.@group).Distinct();
            var distinctHashGroupQuery = (from q in rawDataFileList where q.selected == true select q.hashGroup).Distinct();
            int counter = 0;
            foreach (string currentDistinctGroup in distinctGroupQuery)
            {
                foreach (string currentDistinctHashGroup in distinctHashGroupQuery)
                {

                    var filesInThisGroup = from q in rawDataFileList
                                           where (q.@group == currentDistinctGroup
                                           && q.hashGroup == currentDistinctHashGroup
                                           && q.selected == true)
                                           select q;
                    DistinctList<StandarizedIDData>[] standarizedIDDataTable = new DistinctList<StandarizedIDData>[hashTableElementCount];
                    for (int i = 0; i < hashTableElementCount; i++) standarizedIDDataTable[i] = new DistinctList<StandarizedIDData>();
                    foreach (File currentFile in filesInThisGroup)
                    {
                        List<StringDataFormat> stringDataFormatsForCurrentFile = getStringDataFormatsWithRightYear(currentFile);
                        List<NumberDataFormat> numberDataFormatsForCurrentFile = getNumberDataFormatsWithRightYear(currentFile);
                        int indexID = stringDataFormatsForCurrentFile.FindIndex(x => x.key == "ID");
                        int indexBirthday = stringDataFormatsForCurrentFile.FindIndex(x => x.key == "ID_BIRTHDAY");
                        int indexSex = stringDataFormatsForCurrentFile.FindIndex(x => x.key == "ID_SEX");
                        int indexInDate = stringDataFormatsForCurrentFile.FindIndex(x => x.key == "ID_IN_DATE");
                        int indexOutDate = stringDataFormatsForCurrentFile.FindIndex(x => x.key == "ID_OUT_DATE");

                        using (var sr = new StreamReader(currentFile.path, Encoding.Default))
                        {
                            while (!sr.EndOfStream)
                            {
                                var dataRow = ReadRow(sr, stringDataFormatsForCurrentFile, numberDataFormatsForCurrentFile);

                                if (!isMatchBirthYearRange(dataRow.stringData[indexBirthday]))
                                    continue;

                                var newIDData = new StandarizedIDData()
                                {
                                    ID = dataRow.stringData[indexID],
                                    Birthday = dataRow.stringData[indexBirthday],
                                    isMale = dataRow.stringData[indexSex] == "M",
                                    firstInDate = dataRow.stringData[indexInDate].StringToDate(),
                                    lastInDate = dataRow.stringData[indexInDate].StringToDate(),
                                    firstOutDate = dataRow.stringData[indexOutDate].StringToDate(),
                                    lastOutDate = dataRow.stringData[indexOutDate].StringToDate()
                                };
                                uint hash = getIDHash(newIDData);
                                int index = standarizedIDDataTable[hash].AddDistinct(newIDData);
                                if (index >= 0)
                                {
                                    standarizedIDDataTable[hash][index].isMale = newIDData.isMale;
                                    standarizedIDDataTable[hash][index].firstInDate = newIDData.firstInDate;
                                    standarizedIDDataTable[hash][index].firstOutDate = newIDData.firstOutDate;
                                    standarizedIDDataTable[hash][index].lastInDate = newIDData.lastInDate;
                                    standarizedIDDataTable[hash][index].lastOutDate = newIDData.lastOutDate;
                                }
                                else
                                {
                                    counter++;
                                }
                            }
                        }
                    }
                    var outputFilePath = getOutputFilePath(currentDistinctGroup, currentDistinctHashGroup);
                    using (var sw = new StreamWriter(outputFilePath, false, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(StandarizedIDData.ToTitle());
                        foreach (var thisTable in standarizedIDDataTable)
                            foreach (var thisstandarizedIDData in thisTable)
                            {
                                sw.WriteLine(thisstandarizedIDData.ToWriteLine());
                            }
                    }
                }
            }
        }

        private string getOutputFilePath(string group, string hashGroup)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
            if (hashGroup == "NA")
            {
                return outputDir + "\\" + $"standarized ID table_{group}.IDS";
            }
            else
            {
                return outputDir + "\\" + $"standarized ID table_{hashGroup}.IDS";
            }
        }

        private readonly int hashTableElementCount;
        private uint getIDHash(StandarizedIDData inputIDData) //使用第二組四位數來作為hash (第一組留給split group by hash用)
        {
            return uint.Parse(inputIDData.ID.Substring(4, 4), System.Globalization.NumberStyles.HexNumber);
        }
        private int charToASCII(char c)
        {
            if (c < 59)
                return c - 48;
            return c - 86;
        }
        private bool isMatchBirthYearRange(string birthdate)
        {
            DateTime dt = birthdate.StringToDate();
            try
            {
                bool result = (dt.Year >= birthYearLowerLimit || _birthYearLowerLimit == default(int)) &&
                    (dt.Year <= _birthYearUpperLimit || _birthYearUpperLimit == default(int));
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}
