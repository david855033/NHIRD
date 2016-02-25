using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class ActionData : IComparable
    {
        public string FEE_YM;
        public string HOSP_ID;
        public string APPL_TYPE;
        public string APPL_DATE;
        public string CASE_TYPE;
        public string SEQ_NO;

        string Comparer
        {
            get
            {
                return FEE_YM + HOSP_ID + APPL_TYPE
                  + APPL_DATE + CASE_TYPE + SEQ_NO;
            }
        }

        public int CompareTo(object obj)
        {
            var that = obj as ActionData;
            return (this.Comparer).CompareTo(that.Comparer);
        }
    }

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

        string Comparer
        {
            get
            {
                return ID + Birthday;
            }
        }

        public int CompareTo(object obj)
        {
            var that = obj as IDData;
            return (this.Comparer).CompareTo(that.Comparer);
        }
    }
    //accept object with Icomparable, use Binary search to do Sorted list
    public class DistinctList<T> : List<T>
    {
        /// <summary>
        /// insert new obj if not already exist, return binarysearch result at meanwhile
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int AddDistinct(T obj)
        {
            var Obj_Icomparable = obj as IComparable;
            int index = this.BinarySearch(obj);
            if (index < 0)
            {
                this.Insert(~index, obj);
            }
            return index;
        }
        /// <summary>
        /// use binary search
        /// </summary>
        /// <param name="obj"><use binarysearch to return index of obj/param>
        /// <returns></returns>
        new public int IndexOf(T obj)
        {
            return this.BinarySearch(obj);
        }
    }
}
