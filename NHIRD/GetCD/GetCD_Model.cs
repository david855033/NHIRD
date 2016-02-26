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
        }
 
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
        /// 儲存檔案清單(在執行前直接設定，不使用binding)
        /// </summary>
        public List<File> inputFileList = new List<File>();
        
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
            extractData.Do(parentVM.parentWindow.parentWindow.rawDataFormats, (from f in inputFileList where f.selected == true select f.@group).Distinct()
                                                                              , from f in inputFileList where f.selected == true select f, str_outputDir);
        }

    }

}
