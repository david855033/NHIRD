using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class DiagnosisGroup
    {
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        private DistinctList<string> _includeList = new DistinctList<string>();
        private DistinctList<string> _excludeList = new DistinctList<string>();

        public int getIncludeCount()
        {
            return _includeList.Count();
        }
        public string[] getIncludeList()
        {
            return _includeList.ToArray();
        }
        public void addInclude(string Include)
        {
            if (Include.Trim() == "") return;
            _includeList.AddDistinct(Include);
        }
        public void deleteInclude(string Include)
        {
            _includeList.Remove(Include);
        }
        public void clearInclude()
        {
            _includeList.Clear();
        }
        public void editInclude(string target, string editTo)
        {
            int index = _includeList.IndexOf(target);
            if (index >= 0 && index < _includeList.Count && !_includeList.Any(x => x == editTo))
                _includeList[index] = editTo;
        }

        public bool hasThisOrder(IEnumerable<string> ICDs)
        {
            int result = _includeList.BinarySearch(order);
            return result >= 0;
        }

    }
}