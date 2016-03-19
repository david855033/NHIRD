using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class StandarizedIDData : IDData
    {
        bool _isMale;
        public bool isMale
        {
            get { return _isMale; }
            set { _isMale = value; }
        }

        DateTime _firstInDate;
        public DateTime firstInDate
        {
            get { return _firstInDate; }
            set
            {
                if (value < _firstInDate || _firstInDate == default(DateTime))
                {
                    _firstInDate = value;
                }
            }
        }
        DateTime _firstOutDate;
        public DateTime firstOutDate
        {
            get { return _firstOutDate; }
            set
            {
                if (value > _firstOutDate || _firstOutDate == default(DateTime))
                {
                    _firstOutDate = value;
                }
            }
        }
        DateTime _lastInDate;
        public DateTime lastInDate
        {
            get { return _lastInDate; }
            set
            {
                if (value < _lastInDate || _lastInDate == default(DateTime))
                {
                    _lastInDate = value;
                }
            }
        }
        DateTime _lastOutDate;
        public DateTime lastOutDate
        {
            get { return _lastOutDate; }
            set
            {
                if (value > _lastOutDate || _lastOutDate == default(DateTime))
                {
                    _lastOutDate = value;
                }
            }
        }

        public static string ToTitle()
        {
            return "ID\tBirthday\tGender\tfirstInDate\tfirstOutDate\tlastInDate\tlastOutDate";
        }
        public string ToWriteLine()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ID);
            sb.Append('\t');
            sb.Append(Birthday);
            sb.Append('\t');
            sb.Append(isMale ? "M" : "F");
            sb.Append('\t');
            sb.Append(firstInDate.DateToString());
            sb.Append('\t');
            sb.Append(firstOutDate.DateToString());
            sb.Append('\t');
            sb.Append(lastInDate.DateToString());
            sb.Append('\t');
            sb.Append(lastOutDate.DateToString());
            return sb.ToString();
        }
    }
}
