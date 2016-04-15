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
            data = new List<ActionDataCombinedWithOrderData>();
            foreach (var currentMatch in matchResult)
            {
                readAction(currentMatch.actionFile.path);
            }
        }
        
        void readAction(string actionFilePath)
        {
            using (var sr = new StreamReader(actionFilePath))
            {

            }
        }

        List<string> title;
        List<ActionDataCombinedWithOrderData> data;
        void readTitle() { }
        void appendOrderGroupToTitle() { }
        void combineOrderToAction() { }
        void write() { }

        class ActionDataCombinedWithOrderData :ActionData
        {
            public static int orderGroupCount;
            readonly string dataline;

            bool[] hasThisOrder = new bool[orderGroupCount];
            
        }
    }
}