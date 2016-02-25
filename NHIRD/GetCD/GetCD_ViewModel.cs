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
    public class GetCD_ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 實作Model
        /// </summary>
        public readonly GetCD_Model Model_Instance;
        /// <summary>
        /// 實作此VM之上級視窗
        /// </summary>
        public Window_GetCD parentWindow;
        // -- Consturctor, 需實作所有ICommand
        public GetCD_ViewModel(Window_GetCD parent)
        {
            parentWindow = parent;
            Model_Instance = new GetCD_Model(this);
            Do_ExtractData = new RelayCommand(ExtractData, (x) => true);
        }

        public bool IsCDFileTypeEnabled
        {
            get { return Model_Instance.IsCDFileTypeEnabled; }
            set
            {
                Model_Instance.IsCDFileTypeEnabled = value;
                makeFileList(InputDir);
                OnPropertyChanged(nameof(IsCDFileTypeEnabled));
            }
        }
        public bool IsDDFileTypeEnabled
        {
            get { return Model_Instance.IsDDFileTypeEnabled; }
            set
            {
                Model_Instance.IsDDFileTypeEnabled = value;
                makeFileList(InputDir);
                OnPropertyChanged(nameof(IsDDFileTypeEnabled));
            }
        }

        // -- Properties
        /// <summary>
        /// 資料夾的路徑，更動時自動更新fileList
        /// </summary>
        public string InputDir
        {
            get
            {
                return Model_Instance.str_inputDir;
            }
            set
            {
                Model_Instance.str_inputDir = value;
                GlobalSetting.set("CD_InputDir", value);
                // -- 初始化 file / year / group list
                makeFileList(value);
                OnPropertyChanged(nameof(InputDir));
            }
        }
        /// <summary>
        /// 更動input dir 或
        /// </summary>
        /// <param name="inputPath"></param>
        void makeFileList(string inputPath)
        {
            try
            {
                List<string> paths = new List<string>();
                foreach (var currentFileType in Model_Instance.selectedFileTypes)
                {
                    paths.AddRange(Directory.EnumerateFiles(inputPath, "*" + currentFileType + "*.DAT", SearchOption.AllDirectories));
                }
                paths.Sort();
                // -- file
                var newfiles = new ObservableCollection<File>();
                foreach (string str_filepath in paths)
                {
                    newfiles.Add(new File(str_filepath));
                }
                inputFileList.Clear();
                inputFileList = newfiles;
                // -- year
                var newyears = new ObservableCollection<Year>();
                foreach (string s in inputFileList.Select(x => x.year).Distinct())
                {
                    newyears.Add(new Year(s));
                }
                inputYearList.Clear();
                inputYearList = newyears;
                // -- group
                var newgroups = new ObservableCollection<Group>();
                foreach (string s in inputFileList.Select(x => x.group).Distinct())
                {
                    newgroups.Add(new Group(s));
                }
                inputGroupList.Clear();
                inputGroupList = newgroups;
                parentWindow.refresh_Listviews();
            }
            catch
            {
                System.Windows.MessageBox.Show("不正確的路徑\r\n提示：不可以使用磁碟機之根目錄");
            }
        }
        /// <summary>
        /// 檔案清單
        /// </summary>
        public ObservableCollection<File> inputFileList
        {
            get
            {
                return Model_Instance.inputFileList;
            }
            set
            {
                Model_Instance.inputFileList = value;
                OnPropertyChanged("");
            }
        }
        /// <summary>
        /// 年份清單(於載入檔案清單時建立)
        /// </summary>
        public ObservableCollection<Year> inputYearList
        {
            get
            {
                return Model_Instance.inputYearList;
            }
            set
            {
                Model_Instance.inputYearList = value;
                OnPropertyChanged("");
            }
        }
        /// <summary>
        /// 組別清單(於載入檔案清單時建立)
        /// </summary>
        public ObservableCollection<Group> inputGroupList
        {
            get
            {
                return Model_Instance.inputGroupList;
            }
            set
            {
                Model_Instance.inputGroupList = value;
                OnPropertyChanged("");
            }
        }
        /// <summary>
        /// 顯示目前選取的檔案數量
        /// </summary>
        public string FileStatus
        {
            get { return Model_Instance.str_filestatus; }
            set
            {
                Model_Instance.str_filestatus = value;
                OnPropertyChanged(nameof(FileStatus));
            }
        }


        #region -- ICD criteira Contorls
        /// <summary>
        /// ICD include清單
        /// </summary>
        public ObservableCollection<string> ICDIncludes
        {
            get
            {
                return Model_Instance.list_ICDinclude;
            }
            set
            {
                Model_Instance.list_ICDinclude = value;
                OnPropertyChanged(nameof(ICDIncludes));
            }
        }
        /// <summary>
        /// ICD Exclude清單
        /// </summary>
        public ObservableCollection<string> ICDExcludes
        {
            get
            {
                return Model_Instance.list_ICDExclude;
            }
            set
            {
                Model_Instance.list_ICDExclude = value;
                OnPropertyChanged(nameof(ICDExcludes));
            }
        }
        public bool IsICDIncludeEnabled
        {
            get { return Model_Instance.IsICDIncludeEnabled; }
            set
            {
                Model_Instance.IsICDIncludeEnabled = value;
                if (value == false) IsICDExcludeEnabled = false; //Include被取消時同時取消Exclude
                OnPropertyChanged(nameof(IsICDIncludeEnabled));
            }
        }
        public bool IsICDExcludeEnabled
        {
            get { return Model_Instance.IsICDExcludeEnabled; }
            set
            {
                Model_Instance.IsICDExcludeEnabled = value;
                if (value == true) IsICDIncludeEnabled = true; //exclude被啟動時同時啟動include
                OnPropertyChanged(nameof(IsICDExcludeEnabled));
            }
        }
        #endregion

        #region -- PROC criteira Contorls
        /// <summary>
        /// PROC include清單
        /// </summary>
        public ObservableCollection<string> PROCIncludes
        {
            get
            {
                return Model_Instance.list_PROCinclude;
            }
            set
            {
                Model_Instance.list_PROCinclude = value;
                OnPropertyChanged(nameof(PROCIncludes));
            }
        }
        /// <summary>
        /// PROC Exclude清單
        /// </summary>
        public ObservableCollection<string> PROCExcludes
        {
            get
            {
                return Model_Instance.list_PROCExclude;
            }
            set
            {
                Model_Instance.list_PROCExclude = value;
                OnPropertyChanged(nameof(PROCExcludes));
            }
        }
        public bool IsPROCIncludeEnabled
        {
            get { return Model_Instance.IsPROCIncludeEnabled; }
            set
            {
                Model_Instance.IsPROCIncludeEnabled = value;
                if (value == false) IsPROCExcludeEnabled = false; //Include被取消時同時取消Exclude
                OnPropertyChanged(nameof(IsPROCIncludeEnabled));
            }
        }
        public bool IsPROCExcludeEnabled
        {
            get { return Model_Instance.IsPROCExcludeEnabled; }
            set
            {
                Model_Instance.IsPROCExcludeEnabled = value;
                if (value == true) IsPROCIncludeEnabled = true; //exclude被啟動時同時啟動include
                OnPropertyChanged(nameof(IsPROCExcludeEnabled));
            }
        }
        #endregion

        #region -- Age Criteria Controls
        public bool IsAgeLCriteriaEnable
        {
            get { return Model_Instance.IsAgeLCriteriaEnable; }
            set { Model_Instance.IsAgeLCriteriaEnable = value; }
        }
        public bool IsAgeUCriteriaEnable
        {
            get { return Model_Instance.IsAgeUCriteriaEnable; }
            set { Model_Instance.IsAgeUCriteriaEnable = value; }
        }
        public string str_AgeL
        {
            get { return Model_Instance.db_AgeL.Round(1); }
            set
            {
                double result = -1;
                double.TryParse(value, out result);
                if (result > 100) result = 100;
                if (result < 0) result = 0;
                Model_Instance.db_AgeL = result;
                OnPropertyChanged(nameof(str_AgeL));
            }
        }
        public string str_AgeU
        {
            get { return Model_Instance.db_AgeU.Round(1); }
            set
            {
                double result = -1;
                double.TryParse(value, out result);
                if (result > 100) result = 100;
                if (result < 0) result = 0;
                Model_Instance.db_AgeU = result;
                OnPropertyChanged(nameof(str_AgeU));
            }
        }
        #endregion

        #region -- ID criteria controls
        /// <summary>
        /// ID Criteria List 的Folder Path(資料夾內應該要內含CD或DD的EXT檔案), 更動時更新ID Criteria List
        /// </summary>
        public bool IsIDCriteriaEnable
        {
            get { return Model_Instance.IsIDCriteriaEnable; }
            set {
                Model_Instance.IsIDCriteriaEnable = value;
            }
        } 
        public string IDCriteriaFolderPath
        {
            get { return Model_Instance.IDCriteriaFolderPath; }
            set
            {
                Model_Instance.IDCriteriaFolderPath =value;
                GlobalSetting.set("CD_IDCriteriaDir", value);
                OnPropertyChanged(nameof(IDCriteriaFolderPath));
            }
        }
        public List<File> IDCriteriaFileList
        {
            get { return Model_Instance.IDCriteriaFileList; }
            set { Model_Instance.IDCriteriaFileList = value; }
        }
        public string IDCriteriaMessage
        {
            get { return Model_Instance.IDCriteriaMessage; }
            set
            {
                Model_Instance.IDCriteriaMessage = value;
                OnPropertyChanged(nameof(IDCriteriaMessage));
            }
        }
        #endregion

        #region -- Order criteria controls
        /// <summary>
        /// ID Criteria List 的Folder Path(資料夾內應該要內含CD或DD的EXT檔案), 更動時更新ID Criteria List
        /// </summary>
        public bool IsOrderCriteriaEnable
        {
            get { return Model_Instance.IsOrderCriteriaEnable; }
            set
            {
                Model_Instance.IsOrderCriteriaEnable = value;
            }
        }
        public string OrderCriteriaFolderPath
        {
            get { return Model_Instance.OrderCriteriaFolderPath; }
            set
            {
                Model_Instance.OrderCriteriaFolderPath = value;
                GlobalSetting.set("CD_OrderCriteriaDir", value);
                OnPropertyChanged(nameof(OrderCriteriaFolderPath));
            }
        }
        public List<File> OrderCriteriaFileList
        {
            get { return Model_Instance.OrderCriteriaFileList; }
            set { Model_Instance.OrderCriteriaFileList = value; }
        }
        public string OrderCriteriaMessage
        {
            get { return Model_Instance.OrderCriteriaMessage; }
            set
            {
                Model_Instance.OrderCriteriaMessage = value;
                OnPropertyChanged(nameof(OrderCriteriaMessage));
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
                GlobalSetting.set("CD_OutputDir", value);
                OnPropertyChanged("");
            }
        }
        /// <summary>
        /// 顯示訊息(除錯用)
        /// </summary>
        public string message
        {
            get { return Model_Instance.message; }
            set { Model_Instance.message = value; OnPropertyChanged(nameof(message)); }
        }

        // -- Actions
        /// <summary>
        /// 開始運算 for data bingding(於建構子中初始化)
        /// </summary>
        public ICommand Do_ExtractData { get; }
        /// <summary>
        /// 呼叫Model開始運算
        /// </summary>
        /// <param name="obj"></param>
        public void ExtractData(object obj)
        {
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
