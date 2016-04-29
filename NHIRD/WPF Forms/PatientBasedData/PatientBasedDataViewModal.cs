using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NHIRD
{
    class PatientBasedDataViewModal: BasedNotifyPropertyChanged
    {
        public readonly PatientBasedDataModel Model_Instance;
        public PatientBasedDataWindow parentWindow;
        public PatientBasedDataViewModal(PatientBasedDataWindow parent)
        {
            parentWindow = parent;
            Model_Instance = new PatientBasedDataModel(this);
            generatePatientBasedDataCommand = new RelayCommand(generatePatientBasedData, (x) => true);
        }

        // -- Properties --
        public string inputDir
        {
            get
            {
                return Model_Instance.inputDir;
            }
            set
            {
                Model_Instance.inputDir = value;
                GlobalSetting.set("PatientBasedData_InputDir", value);
                OnPropertyChanged(nameof(inputDir));
            }
        }
        public ObservableCollection<File> inputFiles
        {
            get
            {
                return Model_Instance.inputFiles;
            }
            set
            {
                Model_Instance.inputFiles = value;
            }
        }

        //--Action
        public ICommand generatePatientBasedDataCommand { get; }
        public void generatePatientBasedData(object obj)
        {
            Model_Instance.orderGroupList = parentWindow.orderGroupEditor.orderGroupList;
            Model_Instance.diagnosisGroupList = parentWindow.diagnosisGroupEditor.diagnosisGroupList;
            Model_Instance.generatePatientBasedData();
        }

        /// <summary>
        /// 輸出資料夾
        /// </summary>
        public string outputDir
        {
            get
            {
                return Model_Instance.outputDir;
            }
            set
            {
                Model_Instance.outputDir = value;
                GlobalSetting.set("PatientBasedData_OutputDir", value);
                OnPropertyChanged();
            }
        }
    }
}
