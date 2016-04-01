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
    public class ID_ViewModel : INotifyPropertyChanged
    {
        public readonly ID_Model Model_Instance;
        public static List<string> ID_FILE_TYPE = new List<string>() { "ID" };
        public Window_ID parentWindow;
        public ID_ViewModel(Window_ID parent)
        {
            parentWindow = parent;
            Model_Instance = new ID_Model(this);
            Do_StandarizeID = new RelayCommand(StandarizeID, (x) => true);
        }
        #region File Input Control
        string _inputDir;
        public string InputDir
        {
            get
            {
                return _inputDir;
            }
            set
            {
                _inputDir = value;
                GlobalSetting.set("ID_InputDir", value);
                OnPropertyChanged(nameof(InputDir));
                parentWindow.fileListControl.Renew(InputDir, ID_FILE_TYPE);
            }
        }
        public ObservableCollection<File> inputFileList
        {
            get { return Model_Instance.inputFileList; }
            set
            {
                Model_Instance.inputFileList = value;
                OnPropertyChanged(nameof(inputFileList));
            }
        }
        #endregion

        public string upperLimit_
        {
            get {
                return Model_Instance.upperLimit.ToString();
            }
            set {
                Int32.TryParse(value, out Model_Instance.upperLimit);
                OnPropertyChanged(nameof(upperLimit_)); }
        }
        public bool isUpperLimitEnabled_
        {
            get { return Model_Instance.isUpperLimitEnabled; }
            set {
                Model_Instance.isUpperLimitEnabled = value;
                OnPropertyChanged(nameof(isUpperLimitEnabled_));
            }
        }

        public string lowerLimit_
        {
            get { return Model_Instance.lowerLimit.ToString(); }
            set { Int32.TryParse(value, out Model_Instance.lowerLimit); OnPropertyChanged(nameof(lowerLimit_)); }
        }
        public bool isLowerLimitEnabled_
        {
            get { return Model_Instance.isLowerLimitEnabled; }
            set {
                Model_Instance.isLowerLimitEnabled = value;
                OnPropertyChanged(nameof(isLowerLimitEnabled_)); }
        }

        /// <summary>
        /// 輸出資料夾
        /// </summary>
        public string str_outputDir
        {
            get
            {
                return Model_Instance.str_outputDir;
            }
            set
            {
                Model_Instance.str_outputDir = value;
                GlobalSetting.set("ID_OutputDir", value);
                OnPropertyChanged("");
            }
        }

        // -- Actions
        public ICommand Do_StandarizeID { get; }
        //在此從控制項傳入inputFileList
        public void StandarizeID(object obj)
        {
            Model_Instance.inputFileList = parentWindow.fileListControl.inputFileList;
            Model_Instance.DoStandarizeID();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
