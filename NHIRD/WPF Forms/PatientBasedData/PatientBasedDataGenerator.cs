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

        DistinctList<PatientBasedData> PatientList = new DistinctList<PatientBasedData>();

        public PatientBasedDataGenerator(IEnumerable<File> inputFile, IEnumerable<OrderGroup> orderGroupList, IEnumerable<DiagnosisGroup> diagnosisGroupList, string outputFolder)
        {
            this.inputFile = new List<File>(inputFile);
            this.orderGroupList = new List<OrderGroup>(orderGroupList);
            this.diagnosisGroupList = new List<DiagnosisGroup>(diagnosisGroupList);
            this.outputFolder = outputFolder;
        }

        public void Do()
        {
            var groupList = (from q in inputFile where q.FileType == "CD" || q.FileType == "DD" select q.@group).Distinct();
            var hashGroupList = (from q in inputFile where q.FileType == "CD" || q.FileType == "DD" select q.hashGroup).Distinct();

            System.Windows.MessageBox.Show($"input file count = {inputFile.Count}, group count = {orderGroupList.Count}, dx count = {diagnosisGroupList.Count}, output folder = {outputFolder}, groupList ={groupList.Count()}, hashGroupList = {hashGroupList.Count()}");
            foreach (var Rgroup in groupList)
            {
                foreach (var Hgroup in hashGroupList)
                {
                    analyzeCDfilesInOneGroup(Rgroup, Hgroup);
                    writeFile(Rgroup, Hgroup);
                }
            }

        }
        void analyzeCDfilesInOneGroup(string Rgroup, string Hgroup)
        {
            var CDFileQuery = from q in inputFile
                              where q.FileType == "CD"
                                && q.@group == Rgroup
                                && q.hashGroup == Hgroup
                              select q;
            foreach (var CDFile in CDFileQuery)
            {
                using (var sr = new StreamReader(CDFile.path, Encoding.Default))
                {
                    string[] title = sr.ReadLine().Split('\t');
                    int indexID = Array.IndexOf(title, "ID");
                    int indexBirthday = Array.FindIndex(title, x => x.IndexOf("BIRTHDAY") >= 0);

                    while (!sr.EndOfStream)
                    {
                        string[] splitline = sr.ReadLine().Split('\t');
                        string ID = splitline[indexID];
                        string Birthday = splitline[indexBirthday];
                        var NewPatient = new PatientBasedData() { ID = ID, Birthday = Birthday };
                        int index = PatientList.BinarySearch(NewPatient);
                        if (index < 0)
                        {
                            index = ~index;
                            PatientList.Insert(index, NewPatient);
                        }
                        PatientList[index].CDcount++;
                    }
                }
            }
        }

        void writeFile(string Rgroup, string Hgroup)
        {
            string PostFix = (Rgroup == "NA" ? ("_" + Rgroup) : "") + (Hgroup == "NA" ? ("_" + Hgroup) : "");
            string fileName = outputFolder + @"\" + "Patient Based Data" + PostFix + ".PBD";
        }
    }


}
