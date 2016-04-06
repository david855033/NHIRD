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
    public class GetGDViewModel : INotifyPropertyChanged
    {
        public static List<string> GD_FILE_TYPE = new List<string>() { "GD" };

        /// <summary>
        /// 實作Model
        /// </summary>
        public readonly GetGDModel Model_Instance;
        /// <summary>
        /// 實作此VM之上級視窗
        /// </summary>
        public GetGDWindow parentWindow;
        // -- Consturctor, 需實作所有ICommand
        public GetGDViewModel(GetGDWindow parent)
        {
            parentWindow = parent;
            Model_Instance = new GetGDModel(this);
            Do_ExtractData = new RelayCommand(ExtractData, (x) => true);
        }

        #region -- file input controls
        // -- 資料夾的路徑，更動時觸發fileListControl的renew功能
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
                GlobalSetting.set("GD_InputDir", value);
                OnPropertyChanged(nameof(InputDir));
                parentWindow.fileListControl.Renew(InputDir, GD_FILE_TYPE);
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

        #region -- ID criteria controls
        /// <summary>
        /// ID Criteria List 
        /// </summary>
        public bool IsIDCriteriaEnable
        {
            get { return Model_Instance.IsIDCriteriaEnable; }
            set
            {
                Model_Instance.IsIDCriteriaEnable = value;
            }
        }
        public string IDCriteriaFolderPath
        {
            get { return Model_Instance.IDCriteriaFolderPath; }
            set
            {
                Model_Instance.IDCriteriaFolderPath = value;
                GlobalSetting.set("GD_IDCriteriaDir", value);
                OnPropertyChanged(nameof(IDCriteriaFolderPath));
            }
        }
        public ObservableCollection<File> IDCriteriaFileList
        {
            get { return Model_Instance.IDCriteriaFileList; }
            set { Model_Instance.IDCriteriaFileList = value; }
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
                GlobalSetting.set("GD_OutputDir", value);
                OnPropertyChanged("");
            }
        }

        // -- Actions
        /// <summary>
        /// 開始運算 for data bingding(於建構子中初始化)
        /// </summary>
        public ICommand Do_ExtractData { get; }
        /// <summary>
        /// 呼叫Model開始運算(在此正式將所有FileList傳入model)
        /// </summary>
        /// <param name="obj"></param>
        public void ExtractData(object obj)
        {
            Model_Instance.inputFileList = parentWindow.fileListControl.inputFileList;
            Model_Instance.DoExtractData();
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
