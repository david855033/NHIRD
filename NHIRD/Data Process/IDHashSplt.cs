using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class IDHashSplitter : DataProcessor
    {

        public void Do(List<RawDataFormat> rawDataFormats, IEnumerable<File> files, string outputDir)
        {
            ///使用rawDataFormats初始化資料清單(分為數值資料跟字串資料)
            this.selectedFileTypes = new string[] { "ID" };
            this.rawDataFormats = rawDataFormats;
            this.outputDir = outputDir;
            this.rawDataFileList = files;

            initializeDataFormats();
            SplitIDDataInFiles();
        }

        private void SplitIDDataInFiles()
        {
            initiateStreamWriterArray();
            var selectedFile = (from q in rawDataFileList where q.selected == true select q);
            foreach (var currentFile in selectedFile)
            {
                SplitIDData(currentFile);
            }
            closeStreamWriterArray();
        }
        private void SplitIDData(File currentFile)
        {
            List<StringDataFormat> stringDataFormatsForCurrentFile = getStringDataFormatsWithRightYear(currentFile);
            List<NumberDataFormat> numberDataFormatsForCurrentFile = getNumberDataFormatsWithRightYear(currentFile);
            int indexID = stringDataFormatsForCurrentFile.FindIndex(x => x.key == "ID");
            using (var sr = new StreamReader(currentFile.path, System.Text.Encoding.Default))
            {
                
                while (!sr.EndOfStream)
                {
                    var dataRow = ReadRow(sr, stringDataFormatsForCurrentFile, numberDataFormatsForCurrentFile);
                    uint t = HashByIDGenerator.GenerateHash(dataRow.stringData[indexID]);
                    swArray[t].WriteLine(dataRow.dataLine);
                }
            }
        }
        StreamWriter[] swArray = new StreamWriter[256];
        private void initiateStreamWriterArray()
        {
            for (int i = 0; i < swArray.Length; i++)
            {
                var outputFilePath = getOutputFilePath(i);
                swArray[i] = new StreamWriter(outputFilePath, false, System.Text.Encoding.Default);
            }
        }
        private string getOutputFilePath(int hash)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
            return outputDir + "\\" + $"ID splitted_H{hash}.DAT";
        }
        private void closeStreamWriterArray()
        {
            foreach (var sw in swArray) sw.Close();
        }
        
    }

}
