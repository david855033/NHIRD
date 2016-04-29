using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class PatientBasedDataGenerator
    {
        readonly List<File> inputFile;
        readonly List<OrderGroup> orderGroupList;
        readonly List<DiagnosisGroup> diagnosisGroupList;
        readonly string outputFolder;

        List<PatientBasedData> PatientList = new List<PatientBasedData>();
        

        public PatientBasedDataGenerator(IEnumerable<File> inputFile, IEnumerable<OrderGroup> orderGroupList, IEnumerable<DiagnosisGroup> diagnosisGroupList, string outputFolder)
        {
            this.inputFile = new List<File>(inputFile);
            this.orderGroupList = new List<OrderGroup>(orderGroupList);
            this.diagnosisGroupList = new List<DiagnosisGroup>(diagnosisGroupList);
            this.outputFolder = outputFolder;
        }

        public void Do()
        {
            System.Windows.MessageBox.Show($"input file count = {inputFile.Count}, group count = {orderGroupList.Count}, dx count = {diagnosisGroupList.Count}, output folder = {outputFolder}");
            
        }

        void analyzeCDfiles()
        {
            var CDFileQuery = from q in inputFile where q.FileType == "CD" select q;
            foreach(var CDFile in CDFileQuery)
            {
                using (var sr = new StreamReader(CDFile.path, Encoding.Default))
                {
                    string[] title = sr.ReadLine().Split('\t');
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                    }
                }
            }
        }
    }

    class DetailOfDiagnosisGroup
    {
        public DateTime firstDate;
        public double firstAge;
        public int CDCount, DDCount;
        public string toString()
        {
            return "TODO";
        }
    }
}
