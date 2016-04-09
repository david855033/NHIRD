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
    /// JoinOrderWindow.xaml 的互動邏輯
    /// </summary>
    public partial class JoinOrderWindow : Window
    {
        internal readonly JoinOrderViewModel ViewModel_Instance;
        public MainWindow parentWindow;
        public JoinOrderWindow(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            ViewModel_Instance = new JoinOrderViewModel(this);
            this.DataContext = ViewModel_Instance;
        }

        private void JoinOrderWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
        }

        private void JoinOrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ActionFolderSelector.FolderPath =
                GlobalSetting.get("JoinOrder_InputDirAction");
            OrderFolderSelector.FolderPath =
                GlobalSetting.get("JoinOrder_InputDirOrder");
            OutputFolderSelector.FolderPath =
                GlobalSetting.get("JoinOrder_OutputDir");
        }
    }
}
