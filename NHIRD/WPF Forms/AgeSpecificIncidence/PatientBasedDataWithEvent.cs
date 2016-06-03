using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class PatientBasedDataWithEvent : IDData
    {
        public Gender gender;
        public List<double> eventAges = new List<double>();
    }
}
