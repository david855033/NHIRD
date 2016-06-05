using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace NHIRD
{
    class AgeSpecificIncidenceCalculator
    {
        IEnumerable<MatchOfPatiendBasedDataAndStandarizedID> matchList;
        List<string> EventNames;
        List<int> EventIndex;
        List<PatientBasedDataWithEvent> patientBasedDataWithEvents;
        bool IDCriteraiEnable;
        List<IDData> IDCriteria;
        AgeSpecificIncidenceTable[] ASITables;
        DateTime dataEndDate;
        string outputDir;
        public AgeSpecificIncidenceCalculator(IEnumerable<MatchOfPatiendBasedDataAndStandarizedID> matchList)
        {
            this.matchList = matchList;
        }

        public void Do(DateTime dataEndDate, string outputDir)
        {
            this.dataEndDate = dataEndDate;
            this.outputDir = outputDir;
            analyzeTitleOfPatientBasedDataAndGetEventNames(matchList.First().patientBasedData.path);
            initializeAgeSpecificIncidenceTable();
            foreach (var currentMatch in matchList)
            {
                CalculateOneMatch(currentMatch);
            }
            createOutputDir(outputDir);
            writeASItables();
            // read PBD -> 先從title分析group list後 初始化List<string> eventNames, OK
            // 初始化ASI table, 欄位: Age  男-人年 -- 女-人年 - 男EventCount -  女EventCount , 以此做數個陣列(每個eventname用一個table)

            // 再產生List<patient>(含ID/性別/Event[](field: hasThisEvent, eventAge))，再讀取每個病人資料

            // 逐筆讀取Standarize ID, 比對有無出現在PBD裡面，然後增加所有ASI table的人年 以及對應的ASI table的evnetcount數量           
            // 對每個match重複以上動作
            // 輸出ASI
        }

        private void createOutputDir(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
        }

        private void analyzeTitleOfPatientBasedDataAndGetEventNames(string anyPatientBasedDataPath)
        {
            EventNames = new List<string>();
            using (var sr = new StreamReader(anyPatientBasedDataPath, Encoding.Default))
            {
                string title = sr.ReadLine();
                string[] split = title.Split('\t');
                var query = from q in split where q.IndexOf("-最早年紀") >= 0 select q;
                foreach (var s in query)
                {
                    EventNames.Add(s.Replace("-最早年紀", ""));
                }
            }
        }
        private void initializeAgeSpecificIncidenceTable()
        {
            ASITables = new AgeSpecificIncidenceTable[EventNames.Count];
            for (int i = 0; i < ASITables.Count(); i++)
            {
                ASITables[i] = AgeSpecificIncidenceTable.getNewAgeSpecificIncidenceTable(EventNames[i]);
            }
        }


        private void CalculateOneMatch(MatchOfPatiendBasedDataAndStandarizedID currentMatch)
        {
            IDCriteraiEnable = false;
            readPBD(currentMatch.patientBasedData.path);
            if (currentMatch.IDincludeCriteria != null)
            {
                IDCriteraiEnable = true;
                readIDCriteria(currentMatch.IDincludeCriteria.path);
            }
            readSID(currentMatch.standarizedID.path);
        }

        private void readPBD(string filePath)
        {
            using (var sr = new StreamReader(filePath, Encoding.Default))
            {
                patientBasedDataWithEvents = new List<PatientBasedDataWithEvent>();
                string[] titles = sr.ReadLine().Split('\t');
                initializeEventIndex(titles);
                int index_ID = Array.IndexOf(titles, "ID");
                int index_Birthday = Array.IndexOf(titles, "Birthday");
                int index_Gender = Array.IndexOf(titles, "Gender");
                while (!sr.EndOfStream)
                {
                    string[] splitline = sr.ReadLine().Split('\t');
                    PatientBasedDataWithEvent toAdd = new PatientBasedDataWithEvent()
                    {
                        ID = splitline[index_ID],
                        Birthday = splitline[index_Birthday],
                        gender = splitline[index_Gender] == "F" ? Gender.Female : Gender.Male
                    };
                    foreach (var i in EventIndex)
                    {
                        double age = -1;
                        if (splitline[i] != "")
                        {
                            age = Convert.ToDouble(splitline[i]);
                        }
                        toAdd.eventAges.Add(age);
                    }
                    patientBasedDataWithEvents.Add(toAdd);
                }
            }
        }

        private void readIDCriteria(string filePath)
        {
            using (var sr = new StreamReader(filePath, Encoding.Default))
            {
                IDCriteria = new List<IDData>();
                string[] titles = sr.ReadLine().Split('\t');
                int index_ID = Array.IndexOf(titles, "ID");
                int index_Birthday = Array.IndexOf(titles, "Birthday");
                while (!sr.EndOfStream)
                {
                    string[] splitline = sr.ReadLine().Split('\t');
                    IDData toAdd = new IDData()
                    {
                        ID = splitline[index_ID],
                        Birthday = splitline[index_Birthday],
                    };
                    IDCriteria.Add(toAdd);
                }
            }
        }


        private void initializeEventIndex(string[] titles)
        {
            EventIndex = new List<int>();
            foreach (var eventName in EventNames)
            {
                EventIndex.Add(Array.IndexOf(titles, eventName + "-最早年紀"));
            }
        }

        private void readSID(string filePath)
        {
            using (var sr = new StreamReader(filePath, Encoding.Default))
            {
                string[] titles = sr.ReadLine().Split('\t');
                int index_ID = Array.IndexOf(titles, "ID");
                int index_Birthday = Array.IndexOf(titles, "Birthday");
                int index_Gender = Array.IndexOf(titles, "Gender");
                int index_firstInDate = Array.IndexOf(titles, "firstInDate");
                int index_lastOutDate = Array.IndexOf(titles, "lastOutDate");
                while (!sr.EndOfStream)
                {
                    string[] splitline = sr.ReadLine().Split('\t');
                    string ID = splitline[index_ID];
                    string Birthday = splitline[index_Birthday];
                    var IDtoCompare = new IDData() { ID = ID, Birthday = Birthday };
                    if (IDCriteraiEnable && IDCriteria.BinarySearch(IDtoCompare) < 0)  //ID criteria
                    {
                        continue;
                    }
                    Gender gender = splitline[index_Gender] == "F" ? Gender.Female : Gender.Male;
                    string firstInDate = splitline[index_firstInDate];
                    string lastOutDate = splitline[index_lastOutDate];
                    double firstInAge = firstInDate.StringToDate().Subtract(Birthday.StringToDate()).TotalDays / 365.25;
                    if (firstInAge < 0)
                    {
                        firstInAge = 0;
                    }
                    if (firstInAge > 99)
                    {
                        firstInAge = 99;
                    }
                    double lastOutAge = lastOutDate.StringToDate().Subtract(Birthday.StringToDate()).TotalDays / 365.25;
                    if (lastOutAge < 0 || lastOutDate.StringToDate() > dataEndDate)
                    {
                        lastOutAge = dataEndDate.Subtract(Birthday.StringToDate()).TotalDays / 365.25;
                    }
                    var toCompare = new PatientBasedDataWithEvent() { ID = ID, Birthday = Birthday };
                    int index = patientBasedDataWithEvents.BinarySearch(toCompare);
                    if (index < 0)
                    {
                        for (int i = 0; i < ASITables.Count(); i++)
                        {
                            ASITables[i].addPatientYear(gender, firstInAge, lastOutAge);
                        }
                    }
                    else if (index >= 0)
                    {
                        lastOutAge = dataEndDate.Subtract(Birthday.StringToDate()).TotalDays / 365.25;
                        for (int i = 0; i < ASITables.Count(); i++)
                        {
                            double currentEventAge = patientBasedDataWithEvents[index].eventAges[i];
                            if (currentEventAge >= 0)
                            {
                                ASITables[i].addPatientYear(gender, firstInAge, currentEventAge);
                                ASITables[i].addEvent(gender, currentEventAge);
                            }
                            else
                            {
                                ASITables[i].addPatientYear(gender, firstInAge, lastOutAge);
                            }
                        }
                    }

                }
            }
        }

        private void writeASItables()
        {
            foreach (var t in ASITables)
            {
                using (var sw = new StreamWriter(outputDir + @"\" + t.tableName + ".ASI"))
                {
                    sw.Write(t.getResult());
                }
            }
        }
    }
}
