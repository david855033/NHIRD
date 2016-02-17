using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NHIRD
{
    class GetOrder_ViewModel
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

        public bool IsOOFileTypeEnabled
        {
            get { return Model_Instance.IsOOFileTypeEnabled; }
            set
            {
                Model_Instance.IsOOFileTypeEnabled = value;
                makeFileList(InputDir);
                OnPropertyChanged(nameof(IsOOFileTypeEnabled));
            }
        }
        public bool IsDOFileTypeEnabled
        {
            get { return Model_Instance.IsDOFileTypeEnabled; }
            set
            {
                Model_Instance.IsDOFileTypeEnabled = value;
                makeFileList(InputDir);
                OnPropertyChanged(nameof(IsDOFileTypeEnabled));
            }
        }
        public bool IsGOFileTypeEnabled
        {
            get { return Model_Instance.IsGOFileTypeEnabled; }
            set
            {
                Model_Instance.IsGOFileTypeEnabled = value;
                makeFileList(InputDir);
                OnPropertyChanged(nameof(IsGOFileTypeEnabled));
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
                GlobalSetting.set("Order_InputDir", value);
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
                Model_Instance.str_outputDir =value;
                GlobalSetting.set("Order_OutputDir", value);
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
