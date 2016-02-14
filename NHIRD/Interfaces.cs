using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class IDData : IComparable
    {
        public string ID;
        private DateTime _Birthday;
        /// <summary>
        /// return "yyyy-MM"
        /// </summary>
        public string Birthday
        {
            get
            {
                return _Birthday.ToString("yyyy-MM");
            }
            set
            {
                _Birthday = value.StringToDate();
            }
        }


        public int CompareTo(object obj)
        {
            var that = obj as IDData;
            return (this.ID + this.Birthday).CompareTo(that.ID + that.Birthday);
        }
    }

    public class DistinctList<T> : List<T>
    {
        public void AddDistinct(T obj)
        {
            var Obj_Icomparable = obj as IComparable;
            int index = this.BinarySearch(obj);
            if (index < 0)
            {
                this.Insert(~index, obj);
            }
        }
    }
}
