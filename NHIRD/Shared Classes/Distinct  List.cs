using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{

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
