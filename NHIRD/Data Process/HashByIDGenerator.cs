using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    static class HashByIDGenerator
    {
        /// <summary>
        /// 使用ID string前2位數 轉換成uint (0-255)
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static uint GenerateHash(string ID)
        {
            return uint.Parse(ID.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        }
    }
}
