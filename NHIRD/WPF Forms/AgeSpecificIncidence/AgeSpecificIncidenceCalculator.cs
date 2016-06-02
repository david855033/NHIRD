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
        AgeSpecificIncidenceTable[] ASITables;
        public AgeSpecificIncidenceCalculator(IEnumerable<MatchOfPatiendBasedDataAndStandarizedID> matchList)
        {
            this.matchList = matchList;
        }

        public void Do()
        {
            System.Windows.MessageBox.Show("calculation ASI");

            analyzeTitleOfPatientBasedDataAndGetEventNames(matchList.First().patientBasedData.path);
            initializeAgeSpecificIncidenceTable();
            foreach (var currentMatch in matchList)
            {
                CalculateOneMatch(currentMatch);
            } 

            // read PBD -> 先從title分析group list後 初始化List<string> eventNames, OK
            // 初始化ASI table, 欄位: Age  男-人年 -- 女-人年 - 男EventCount -  女EventCount , 以此做數個陣列(每個eventname用一個table)
            
            // 再產生List<patient>(含ID/性別/Event[](field: hasThisEvent, eventAge))，再讀取每個病人資料
            
            // 逐筆讀取Standarize ID, 比對有無出現在PBD裡面，然後增加所有ASI table的人年 以及對應的ASI table的evnetcount數量           
            // 對每個match重複以上動作
            // 輸出ASI
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
                    EventNames.Add(s.Replace("-最早年紀",""));
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
            readPBD(currentMatch.patientBasedData.path);
            readSID(currentMatch.standarizedID.path);
        }
        private void readPBD(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                string title = sr.ReadLine();
                while (!sr.EndOfStream())
                {

                }
            }
        }
        private void readSID(string filePath)
        {

        }

    }
}
