using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class ActionData : IComparable
    {
        private string _FEE_YM;
        private int int_FEE_YM;
        public string FEE_YM
        {
            get { return _FEE_YM; }
            set
            {
                _FEE_YM = value;
                int.TryParse(value, out int_FEE_YM);
            }
        }

        private string _HOSP_ID;
        public string HOSP_ID
        {
            get { return _HOSP_ID; }
            set
            {
                _HOSP_ID = value;
                for (int i = 0; i < _HOSP_ID_HexToInt.Length; i++)
                    _HOSP_ID_HexToInt[i] = HextoUInt(value.Substring(i * 8, 8));
            }
        }
        private uint[] _HOSP_ID_HexToInt = new uint[4];
        public uint[] HOSP_ID_HexToInt
        {
            get { return _HOSP_ID_HexToInt.ToArray(); }
        }
        static uint HextoUInt(string input)
        {
            return uint.Parse(input, System.Globalization.NumberStyles.HexNumber);
        }

        private string _APPL_DATE;
        private int int_APPL_DATE;
        public string APPL_DATE
        {
            get { return _APPL_DATE; }
            set
            {
                _APPL_DATE = value;
                int.TryParse(value, out int_APPL_DATE);
            }
        }

        private string _SEQ_NO;
        private int int_SEQ_NO;
        public string SEQ_NO
        {
            get { return _SEQ_NO; }
            set
            {
                _SEQ_NO = value;
                int.TryParse(value, out int_SEQ_NO);
            }
        }


        public string CASE_TYPE;
        public string APPL_TYPE;


        public int CompareTo(object obj)
        {
            var that = obj as ActionData;

            if (this._HOSP_ID_HexToInt[0] != that._HOSP_ID_HexToInt[0])
                return (this._HOSP_ID_HexToInt[0]).CompareTo(that._HOSP_ID_HexToInt[0]);

            if (this._HOSP_ID_HexToInt[1] != that._HOSP_ID_HexToInt[1])
                return (this._HOSP_ID_HexToInt[1]).CompareTo(that._HOSP_ID_HexToInt[1]);

            if (this._HOSP_ID_HexToInt[2] != that._HOSP_ID_HexToInt[2])
                return (this._HOSP_ID_HexToInt[2]).CompareTo(that._HOSP_ID_HexToInt[2]);

            if (this._HOSP_ID_HexToInt[3] != that._HOSP_ID_HexToInt[3])
                return (this._HOSP_ID_HexToInt[3]).CompareTo(that._HOSP_ID_HexToInt[3]);

            if (this.int_FEE_YM != that.int_FEE_YM)
                return (this.int_FEE_YM).CompareTo(that.int_FEE_YM);

            if (this.APPL_DATE != that.APPL_DATE)
                return (this.APPL_DATE).CompareTo(that.APPL_DATE);

            if (this.SEQ_NO != that.SEQ_NO)
                return (this.SEQ_NO).CompareTo(that.SEQ_NO);

            if (this.CASE_TYPE != that.CASE_TYPE)
                return (this.CASE_TYPE).CompareTo(that.CASE_TYPE);

            return (this.APPL_TYPE).CompareTo(that.APPL_TYPE);

        }
    }


}
