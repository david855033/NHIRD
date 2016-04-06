using System;
using System.Collections.Generic;
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
    /// GetGDWindow.xaml 的互動邏輯
    /// </summary>
    public partial class GetGDWindow : Window
    {
        public readonly GetGDViewModel ViewModel_Instance;
        public MainWindow parentWindow;
        public GetGDWindow(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            ViewModel_Instance = new GetGDViewModel(this);
            this.DataContext = ViewModel_Instance;
        }
        /// <summary>
        /// 從global setting載入預設值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetGDWindow_Loaded(object sender, RoutedEventArgs e)
        {
            inputFolderSelect.FolderPath =
                GlobalSetting.get("GD_InputDir");
            ouputFolderSelect.FolderPath =
                GlobalSetting.get("GD_OutputDir");
            IDCrieteria_FolderSelect.FolderPath =
                GlobalSetting.get("GD_IDCriteriaDir");
        }
        /// <summary>
        /// 切回menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetGDWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
        }
    }
}
