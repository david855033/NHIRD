﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    internal class JoinOrderModel
    {
        JoinOrderViewModel parentVM;
        public JoinOrderModel(JoinOrderViewModel parentVM)
        {
            this.parentVM = parentVM;
            actionFiles = new ObservableCollection<File>();
            orderFiles = new ObservableCollection<File>();
        }
        #region -- file control
        public string str_InputDirAction { get; set; }
        public ObservableCollection<File> actionFiles { get; set; }
        public string str_InputDirOrder { get; set; }
        public ObservableCollection<File> orderFiles { get; set; }
        public string str_OuputDir { get; set; }
        #endregion
        #region -- orderGroupList
        public List<OrderGroup> orderGroupList;
        #endregion

        public void matchActionAndOrderFile()
        {
            System.Windows.MessageBox.Show($"{actionFiles.Count} action files and {orderFiles.Count} order files");
        }

        public void joinOrder()
        {
            System.Windows.MessageBox.Show($"{orderGroupList.Count} groups");
        }
    }
}