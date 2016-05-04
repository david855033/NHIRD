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
        public EventDetail[] diagnosisDetail;
        public EventDetail[] orderDetail;

        public void setDiagnosisDetail(int GroupCount)
        {
            diagnosisDetail = new EventDetail[GroupCount];
        }
        public void setOrderDetail(int GroupCount)
        {
            orderDetail = new EventDetail[GroupCount];
        }

        public string toWriteLine()
        {
            return ID + "\t" + Birthday;
        }
    }

    class EventDetail
    {
        public DateTime firstDate;
        public double firstAge;
        public int CDCount, DDCount;
        public string toString()
        {
            return "TODO";
        }
    }
}
