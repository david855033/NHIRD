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
        StreamWriter swAllCombined;

        DistinctList<PatientBasedData> PatientList;

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
            if(!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);
            using (swAllCombined = new StreamWriter(outputFolder + @"\" + "Patient Based Data _All.PBD", false, Encoding.Default))
            {
                swAllCombined.WriteLine(makeTitle());
                foreach (var Rgroup in groupList)
                {
                    foreach (var Hgroup in hashGroupList)
                    {
                        PatientList = new DistinctList<PatientBasedData>();
                        analyzeCDfilesInOneGroup(Rgroup, Hgroup);
                        analyzeDDfilesInOneGroup(Rgroup, Hgroup);
                        writeFile(Rgroup, Hgroup);
                        PatientList.Clear();
                    }
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
                    int indexSex = Array.FindIndex(title, x => x.IndexOf("SEX") >= 0);
                    int indexDate = Array.FindIndex(title, x => x.IndexOf("FUNC_DATE") >= 0);
                    int indexICDs = Array.FindIndex(title, x => x.IndexOf("ACODE_ICD9") >= 0);

                    string[] groupTitle = Array.FindAll(title, x => x.IndexOf("[order]") >= 0);
                    int[] indexGroups = new int[orderGroupList.Count];
                    for (int i = 0; i < orderGroupList.Count; i++)
                    {
                        string matchedGroupTitle = groupTitle.FirstOrDefault(x => x.Remove(0, 7) == orderGroupList[i].name);
                        if (matchedGroupTitle != default(string))
                        {
                            indexGroups[i] = Array.IndexOf(title, matchedGroupTitle);
                        }
                        else
                        {
                            indexGroups[i] = -1;
                        }

                    }


                    while (!sr.EndOfStream)
                    {
                        string[] splitline = sr.ReadLine().Split('\t');
                        string ID = splitline[indexID];
                        string Birthday = splitline[indexBirthday];
                        string gender = splitline[indexSex];
                        string[] ICDs = new string[] { splitline[indexICDs], splitline[indexICDs + 1], splitline[indexICDs + 2] };
                        string[] orders = new string[orderGroupList.Count];
                        for (int i = 0; i < orderGroupList.Count; i++)
                        {
                            if (indexGroups[i] >= 0)
                            {
                                orders[i] = splitline[indexGroups[i]];
                            }

                            else
                            {
                                orders[i] = "";
                            }
                        }

                        string date = splitline[indexDate];
                        var NewPatient = new PatientBasedData() { ID = ID, Birthday = Birthday };

                        int index = PatientList.BinarySearch(NewPatient);
                        if (index < 0)
                        {
                            index = ~index;
                            NewPatient.setDiagnosisDetail(diagnosisGroupList.Count);
                            NewPatient.setOrderDetail(orderGroupList.Count);
                            PatientList.Insert(index, NewPatient);
                        }
                        PatientList[index].CDcount++;
                        PatientList[index].gender = gender;
                        for (int i = 0; i < diagnosisGroupList.Count; i++)
                        {
                            if (diagnosisGroupList[i].isThisGroupMatched(ICDs))
                            {
                                PatientList[index].diagnosisDetails[i].CDCount++;
                                PatientList[index].diagnosisDetails[i].firstDate = date;
                            }
                        }

                        for (int i = 0; i < orderGroupList.Count; i++)
                        {
                            if (orders[i] != "" && Convert.ToInt32(orders[i]) > 0)
                            {
                                PatientList[index].orderDetails[i].CDCount++;
                                PatientList[index].orderDetails[i].firstDate = date;
                            }
                        }
                    }
                }
            }
        }

        void analyzeDDfilesInOneGroup(string Rgroup, string Hgroup)
        {
            var DDFileQuery = from q in inputFile
                              where q.FileType == "DD"
                                && q.@group == Rgroup
                                && q.hashGroup == Hgroup
                              select q;
            foreach (var DDFile in DDFileQuery)
            {
                using (var sr = new StreamReader(DDFile.path, Encoding.Default))
                {
                    string[] title = sr.ReadLine().Split('\t');
                    int indexID = Array.IndexOf(title, "ID");
                    int indexBirthday = Array.FindIndex(title, x => x.IndexOf("BIRTHDAY") >= 0);
                    int indexSex = Array.FindIndex(title, x => x.IndexOf("SEX") >= 0);
                    int indexDate = Array.FindIndex(title, x => x.IndexOf("IN_DATE") >= 0);
                    int indexICDs = Array.FindIndex(title, x => x.IndexOf("ICD9CM_CODE") >= 0);

                    string[] groupTitle = Array.FindAll(title, x => x.IndexOf("[order]") >= 0);
                    int[] indexGroups = new int[orderGroupList.Count];
                    for (int i = 0; i < orderGroupList.Count; i++)
                    {
                        string matchedGroupTitle = groupTitle.FirstOrDefault(x => x.Remove(0, 7) == orderGroupList[i].name);
                        if (matchedGroupTitle != default(string))
                        {
                            indexGroups[i] = Array.IndexOf(title, matchedGroupTitle);
                        }
                        else
                        {
                            indexGroups[i] = -1;
                        }

                    }


                    while (!sr.EndOfStream)
                    {
                        string[] splitline = sr.ReadLine().Split('\t');
                        string ID = splitline[indexID];
                        string Birthday = splitline[indexBirthday];
                        string gender = splitline[indexSex];
                        string[] ICDs = new string[] { splitline[indexICDs], splitline[indexICDs + 1], splitline[indexICDs + 2], splitline[indexICDs + 3], splitline[indexICDs + 4] };
                        string[] orders = new string[orderGroupList.Count];
                        for (int i = 0; i < orderGroupList.Count; i++)
                        {
                            if (indexGroups[i] >= 0)
                            {
                                orders[i] = splitline[indexGroups[i]];
                            }

                            else
                            {
                                orders[i] = "";
                            }
                        }

                        string date = splitline[indexDate];
                        var NewPatient = new PatientBasedData() { ID = ID, Birthday = Birthday };

                        int index = PatientList.BinarySearch(NewPatient);
                        if (index < 0)
                        {
                            index = ~index;
                            NewPatient.setDiagnosisDetail(diagnosisGroupList.Count);
                            NewPatient.setOrderDetail(orderGroupList.Count);
                            PatientList.Insert(index, NewPatient);
                        }
                        PatientList[index].DDcount++;
                        PatientList[index].gender = gender;
                        for (int i = 0; i < diagnosisGroupList.Count; i++)
                        {
                            if (diagnosisGroupList[i].isThisGroupMatched(ICDs))
                            {
                                PatientList[index].diagnosisDetails[i].DDCount++;
                                PatientList[index].diagnosisDetails[i].firstDate = date;
                            }
                        }

                        for (int i = 0; i < orderGroupList.Count; i++)
                        {
                            if (orders[i] != "" && Convert.ToInt32(orders[i]) > 0)
                            {
                                PatientList[index].orderDetails[i].DDCount++;
                                PatientList[index].orderDetails[i].firstDate = date;
                            }
                        }
                    }
                }
            }
        }

        string makeTitle()
        {
            StringBuilder title = new StringBuilder(PatientBasedData.toTitile());
            foreach (var g in diagnosisGroupList)
                title.Append(EventDetail.toTitle(g.name));
            foreach (var g in orderGroupList)
                title.Append(EventDetail.toTitle("[order]" + g.name));
            return title.ToString();
        }
        bool checkDiagnosisGroup(IEnumerable<string> ICDs)
        {
            bool result = false;
            foreach (var d in diagnosisGroupList)
            {
                if (result = d.isThisGroupMatched(ICDs)) break;
            }
            return result;
        }

        void writeFile(string Rgroup, string Hgroup)
        {
            string PostFix = (Rgroup == "NA" ? "" : ("_" + Rgroup)) + (Hgroup == "NA" ? "" : ("_" + Hgroup));
            string filePath = outputFolder + @"\" + "Patient Based Data" + PostFix + ".PBD";
            using (var sw = new StreamWriter(filePath, false, Encoding.Default))
            {
                sw.WriteLine(makeTitle());
                foreach (var p in PatientList)
                {
                    sw.WriteLine(p.toWriteLine());
                    swAllCombined.WriteLine(p.toWriteLine());
                }
            }
        }
    }


}
