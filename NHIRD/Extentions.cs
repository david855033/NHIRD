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
        public static string Round(this double input, int deci = 0)
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
        public static DateTime StringToDate(this string input)
        {
            try
            {
                input = input.TrimEnd();
                if (input.Length == 8)
                {
                    return Convert.ToDateTime(input.Substring(0, 4) + "-" + input.Substring(4, 2) + "-" + input.Substring(6, 2));
                }
                else if(input.Length == 6)
                {
                    return Convert.ToDateTime(input.Substring(0, 4) + "-" + input.Substring(4, 2) + "-01");
                }
                return DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

    }


}
