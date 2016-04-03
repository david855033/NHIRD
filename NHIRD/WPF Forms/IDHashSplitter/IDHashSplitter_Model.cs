using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class IDHashSplitter_Model
    {
        readonly IDHashSplitter_ViewModel parentVM;
        public IDHashSplitter_Model(IDHashSplitter_ViewModel parentVM)
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

        public void Do_IDHashSplit()
        {
            new IDHashSplitter().Do(
                parentVM.parentWindow.parentWindow.rawDataFormats,
                inputFileList,
                str_outputDir
                );
        }
    }
}
