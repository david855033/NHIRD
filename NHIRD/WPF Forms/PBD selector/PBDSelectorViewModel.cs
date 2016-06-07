using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class PBDSelectorViewModel : BasedNotifyPropertyChanged
    {
        public PBDSelectorWindow parentWindow;
        public PBDSelectorViewModel(PBDSelectorWindow parent)
        {
            parentWindow = parent;
        }

        string _patientBasedDataFolderPath;
        public string patientBasedDataFolderPath
        {
            get
            {
                return _patientBasedDataFolderPath;
            }
            set
            {
                _patientBasedDataFolderPath = value;
                GlobalSetting.set("PBDSelector_PBD", value);
                OnPropertyChanged(nameof(patientBasedDataFolderPath));
            }
        }

        string _outputDir;
        public string outputDir
        {
            get
            {
                return _outputDir;
            }

            set
            {
                _outputDir = value;
                GlobalSetting.set("PBDSelector_outputDir", value);
                OnPropertyChanged(nameof(outputDir));
            }
        }

        string _selectField;
        public string selectField
        {
            get
            {
                return _selectField;
            }
            set
            {
                _selectField = value;
                OnPropertyChanged(nameof(selectField));
            }
        }

        public void Do()
        {
            System.Windows.MessageBox.Show("Do.");
        }
    }
}
