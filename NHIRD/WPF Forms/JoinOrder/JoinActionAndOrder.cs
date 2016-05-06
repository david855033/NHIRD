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

            //檢查目錄是否存
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
        }

        public void Do()
        {
            actionTitle = new List<string>();
            actionData = new DistinctList<ActionDataCombinedWithOrderData>();



            foreach (var currentMatch in matchResult)
            {
                if (currentMatch.MatchedOrderFiles.Count == 0) continue;
                readAction(currentMatch.actionFile.path);
                appendOrderGroupToActionTitle();
                foreach (var currentOrder in currentMatch.MatchedOrderFiles)
                {
                    combineOrderToAction(currentOrder.path);
                }
                var query = from q in actionData where q.hasThisOrder.Any(x => x) select q;

                write(currentMatch.actionFile);

                actionData.Clear();
                actionTitle.Clear();
            }
        }

        List<string> actionTitle;
        List<string> orderTitle;
        DistinctList<ActionDataCombinedWithOrderData> actionData;

        void readAction(string actionFilePath)
        {
            using (var sr = new StreamReader(actionFilePath))
            {
                actionTitle = new List<string>(sr.ReadLine().Split('\t'));
                var index = new Index();
                index.analyzeTitle(actionTitle);
                while (!sr.EndOfStream)
                {
                    var dataRow = new ActionDataCombinedWithOrderData(sr.ReadLine(), index);
                    actionData.AddDistinct(dataRow);
                }

            }
        }
        void readTitle() { }
        void appendOrderGroupToActionTitle()
        {
            foreach (var orderGroup in orderGroupList)
            {
                actionTitle.Add(orderGroup.name);
            }
        }
        void combineOrderToAction(string orderFilePath)
        {
            using (var sr = new StreamReader(orderFilePath))
            {
                orderTitle = new List<string>(sr.ReadLine().Split('\t'));
                var index = new Index();
                index.analyzeTitle(orderTitle);
                var orderIndex = orderTitle.FindIndex(x => x == "ORDER_CODE" || x == "DRUG_NO");
                while (!sr.EndOfStream)
                {
                    string dataline = sr.ReadLine();
                    string order = dataline.Split('\t')[orderIndex].Trim();
                    for (int i = 0; i < orderGroupList.Count; i++)
                    {
                        if (orderGroupList[i].hasThisOrder(order))
                        {
                            var comparer = ActionDataCombinedWithOrderData.getComparer(dataline, index);
                            int indexData = actionData.BinarySearch(comparer);
                            if (indexData >= 0)
                            {
                                actionData[indexData].hasThisOrder[i] = true;
                                continue;
                            }
                        }
                    }
                }
            }
        }
        void write(File actionFile)
        {
            string outputFilePath = outputDir + @"\" + actionFile.name.Split('.').First() + ".EXTO";
            using (var sw = new StreamWriter(outputFilePath, false, Encoding.Default))
            {
                StringBuilder titleToWrite = new StringBuilder();
                foreach (var s in actionTitle)
                {
                    titleToWrite.Append("[group]" + s + "\t");
                }
                sw.WriteLine(titleToWrite.ToString().TrimEnd('\t'));

                foreach (var d in actionData)
                {
                    StringBuilder currentRow = new StringBuilder(d.dataline);
                    foreach (var hasOrder in d.hasThisOrder)
                    {
                        currentRow.Append("\t" + (hasOrder ? "1" : "0"));
                    }
                    sw.WriteLine(currentRow);
                }
            }
        }

        class ActionDataCombinedWithOrderData : ActionData
        {
            public static int orderGroupCount;
            public readonly string dataline;
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
