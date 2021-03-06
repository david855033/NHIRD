﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    public class ID_Model
    {
        ID_ViewModel parentVM;
        public ID_Model(ID_ViewModel parentVM)
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

        public int upperLimit;
        public int lowerLimit;
        public bool isUpperLimitEnabled;
        public bool isLowerLimitEnabled;

        public void DoStandarizeID()
        {
            var standarizeID = new StandarizeID();
            if (isUpperLimitEnabled) standarizeID.birthYearUpperLimit = upperLimit;
            if (isLowerLimitEnabled) standarizeID.birthYearLowerLimit = lowerLimit;
            standarizeID.Do(parentVM.parentWindow.parentWindow.rawDataFormats,
                inputFileList,
                str_outputDir);
        }

    }
}
