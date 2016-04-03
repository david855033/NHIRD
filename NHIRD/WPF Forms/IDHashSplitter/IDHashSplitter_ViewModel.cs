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
    
    public class IDHashSplitter_ViewModel : INotifyPropertyChanged
    {
        public readonly IDHashSplitter_Model Model_Instance;
        public static List<string> ID_FILE_TYPE = new List<string>() { "ID" };
        public readonly Window_IDHashSplitter parentWindow;
        public IDHashSplitter_ViewModel(Window_IDHashSplitter parentWindow)
        {
            this.parentWindow = parentWindow;
            Model_Instance = new IDHashSplitter_Model(this);
            Do_IDHashSplit = new RelayCommand(IDHashSplit, (x) => true);
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
                GlobalSetting.set("IDSplit_InputDir", value);
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
                GlobalSetting.set("IDSplit_OutputDir", value);
                OnPropertyChanged("");
            }
        }


        public ICommand Do_IDHashSplit { get; }
        public void IDHashSplit(object obj)
        {
            Model_Instance.inputFileList = parentWindow.fileListControl.inputFileList;
            Model_Instance.Do_IDHashSplit();
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
