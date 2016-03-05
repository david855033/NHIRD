using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NHIRD
{
    /// <summary>
    /// WindowGetOrder.xaml 的互動邏輯
    /// </summary>
    public partial class Window_GetOrder : Window
    {
        public readonly GetOrder_ViewModel ViewModel_Instance;
        public MainWindow parentWindow;
        // -- constructor
        public Window_GetOrder(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            ViewModel_Instance = new GetOrder_ViewModel(this);
            this.DataContext = ViewModel_Instance;           
        }
        /// <summary>
        /// 從global setting載入預設值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowGetOrder_Loaded(object sender, RoutedEventArgs e)
        {
            inputFolderSelect.FolderPath =
                GlobalSetting.get("Order_InputDir");
            ouputFolderSelect.FolderPath =
                GlobalSetting.get("Order_OutputDir");
            ActionCrieteria_FolderSelect.FolderPath =
                GlobalSetting.get("Order_ActionCriteriaDir");
        }

        /// <summary>
        /// 切回menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowGetOrder_Unloaded(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
        }


        private void inputFolderSelect_OnFolderChanged()
        {
            ViewModel_Instance.InputDir = inputFolderSelect.FolderPath;
        }

        private void OuputFolderSelect_OnFolderChanged()
        {
            ViewModel_Instance.str_outputDir = ouputFolderSelect.FolderPath;
        }



    }
}
