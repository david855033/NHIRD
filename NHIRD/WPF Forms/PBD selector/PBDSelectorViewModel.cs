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
                GlobalSetting.set("PBDSelector_selectedField", value);
                OnPropertyChanged(nameof(selectField));
            }
        }

        string _excludeField;
        public string excludeField
        {
            get
            {
                return _excludeField;
            }
            set
            {
                _excludeField = value;
                GlobalSetting.set("PBDSelector_excludeField", value);
                OnPropertyChanged(nameof(excludeField));
            }
        }

        public void Do()
        {
            PBDSelector instance = new PBDSelector();
            instance.setPBDFiles(parentWindow.patientBasedDataFolderSelector.FileList);
            instance.setOutputFolder(_outputDir);
            instance.setSelectedField(_selectField);
            instance.setExcludeField(_excludeField);
            instance.Do();
        }
    }
}
