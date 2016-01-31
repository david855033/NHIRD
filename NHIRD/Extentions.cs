using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    static class Extentions
    {
        public static string PathToFileName(this string path)
        {
            var pathsplit = path.Split('\\');
            return pathsplit.Last();
        }


        public static string Round(this double input, int deci=0)
        {
            try
            {
                return Math.Round(Convert.ToDouble(input), deci).ToString();
            }
            catch
            {
                return "";
            }
        }

    }
    

}
