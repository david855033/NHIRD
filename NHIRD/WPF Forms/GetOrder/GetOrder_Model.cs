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
        }


        #region -- file control
        /// <summary>
        /// 儲存檔案清單
        /// </summary>
        public ObservableCollection<File> inputFileList = new ObservableCollection<File>();
        /// <summary>
        /// 輸出檔案的資料夾路徑
        /// </summary>
        public string str_outputDir { get; set; }
        #endregion


 
        #region -- Order criteria control
        /// <summary>
        /// Order inclusion criteria
        /// </summary>
        public ObservableCollection<string> list_Orderinclude = new ObservableCollection<string>();
        public bool IsOrderCriteriaEnable;
        #endregion

        #region Action Criteria Controls
        public string ActionCriteriaFolderPath { get; set; }
        public ObservableCollection<File> ActionCriteriaFileList = new ObservableCollection<File>();
        public bool IsActionCriteriaEnable { get; set; }
        #endregion

        /// <summary>
        /// 匯入條件並且提取檔案
        /// </summary>
        public void DoExtractData()
        {

            //建立執行個體
            var extractData = new ExtractData();
            //使用ORDER代碼
            if (IsOrderCriteriaEnable)
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
            //判斷是否啟動ActionList條件
            if (IsActionCriteriaEnable)
            {
                extractData.CriteriaList.Add(new ExtractData.Criteria()
                {
                    key = "ACTIONLIST",
                    ActionCriteriaFileList = ActionCriteriaFileList
                });
            }

            //執行
            extractData.Do(parentVM.parentWindow.parentWindow.rawDataFormats,
                (from f in inputFileList where f.selected == true select f.FileType).Distinct()
                , from f in inputFileList where f.selected == true select f,
                str_outputDir);
        }

    }
}

