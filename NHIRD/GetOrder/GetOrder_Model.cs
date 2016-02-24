using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class GetOrder_Model
    {
        /// <summary>
        /// 上層之VM
        /// </summary>
        GetOrder_ViewModel parentVM;
        // -- contructor
        public GetOrder_Model(GetOrder_ViewModel parentVM)
        {
            this.parentVM = parentVM;
            renewSelectedFileTypes();
        }

        #region -- FileType Select
        bool _IsOOFileTypeEnabled = true;
        public bool IsOOFileTypeEnabled
        {
            get { return _IsOOFileTypeEnabled; }
            set
            {
                _IsOOFileTypeEnabled = value; renewSelectedFileTypes();
            }
        }
        bool _IsDOFileTypeEnabled = true;
        public bool IsDOFileTypeEnabled
        {
            get { return _IsDOFileTypeEnabled; }
            set
            {
                _IsDOFileTypeEnabled = value; renewSelectedFileTypes();
            }
        }
        bool _IsGOFileTypeEnabled = true;
        public bool IsGOFileTypeEnabled
        {
            get { return _IsGOFileTypeEnabled; }
            set
            {
                _IsGOFileTypeEnabled = value; renewSelectedFileTypes();
            }
        }
        List<string> _selectedFileTypes = new List<string>();
        /// <summary>
        /// 被選取的FileType
        /// </summary>
        public List<string> selectedFileTypes
        {
            get { return _selectedFileTypes; }
        }
        void renewSelectedFileTypes()
        {
            _selectedFileTypes.Clear();
            if (_IsOOFileTypeEnabled) _selectedFileTypes.Add("OO");
            if (_IsDOFileTypeEnabled) _selectedFileTypes.Add("DO");
            if (_IsGOFileTypeEnabled) _selectedFileTypes.Add("GO");
        }
        #endregion
        #region -- Input file control
        /// <summary>
        /// 讀取檔案的資料夾路徑
        /// </summary>
        public string str_inputDir { get; set; }
        /// <summary>
        /// 輸出檔案的資料夾路徑
        /// </summary>
        public string str_outputDir { get; set; }
        /// <summary>
        /// 儲存檔案清單
        /// </summary>
        public ObservableCollection<File> inputFileList = new ObservableCollection<File>();
        /// <summary>
        /// 儲存年份清單
        /// </summary>
        public ObservableCollection<Year> inputYearList = new ObservableCollection<Year>();
        /// <summary>
        /// 儲存R group清單
        /// </summary>
        public ObservableCollection<Group> inputGroupList = new ObservableCollection<Group>();
        /// <summary>
        /// 紀錄選取檔案數量的顯示訊息
        /// </summary>
        public string str_filestatus { get; set; }
        /// <summary>
        /// 更動group或year選取狀態時，更新檔案選取清單
        /// </summary>
        public void checkFileByCriteria()
        {
            foreach (File f in inputFileList)
            {
                f.selected = false;
            }
            var query = (
                            from p in inputFileList
                            where inputYearList.Where(x => x.selected == true)
                            .Select(x => x.str_year)
                            .Contains(p.year) &&
                            inputGroupList.Where(x => x.selected == true)
                            .Select(x => x.str_group)
                            .Contains(p.@group)
                            select p
                          )
                         .ToList();
            foreach (File f in query)
            {
                f.selected = true;
            }
        }
        #endregion


        #region -- Order criteria control
        /// <summary>
        /// Order inclusion criteria
        /// </summary>
        public ObservableCollection<string> list_Orderinclude = new ObservableCollection<string>();
        public bool IsOrderIncludeEnabled;
        #endregion

        /// <summary>
        /// 顯示除錯用訊息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 匯入條件並且提取檔案
        /// </summary>
        public void DoExtractData()
        {

            //建立執行個體
            var extractData = new ExtractData();
            //使用ORDER代碼
            if (IsOrderIncludeEnabled)
            {
                extractData.CriteriaList.Add(new ExtractData.Criteria()
                {
                    key = "DRUG_NO",
                    StringIncludeList = list_Orderinclude.ToList()
                });
                extractData.CriteriaList.Add(new ExtractData.Criteria()
                {
                    key = "ORDER_CODE",
                    StringIncludeList = list_Orderinclude.ToList()
                });
            }
            
            //執行
            extractData.Do(parentVM.parentWindow.parentWindow.rawDataFormats, selectedFileTypes.ToArray(),
                from f in inputFileList where f.selected == true select f, str_outputDir);
        }

    }
}

