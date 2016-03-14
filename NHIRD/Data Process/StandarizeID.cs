using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NHIRD
{
    class StandarizeID: DataProcessor
    {


        public void Do(List<RawDataFormat> rawDataFormats, IEnumerable<File> files, string outputDir)
        {
            ///使用rawDataFormats初始化資料清單(分為數值資料跟字串資料)
            this.selectedFileTypes = new string[] { "ID" };
            this.rawDataFormats = rawDataFormats;
            this.outputDir = outputDir;
            this.rawDataFileList = files;

            initializeDataFormats();

            MessageBox.Show($"stringDataFormats={stringDataFormats.Count()} numberDataFormats={numberDataFormats.Count()}");
        }

        void ReadAndStandarizeIDFile()
        {
            var distinctGroupQuery = (from q in rawDataFileList where q.selected == true select q.@group).Distinct();
            foreach (string currentDistinctGroup in distinctGroupQuery)
            {
                var filesInThisGroup = from q in rawDataFileList where q.@group == currentDistinctGroup select q;
            }
        }
        
    }
}
