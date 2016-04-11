using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class GetGDModel
    {
        GetGDViewModel parentVM;
        // -- contructor
        public GetGDModel(GetGDViewModel parentVM)
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

        #region ID criteria controls
        public string IDCriteriaFolderPath { get; set; }
        public ObservableCollection<File> IDCriteriaFileList = new ObservableCollection<File>();
        public bool IsIDCriteriaEnable { get; set; }
        #endregion

        /// <summary>
        /// 匯入條件並且提取檔案
        /// </summary>
        public void DoExtractData()
        {
            //建立執行個體
            var extractData = new ExtractData();

            //判斷是否啟動ID條件
            if (IsIDCriteriaEnable)
            {
                extractData.CriteriaList.Add(new ExtractData.Criteria()
                {
                    key = "IDLIST",
                    IDCriteriaFileList = IDCriteriaFileList
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
