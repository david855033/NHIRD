using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class PatientBasedDataViewModal
    {

        public readonly PatientBasedDataModel Model_Instance;
        public PatientBasedDataWindow parentWindow;
        public PatientBasedDataViewModal(PatientBasedDataWindow parent)
        {
            parentWindow = parent;
            Model_Instance = new PatientBasedDataModel(this);
        }
    }
}
