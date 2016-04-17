using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class OrderGroup
    {
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        private DistinctList<string> _orderList = new DistinctList<string>();
        public int getOrderCount()
        {
            return _orderList.Count();
        }
        public string[] getOrderList()
        {
            return _orderList.ToArray();
        }
        public void addOrder(string order)
        {
            if (order.Trim() == "") return;
            _orderList.AddDistinct(order.Trim());
        }
        public void deleteOrder(string order)
        {
            _orderList.Remove(order);
        }
        public void clearOrder()
        {
            _orderList.Clear();
        }
        public void editOrder(string target, string editTo)
        {
            int index = _orderList.IndexOf(target);
            if (index >= 0 && index < _orderList.Count && !_orderList.Any(x => x == editTo))
                _orderList[index] = editTo.Trim();
        }
        public bool hasThisOrder(string order)
        {
            int result = _orderList.BinarySearch(order);
            return  result>= 0;
        }
    }
}
