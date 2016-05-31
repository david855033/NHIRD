using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class AgeSpecificIncidenceCalculator
    {
        IEnumerable<MatchOfPatiendBasedDataAndStandarizedID> matchList;
        public AgeSpecificIncidenceCalculator(IEnumerable<MatchOfPatiendBasedDataAndStandarizedID> matchList)
        {
            this.matchList = matchList;
        }

        public void Do()
        {
            System.Windows.MessageBox.Show("calculation ASI");



            // read PBD -> 先從title分析group list後 初始化List<string> eventNames,
            // 再產生List<patient>(含ID/性別/Event[](field: hasThisEvent, eventAge))，再讀取每個病人資料
            // 初始化ASI table, 欄位: Age  男-人年 -- 女-人年 - 男EventCount -  女EventCount , 以此做數個陣列(每個eventname用一個table)
            // 逐筆讀取Standarize ID, 比對有無出現在PBD裡面，然後增加所有ASI table的人年 以及對應的ASI table的evnetcount數量
           
            // 對每個match重複以上動作
            // 結算男生/女生的ASI
        }

        
    }
}
