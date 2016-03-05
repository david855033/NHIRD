using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace NHIRD
{
    public class GetOrder_ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 實作Model
        /// </summary>
        public readonly GetOrder_Model Model_Instance;
        /// <summary>
        /// 實作此VM之上級視窗
        /// </summary>
        public Window_GetOrder parentWindow;
        // -- Consturctor, 需實作所有ICommand
        public GetOrder_ViewModel(Window_GetOrder parent)
        {
            parentWindow = parent;
            Model_Instance = new GetOrder_Model(this);
            Do_ExtractData = new RelayCommand(ExtractData, (x) => true);
            
        }

        // -- Properties --

        #region file input controls
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
                GlobalSetting.set("Order_InputDir", value);
                OnPropertyChanged(nameof(InputDir));
                renewSelectedFileTypes();
                parentWindow.fileListControl.Renew(InputDir, selectedFileTypes);
            }
        }

        bool _IsOOFileTypeEnabled = true;
        public bool IsOOFileTypeEnabled
        {
            get { return _IsOOFileTypeEnabled; }
            set { _IsOOFileTypeEnabled = value; renewSelectedFileTypes(); }
        }
        bool _IsDOFileTypeEnabled = true;
        public bool IsDOFileTypeEnabled
        {
            get { return _IsDOFileTypeEnabled; }
            set { _IsDOFileTypeEnabled = value; renewSelectedFileTypes(); }
        }
        bool _IsGOFileTypeEnabled = true;
        public bool IsGOFileTypeEnabled
        {
            get { return _IsGOFileTypeEnabled; }
            set { _IsGOFileTypeEnabled = value; renewSelectedFileTypes(); }
        }


        void renewSelectedFileTypes()
        {
            selectedFileTypes.Clear();
            if (_IsDOFileTypeEnabled) selectedFileTypes.Add("DO");
            if (_IsOOFileTypeEnabled) selectedFileTypes.Add("OO");
            if (_IsGOFileTypeEnabled) selectedFileTypes.Add("GO");
            parentWindow.fileListControl.Renew(InputDir, selectedFileTypes);
        }
        List<string> selectedFileTypes = new List<string>();

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

        #region -- Order Criteria Controls
        /// <summary>
        /// ICD include清單
        /// </summary>
        public ObservableCollection<string> OrderIncludes
        {
            get
            {
                return Model_Instance.list_Orderinclude;
            }
            set
            {
                Model_Instance.list_Orderinclude = value;
                OnPropertyChanged(nameof(OrderIncludes));
            }
        }
        public bool IsOrderIncludeEnabled
        {
            get { return Model_Instance.IsOrderCriteriaEnable; }
            set
            {
                Model_Instance.IsOrderCriteriaEnable = value;
                OnPropertyChanged(nameof(IsOrderIncludeEnabled));
            }
        }
        #endregion

        #region -- Action criteria controls
        /// <summary>
        /// Action Criteria List 
        /// </summary>
        public bool IsActionCriteriaEnable
        {
            get { return Model_Instance.IsActionCriteriaEnable; }
            set
            {
                Model_Instance.IsActionCriteriaEnable = value;
            }
        }
        public string ActionCriteriaFolderPath
        {
            get { return Model_Instance.ActionCriteriaFolderPath; }
            set
            {
                Model_Instance.ActionCriteriaFolderPath = value;
                GlobalSetting.set("Order_ActionCriteriaDir", value);
                OnPropertyChanged(nameof(ActionCriteriaFolderPath));
            }
        }
        public ObservableCollection<File> ActionCriteriaFileList
        {
            get { return Model_Instance.ActionCriteriaFileList; }
            set { Model_Instance.ActionCriteriaFileList = value; }
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
                GlobalSetting.set("Order_OutputDir", value);
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
