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

        public StandarizeID()
        {
            baselineDayForHash = DateTime.Parse("2013-12-31");
            hashTableElementCount = 36 * 36 * 36;
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
            int counter = 0;
            foreach (string currentDistinctGroup in distinctGroupQuery)
            {
                var filesInThisGroup = from q in rawDataFileList where q.@group == currentDistinctGroup select q;
                DistinctList<StandarizedIDData>[] standarizedIDDataTable = new DistinctList<StandarizedIDData>[hashTableElementCount];
                for (int i = 0; i < hashTableElementCount; i++) standarizedIDDataTable[i] = new DistinctList<StandarizedIDData>();
                foreach (File currentFile in filesInThisGroup)
                {
                    List<StringDataFormat> stringDataFormatsForCurrentFile = getStringDataFormatsWithRightYear(currentFile);
                    List<NumberDataFormat> numberDataFormatsForCurrentFile = getNumberDataFormatsWithRightYear(currentFile);
                    int indexID = stringDataFormats.FindIndex(x => x.key == "ID");
                    int indexBirthday = stringDataFormats.FindIndex(x => x.key == "ID_BIRTHDAY");
                    int indexSex = stringDataFormats.FindIndex(x => x.key == "ID_SEX");
                    int indexInDate = stringDataFormats.FindIndex(x => x.key == "ID_IN_DATE");
                    int indexOutDate = stringDataFormats.FindIndex(x => x.key == "ID_OUT_DATE");

                    using (var sr = new StreamReader(currentFile.path, System.Text.Encoding.Default))
                    {
                        while (!sr.EndOfStream)
                        {
                            var dataRow = ReadRow(sr, stringDataFormatsForCurrentFile, numberDataFormatsForCurrentFile);
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
                            int hash = getHash(newIDData.ID);
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
                var outputFilePath = getOutputFilePath(currentDistinctGroup);
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
        private List<StringDataFormat> getStringDataFormatsWithRightYear(File currentFile)
        {
            return (from q in stringDataFormats
                    where
                        (currentFile.MKyear >= q.start_year || q.start_year == 0) &&
                        (currentFile.MKyear <= q.end_year || q.end_year == 0) &&
                        q.FileType == currentFile.FileType
                    select
                        q).ToList();
        }
        private List<NumberDataFormat> getNumberDataFormatsWithRightYear(File currentFile)
        {
            return (from q in numberDataFormats
                    where
                        (currentFile.MKyear >= q.start_year || q.start_year == 0) &&
                        (currentFile.MKyear <= q.end_year || q.end_year == 0) &&
                        q.FileType == currentFile.FileType
                    select
                        q).ToList();
        }
        private string getOutputFilePath(string group)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
            return outputDir + "\\" + $"standarized ID table_{group}.IDS";
        }
        private DataRow ReadRow(StreamReader sr, List<StringDataFormat> stringDataFormatsForCurrentFile, List<NumberDataFormat> numberDataFormatsForCurrentFile)
        {
            string line = sr.ReadLine();
            int stringDataCount = stringDataFormatsForCurrentFile.Count, numberDataCount = numberDataFormatsForCurrentFile.Count;
            var dataRow = new DataRow(stringDataCount, numberDataCount);
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

        private readonly DateTime baselineDayForHash;
        private readonly int hashTableElementCount;

        private int getHash(string ID)
        {
            var result = charToASCII(ID[0]) * 36 * 36 + charToASCII(ID[1]) * 36 + charToASCII(ID[2]);
            if (result < 0) result = 0;
            if (result > hashTableElementCount - 1) result = hashTableElementCount - 1;
            return result;
        }
        private int charToASCII(char c)
        {
            if (c < 59)
                return c - 48;
            return c - 86;
        }
    }
}
