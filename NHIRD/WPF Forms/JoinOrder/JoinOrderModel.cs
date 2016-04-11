using System;
using System.Collections.Generic;
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
        }
        #region -- file control
        public string str_InputDirAction { get; set; }
        public string str_InputDirOrder { get; set; }
        public string str_OuputDir { get; set; }
        #endregion
        #region -- orderGroupList
        public List<OrderGroup> orderGroupList;
        #endregion

        public void joinOrder()
        {
            System.Windows.MessageBox.Show($"{orderGroupList.Count} groups");
        }
    }
}
