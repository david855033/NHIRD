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
        public GetCD_Window parentWindow;
        // -- Consturctor, 需實作所有ICommand
        public GetCD_ViewModel(GetCD_Window parent)
        {
            parentWindow = parent;
            Model_Instance = new GetCD_Model(this);
            Do_ExtractData = new RelayCommand(ExtractData, (x) => true);
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
                Model_Instance.str_inputDir = GlobalSetting.inputDir = value;
                // -- 初始化 file / year / group list
                try
                {
                    string[] paths;
                    paths = Directory.EnumerateFiles(value, "*CD*.DAT", SearchOption.AllDirectories).ToArray();
                    Array.Sort(paths);
                    // -- file
                    var newfiles = new ObservableCollection<File>();
                    foreach (string str_filepath in paths)
                    {
                        newfiles.Add(new File(str_filepath));
                    }
                    files.Clear();
                    files = newfiles;
                    // -- year
                    var newyears = new ObservableCollection<Year>();
                    foreach (string s in files.Select(x => x.year).Distinct())
                    {
                        newyears.Add(new Year(s));
                    }
                    years.Clear();
                    years = newyears;
                    // -- group
                    var newgroups = new ObservableCollection<Group>();
                    foreach (string s in files.Select(x => x.group).Distinct())
                    {
                        newgroups.Add(new Group(s));
                    }
                    groups.Clear();
                    groups = newgroups;
                    parentWindow.refresh_Listviews();
                }
                catch
                {
                    System.Windows.MessageBox.Show("不正確的路徑\r\n提示：不可以使用磁碟機之根目錄");
                }
                OnPropertyChanged("");
            }
        }

        /// <summary>
        /// 檔案清單
        /// </summary>
        public ObservableCollection<File> files
        {
            get
            {
                return Model_Instance.list_file;
            }
            set
            {
                Model_Instance.list_file = value;
                OnPropertyChanged("");
            }
        }
        /// <summary>
        /// 年份清單(於載入檔案清單時建立)
        /// </summary>
        public ObservableCollection<Year> years
        {
            get
            {
                return Model_Instance.list_year;
            }
            set
            {
                Model_Instance.list_year = value;
                OnPropertyChanged("");
            }
        }
        /// <summary>
        /// 組別清單(於載入檔案清單時建立)
        /// </summary>
        public ObservableCollection<Group> groups
        {
            get
            {
                return Model_Instance.list_group;
            }
            set
            {
                Model_Instance.list_group = value;
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
                Model_Instance.str_outputDir = GlobalSetting.outputDir = value;
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
        /// ICD include清單
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
        public string db_AgeL
        {
            get { return Model_Instance.db_AgeL.Round(1); }
            set
            {
                double result = -1;
                double.TryParse(value, out result);
                if (result > 100) result = 100;
                if (result < 0) result = 0;
                Model_Instance.db_AgeL = result;
                OnPropertyChanged(nameof(db_AgeL));
            }
        }
        public string db_AgeU
        {
            get { return Model_Instance.db_AgeU.Round(1); }
            set
            {
                double result = -1;
                double.TryParse(value, out result);
                if (result > 100) result = 100;
                if (result < 0) result = 0;
                Model_Instance.db_AgeU = result;
                OnPropertyChanged(nameof(db_AgeU));
            }
        }
        #endregion

        #region -- ID criteria controls
        public string IDCriteriaFolderPath
        {
            get { return Model_Instance.IDCriteriaFolderPath; }
            set
            {
                Model_Instance.IDCriteriaFolderPath = GlobalSetting.IDcriteriaDir = value;
                try
                {
                    string[] paths;
                    paths = Directory.EnumerateFiles(value, "*CD*.EXT", SearchOption.AllDirectories).ToArray();
                    var newfiles = new List<File>();
                    foreach (string str_filepath in paths)
                    {
                        newfiles.Add(new File(str_filepath));
                    }
                    IDCriteriaFileList.Clear();
                    IDCriteriaFileList = newfiles;
                    var groupCount = (from q in IDCriteriaFileList group q by q.@group into g select g).Count();
                    IDCriteriaMessage = "Total " + IDCriteriaFileList.Count() + " files was loaded. Group count: " + groupCount;
                }
                catch
                {
                    IDCriteriaMessage = "Invalid path";
                }
                OnPropertyChanged(nameof(IDCriteriaFolderPath));
            }
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
        public List<File> IDCriteriaFileList
        {
            get { return Model_Instance.IDCriteria_FileList; }
            set
            {
                Model_Instance.IDCriteria_FileList = value;
            }
        }
        #endregion

        public bool IsIDCriteriaEnable
        {
            get { return Model_Instance.IsIDCriteriaEnable; }
            set { Model_Instance.IsIDCriteriaEnable = value; }
        }
        public bool IsOrderCriteriaEnable
        {
            get { return Model_Instance.IsOrderCriteriaEnable; }
            set { Model_Instance.IsOrderCriteriaEnable = value; }
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
