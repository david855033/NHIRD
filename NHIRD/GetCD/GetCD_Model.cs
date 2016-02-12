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
        public ObservableCollection<File> list_file = new ObservableCollection<File>();
        /// <summary>
        /// 儲存年份清單
        /// </summary>
        public ObservableCollection<Year> list_year = new ObservableCollection<Year>();
        /// <summary>
        /// 儲存R group清單
        /// </summary>
        public ObservableCollection<Group> list_group = new ObservableCollection<Group>();
        /// <summary>
        /// 紀錄選取檔案數量的顯示訊息
        /// </summary>
        public string str_filestatus { get; set; }
        /// <summary>
        /// 顯示除錯用訊息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 儲存ICD inclusion criteria
        /// </summary>
        public ObservableCollection<string> list_ICDinclude = new ObservableCollection<string>();
        /// <summary>
        /// 儲存ICD inclusion criteria
        /// </summary>
        public ObservableCollection<string> list_ICDExclude = new ObservableCollection<string>();
        /// <summary>
        /// 更動group或year選取狀態時，更新檔案選取清單
        /// </summary>
        public void checkFileByCriteria()
        {
            foreach (File f in list_file)
            {
                f.selected = false;
            }
            var query = (
                            from p in list_file
                            where list_year.Where(x => x.selected == true)
                            .Select(x => x.str_year)
                            .Contains(p.year) &&
                            list_group.Where(x => x.selected == true)
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

        #region Age Criteria Controls
        public bool IsAgeLCriteriaEnable { get; set; }
        public bool IsAgeUCriteriaEnable { get; set; }
        public double db_AgeL { get; set; }
        public double db_AgeU { get; set; }
        #endregion

        #region ID criteria controls
        public string IDCriteriaFolderPath { get; set; }
        public string IDCriteriaMessage { get; set; }
        public List<File> IDCriteria_FileList = new List<File>();
        #endregion

        public bool IsIDCriteriaEnable { get; set; }
        public bool IsOrderCriteriaEnable { get; set; }

        /// <summary>
        /// 匯入條件並且提取檔案
        /// </summary>
        public void DoExtractData()
        {
            var extractData = new ExtractData();
            if (list_ICDinclude.Count > 0)
            {
                extractData.CriteriaList.Add(new ExtractData.Criteria() { colname = "ICD", StringList = list_ICDinclude.ToList() });
            }

            if (IsAgeLCriteriaEnable || IsAgeUCriteriaEnable)
            {
                extractData.CriteriaList.Add(new ExtractData.Criteria()
                {
                    colname = "AGE",
                    CriteriaNumUpper = IsAgeUCriteriaEnable ? db_AgeU : 0,
                    CriteriaNumLower = IsAgeLCriteriaEnable ? db_AgeL : 0
                });
            }
            extractData.Do(parentVM.parentWindow.parentWindow.rawDataFormats, "CD", from f in list_file where f.selected == true select f, str_outputDir);
        }

    }

}
