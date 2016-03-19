using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class IDData : IComparable
    {
        string _ID;
        public string ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                for (int i = 0; i < _IDHexToInt.Length; i++)
                    _IDHexToInt[i] = HextoUInt(value.Substring(i * 8, 8));
            }
        }
        public uint[] IDHexToInt { get { return _IDHexToInt.ToArray(); } }
        uint[] _IDHexToInt = new uint[4];

        static uint HextoUInt(string input)
        {
            return uint.Parse(input, System.Globalization.NumberStyles.HexNumber);
        }

        private DateTime _Birthday;
        public DateTime BirthdayInDateTime
        {
            get { return _Birthday; }
        }
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

            if(this._IDHexToInt[0]!= that._IDHexToInt[0])
                return (this._IDHexToInt[0]).CompareTo(that._IDHexToInt[0]);

            if (this._IDHexToInt[1] != that._IDHexToInt[1])
                return (this._IDHexToInt[1]).CompareTo(that._IDHexToInt[1]);

            if (this._IDHexToInt[2] != that._IDHexToInt[2])
                return (this._IDHexToInt[2]).CompareTo(that._IDHexToInt[2]);

            if (this._IDHexToInt[3] != that._IDHexToInt[3])
                return (this._IDHexToInt[3]).CompareTo(that._IDHexToInt[3]);

            return (this._Birthday).CompareTo(that._Birthday);
        }
    }
}
