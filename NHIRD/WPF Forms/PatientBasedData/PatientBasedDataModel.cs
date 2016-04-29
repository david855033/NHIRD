using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{

    class PatientBasedDataModel
    {
        PatientBasedDataViewModal parentVM;
        public PatientBasedDataModel(PatientBasedDataViewModal parentVM)
        {
            this.parentVM = parentVM;
        }


        public string inputDir { get; set; }
        public ObservableCollection<File> inputFiles = new ObservableCollection<File>();
        public string outputDir { get; set; }
        
        public List<OrderGroup> orderGroupList = new List<OrderGroup>();
        public List<DiagnosisGroup> diagnosisGroupList = new List<DiagnosisGroup>();

        public void generatePatientBasedData()
        {
            new PatientBasedDataGenerator(inputFiles, orderGroupList, diagnosisGroupList, outputDir).Do();
        }
    }
}
