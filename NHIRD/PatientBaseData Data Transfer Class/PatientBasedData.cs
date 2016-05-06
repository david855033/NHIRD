using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class PatientBasedData : IDData
    {
        public int CDcount = 0, DDcount = 0, GDcount = 0;
        public EventDetail[] diagnosisDetails;
        public EventDetail[] orderDetails;

        public void setDiagnosisDetail(int GroupCount)
        {
            diagnosisDetails = new EventDetail[GroupCount];
            for (int i = 0; i < GroupCount; i++)
            {
                diagnosisDetails[i] = new EventDetail(this);
            }
        }
        public void setOrderDetail(int GroupCount)
        {
            orderDetails = new EventDetail[GroupCount];
            for (int i = 0; i < GroupCount; i++)
            {
                orderDetails[i] = new EventDetail(this);
            }
        }

        public static string toTitile()
        {
            StringBuilder result = new StringBuilder("ID" + "\t" + "Birthday" + "\t" + "門診檔資料數" + "\t" + "住院檔資料數");
            return result.ToString();
        }
        public string toWriteLine()
        {
            StringBuilder result = new StringBuilder(ID + "\t" + Birthday + "\t" + CDcount + "\t" + DDcount);

            foreach (var detail in diagnosisDetails)
            {
                result.Append(detail.toWriteLine());
            }
            foreach (var detail in orderDetails)
            {

                result.Append(detail.toWriteLine());
            }

            return result.ToString();
        }
    }

    class EventDetail
    {
        private DateTime _firstDate;
        public DateTime firstDateInDateTime
        {
            get { return _firstDate; }
            set
            {
                if (value <= _firstDate)
                    _firstDate = value;
            }
        }
        public string firstDate
        {
            get
            {
                if (_firstDate > DateTime.MinValue)
                {
                    return _firstDate.DateToString();
                }
                return "";
            }
            set
            {
                var dt = value.StringToDate();
                if (dt <= _firstDate || _firstDate == DateTime.MinValue)
                    _firstDate = dt;
            }
        }
        private PatientBasedData parentPatientBasedData;

        public EventDetail(PatientBasedData parent)
        {
            _firstDate = DateTime.MinValue;
            parentPatientBasedData = parent;
        }

        public string firstAge
        {
            get
            {
                if (_firstDate == DateTime.MinValue)
                {
                    return "";
                }
                return Math.Round(firstDateInDateTime.Subtract(parentPatientBasedData.BirthdayInDateTime).TotalDays / 365.25, 1).ToString();
            }
        }

        public int CDCount, DDCount;

        public static string toTitle(string prefix)
        {
            return "\t" + prefix + "-最早年紀"
                 + "\t" + prefix + "-門診次數"
                + "\t" + prefix + "-住院次數";
        }
        public string toWriteLine()
        {
            return "\t" + firstAge + "\t" + CDCount + "\t" + DDCount;
        }
    }
}
