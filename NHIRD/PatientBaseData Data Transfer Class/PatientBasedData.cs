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
       
        public string toWriteLine()
        {
            return ID +"\t"+Birthday;
        }
    }
}
