using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class JoinActionAndOrder
    {
        List<MatchOfActionAndOrder> matchResult;
        public string outputDir;
        private List<OrderGroup> orderGroupList;

        public JoinActionAndOrder(List<OrderGroup> orderGroupList, List<MatchOfActionAndOrder> MatchResult, string outputDir)
        {
            this.matchResult = MatchResult;
            this.outputDir = outputDir;
            this.orderGroupList = orderGroupList;
            ActionDataCombinedWithOrderData.orderGroupCount = orderGroupList.Count;
        }

        public void Do()
        {
            title = new List<string>();
            data = new DistinctList<ActionDataCombinedWithOrderData>();
            foreach (var currentMatch in matchResult)
            {
                System.Windows.MessageBox.Show($"read Action{currentMatch.actionFile.path}");
                readAction(currentMatch.actionFile.path);
                appendOrderGroupToTitle();
                foreach (var currentOrder in currentMatch.MatchedOrderFiles)
                {
                    combineOrderToAction(currentOrder.path);
                }
                var query = from q in data where q.hasThisOrder.Any(x => x) select q;
                System.Windows.MessageBox.Show($"In this action {query.Count()} has positive order");
                write();
                data.Clear();
                title.Clear();
            }
        }

        List<string> title;
        DistinctList<ActionDataCombinedWithOrderData> data;

        void readAction(string actionFilePath)
        {
            using (var sr = new StreamReader(actionFilePath))
            {
                title = new List<string>(sr.ReadLine().Split('\t'));
                var index = new Index();
                index.analyzeTitle(title);
                while (!sr.EndOfStream)
                {
                    var dataRow = new ActionDataCombinedWithOrderData(sr.ReadLine(), index);
                    data.AddDistinct(dataRow);
                }

            }
        }
        void readTitle() { }
        void appendOrderGroupToTitle()
        {
            foreach (var orderGroup in orderGroupList)
            {
                title.Add(orderGroup.name);
            }
        }
        void combineOrderToAction(string orderFilePath)
        {
            using (var sr = new StreamReader(orderFilePath))
            {
                title = new List<string>(sr.ReadLine().Split('\t'));
                var index = new Index();
                index.analyzeTitle(title);
                var orderIndex = title.FindIndex(x => x == "ORDER_CODE" || x == "DRUG_NO");
                while (!sr.EndOfStream)
                {
                    string dataline = sr.ReadLine();
                    string order = dataline.Split('\t')[orderIndex].Trim();
                    for (int i = 0; i < orderGroupList.Count; i++)
                    {
                        if (orderGroupList[i].hasThisOrder(order))
                        {
                            var comparer = ActionDataCombinedWithOrderData.getComparer(dataline, index);
                            int indexData = data.BinarySearch(comparer);
                            if (indexData >= 0)
                            {
                                data[indexData].hasThisOrder[i] = true;
                                continue;
                            }
                        }
                    }
                }
            }
        }
        void write() { }

        class ActionDataCombinedWithOrderData : ActionData
        {
            public static int orderGroupCount;
            readonly string dataline;
            public bool[] hasThisOrder = new bool[orderGroupCount];
            public ActionDataCombinedWithOrderData(string dataline, Index index)
            {
                this.dataline = dataline;
                string[] splitline = dataline.Split('\t');
                this.FEE_YM = splitline[index.FEE_YM];
                this.HOSP_ID = splitline[index.HOSP_ID];
                this.APPL_DATE = splitline[index.APPL_DATE];
                this.SEQ_NO = splitline[index.SEQ_NO];
                this.CASE_TYPE = splitline[index.CASE_TYPE];
                this.APPL_TYPE = splitline[index.APPL_TYPE];
            }
            ActionDataCombinedWithOrderData() { }
            public static ActionDataCombinedWithOrderData getComparer(string dataline, Index index)
            {
                string[] splitline = dataline.Split('\t');
                return new ActionDataCombinedWithOrderData()
                {
                    FEE_YM = splitline[index.FEE_YM],
                    HOSP_ID = splitline[index.HOSP_ID],
                    APPL_DATE = splitline[index.APPL_DATE],
                    SEQ_NO = splitline[index.SEQ_NO],
                    CASE_TYPE = splitline[index.CASE_TYPE],
                    APPL_TYPE = splitline[index.APPL_TYPE]
                };
            }
        }
    }

    class Index
    {
        public int FEE_YM;
        public int HOSP_ID;
        public int APPL_DATE;
        public int SEQ_NO;
        public int CASE_TYPE;
        public int APPL_TYPE;
        public void analyzeTitle(List<string> title)
        {
            FEE_YM = title.FindIndex(x => x == "FEE_YM");
            HOSP_ID = title.FindIndex(x => x == "HOSP_ID");
            APPL_DATE = title.FindIndex(x => x == "APPL_DATE");
            SEQ_NO = title.FindIndex(x => x == "SEQ_NO");
            CASE_TYPE = title.FindIndex(x => x == "CASE_TYPE");
            APPL_TYPE = title.FindIndex(x => x == "APPL_TYPE");
        }
    }
}
