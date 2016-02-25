using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using System.IO;
namespace NHIRD
{
    public class GetCD_Model
    {
        /// <summary>
        /// 上層之VM
        /// </summary>
        GetCD_ViewModel parentVM;
        // -- contructor
        public GetCD_Model(GetCD_ViewModel parentVM)
        {
            this.parentVM = parentVM;
            renewSelectedFileTypes();
        }

        #region -- FileType Select
        bool _IsCDFileTypeEnabled = true;
        public bool IsCDFileTypeEnabled { get { return _IsCDFileTypeEnabled; } set { _IsCDFileTypeEnabled = value; renewSelectedFileTypes(); } }
        bool _IsDDFileTypeEnabled = true;
        public bool IsDDFileTypeEnabled { get { return _IsDDFileTypeEnabled; } set { _IsDDFileTypeEnabled = value; renewSelectedFileTypes(); } }
        List<string> _selectedFileTypes = new List<string>();
        void renewSelectedFileTypes()
        {
            _selectedFileTypes.Clear();
            if (_IsCDFileTypeEnabled) _selectedFileTypes.Add("CD");
            if (_IsDDFileTypeEnabled) _selectedFileTypes.Add("DD");
        }
        /// <summary>
        /// 被選取的FileType
        /// </summary>
        public List<string> selectedFileTypes
        {
            get { return _selectedFileTypes; }
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

        /// <summary>
        /// 顯示除錯用訊息
        /// </summary>
        public string message { get; set; }
        #region ICD criteria control
        /// <summary>
        /// 儲存ICD inclusion criteria
        /// </summary>
        public ObservableCollection<string> list_ICDinclude = new ObservableCollection<string>();
        /// <summary>
        /// 儲存ICD inclusion criteria
        /// </summary>
        public ObservableCollection<string> list_ICDExclude = new ObservableCollection<string>();
        public bool IsICDIncludeEnabled;
        public bool IsICDExcludeEnabled;
        #endregion

        #region PROCcriteria control
        /// <summary>
        /// 儲存PROC inclusion criteria
        /// </summary>
        public ObservableCollection<string> list_PROCinclude = new ObservableCollection<string>();
        /// <summary>
        /// 儲存ICD inclusion criteria
        /// </summary>
        public ObservableCollection<string> list_PROCExclude = new ObservableCollection<string>();
        public bool IsPROCIncludeEnabled;
        public bool IsPROCExcludeEnabled;
        #endregion

        #region Age Criteria Controls
        public bool IsAgeLCriteriaEnable { get; set; }
        public bool IsAgeUCriteriaEnable { get; set; }
        public double db_AgeL { get; set; }
        public double db_AgeU { get; set; }
        #endregion

        #region ID criteria controls
        public string IDCriteriaFolderPath { get; set; }
        public string IDCriteriaMessage { get; set; }
        public ObservableCollection<File> IDCriteriaFileList = new ObservableCollection<File>();
        public bool IsIDCriteriaEnable { get; set; }
        #endregion

        #region Order Criteria Controls
        public string OrderCriteriaFolderPath { get; set; }
        public string OrderCriteriaMessage { get; set; }
        public ObservableCollection<File> OrderCriteriaFileList = new ObservableCollection<File>();
        public bool IsOrderCriteriaEnable { get; set; }
        #endregion

        /// <summary>
        /// 匯入條件並且提取檔案
        /// </summary>
        public void DoExtractData()
        {
            //建立執行個體
            var extractData = new ExtractData();
            //判斷是否啟動ICD 條件
            if (IsICDIncludeEnabled)
            {
                if (IsICDExcludeEnabled)
                {
                    extractData.CriteriaList.Add(new ExtractData.Criteria()
                    {
                        key = "ICD9",
                        StringIncludeList = list_ICDinclude.ToList(),
                        StringExcludeList = list_ICDExclude.ToList()
                    });
                }
                else
                {
                    extractData.CriteriaList.Add(new ExtractData.Criteria()
                    {
                        key = "ICD9",
                        StringIncludeList = list_ICDinclude.ToList()
                    });
                }
            }
            //判斷是否啟動PROC 條件
            if (IsPROCIncludeEnabled)
            {
                if (IsPROCExcludeEnabled)
                {
                    extractData.CriteriaList.Add(new ExtractData.Criteria()
                    {
                        key = "ICD_OP",
                        StringIncludeList = list_PROCinclude.ToList(),
                        StringExcludeList = list_PROCExclude.ToList()
                    });
                }
                else
                {
                    extractData.CriteriaList.Add(new ExtractData.Criteria()
                    {
                        key = "ICD_OP",
                        StringIncludeList = list_PROCinclude.ToList()
                    });
                }
            }
            //判斷是否啟動年齡條件
            if (IsAgeLCriteriaEnable || IsAgeUCriteriaEnable)
            {
                extractData.CriteriaList.Add(new ExtractData.Criteria()
                {
                    key = "AGE",
                    CriteriaNumUpper = IsAgeUCriteriaEnable ? db_AgeU : 0,
                    CriteriaNumLower = IsAgeLCriteriaEnable ? db_AgeL : 0
                });
            }
            //判斷是否啟動ID條件
            if (IsIDCriteriaEnable)
            {
                extractData.CriteriaList.Add(new ExtractData.Criteria()
                {
                    key = "IDLIST",
                    IDCriteriaFileList = IDCriteriaFileList
                });
            }
            //判斷是否啟動Order(ActionList)條件
            if (IsOrderCriteriaEnable)
            {
                extractData.CriteriaList.Add(new ExtractData.Criteria()
                {
                    key = "ACTIONLIST",
                    ActionCriteriaFileList = OrderCriteriaFileList
                });
            }


            //執行
            extractData.Do(parentVM.parentWindow.parentWindow.rawDataFormats, selectedFileTypes.ToArray(), from f in inputFileList where f.selected == true select f, str_outputDir);
        }

    }

}
