using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class DiagnosisGroup
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
            if (index >= 0 && index < _includeList.Count
                && editTo.Trim() != ""
                && !_includeList.Any(x => x == editTo))
                _includeList[index] = editTo;
        }

        public int getExcludeCount()
        {
            return _excludeList.Count();
        }
        public string[] getExcludeList()
        {
            return _excludeList.ToArray();
        }
        public void addExclude(string Exclude)
        {
            if (Exclude.Trim() == "") return;
            _excludeList.AddDistinct(Exclude);
        }
        public void deleteExclude(string Exclude)
        {
            _excludeList.Remove(Exclude);
        }
        public void clearExclude()
        {
            _excludeList.Clear();
        }
        public void editExclude(string target, string editTo)
        {
            int index = _excludeList.IndexOf(target);
            if (index >= 0 && index < _excludeList.Count
                && editTo.Trim() != ""
                && !_excludeList.Any(x => x == editTo))
                _excludeList[index] = editTo;
        }

        public bool isThisGroupMatched(IEnumerable<string> ICDs)
        {
            bool matched=false;
            foreach (var ICD in ICDs)
            {
                if (_includeList.Any(x => x == ICD.Substring(0, x.Length)))
                {
                    matched = true;
                }
                if (!_excludeList.Any(x => x == ICD.Substring(0, x.Length)))
                {
                    return false;
                }
            }
            return matched;
        }

    }
}