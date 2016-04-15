using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    static class ActionAndOrderMatcher
    {
        public static List<MatchOfActionAndOrder> doMatch(IEnumerable<File> actionList, IEnumerable<File> orderList)
        {
            var resultMatchList = new List<MatchOfActionAndOrder>();

            foreach (var action in actionList)
            {
                var query = from q in orderList
                            where q.FileType == (getCorrespondingOrderFileType(action.FileType)) &&
                            q.year == action.year &&
                            q.@group == action.@group &&
                            q.hashGroup== action.hashGroup 
                            select q;
                var matchToAdd = new MatchOfActionAndOrder()
                {
                    actionFile = action,
                    MatchedOrderFiles = query.ToList()
                };
                resultMatchList.Add(matchToAdd);
            }
            return resultMatchList;
        }

        static string getCorrespondingOrderFileType(string actionFileType)
        {
            string result = "";
            if (actionFileType == "CD")
            {
                result = "OO";
            }else if(actionFileType =="DD")
            {
                result = "DO";
            }
            else if (actionFileType == "GD")
            {
                result = "GO";
            }
            return result;
        }
    }
}
