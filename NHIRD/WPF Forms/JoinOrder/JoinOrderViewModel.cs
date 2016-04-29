using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NHIRD
{
    internal class JoinOrderViewModel : BasedNotifyPropertyChanged
    {
        //initialiazation
        public readonly JoinOrderModel Model_Instance;
        public JoinOrderWindow parentWindow;
        public JoinOrderViewModel(JoinOrderWindow parent)
        {
            parentWindow = parent;
            Model_Instance = new JoinOrderModel(this);
            joinOrderCommand = new RelayCommand(joinOrder, (x) => true);
         
        }

        // -- Properties --
        #region input settings
        public string InputDirAction
        {
            get
            {
                return Model_Instance.str_InputDirAction;
            }
            set
            {
                Model_Instance.str_InputDirAction = value;
                GlobalSetting.set("JoinOrder_InputDirAction", value);
                OnPropertyChanged(nameof(InputDirAction));
            }
        }
        public ObservableCollection<File> actionFiles
        {
            get { return Model_Instance.actionFiles; }
            set { Model_Instance.actionFiles = value; }
        }

        public string InputDirOrder
        {
            get
            {
                return Model_Instance.str_InputDirOrder;
            }
            set
            {
                Model_Instance.str_InputDirOrder = value;
                GlobalSetting.set("JoinOrder_InputDirOrder", value);
                OnPropertyChanged(nameof(InputDirOrder));
            }
        }
        public ObservableCollection<File> orderFiles
        {
            get { return Model_Instance.orderFiles; }
            set { Model_Instance.orderFiles = value; }
        }

        #endregion

        /// <summary>
        /// 輸出資料夾
        /// </summary>
        public string str_outputDir
        {
            get
            {
                return Model_Instance.str_OuputDir;
            }
            set
            {
                Model_Instance.str_OuputDir = value;
                GlobalSetting.set("JoinOrder_OutputDir", value);
                OnPropertyChanged("");
            }
        }

        //--Action
        public ICommand joinOrderCommand { get; }
        public void joinOrder(object obj)
        {
            Model_Instance.orderGroupList = parentWindow.orderGroupEditor.orderGroupList;
            Model_Instance.joinOrder();
        }

    }
}
