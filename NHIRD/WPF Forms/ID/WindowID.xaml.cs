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
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class Window_ID : Window
    {
        public readonly ID_ViewModel ViewModel_Instance;
        public MainWindow parentWindow;
        public Window_ID(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            ViewModel_Instance = new ID_ViewModel(this);
            this.DataContext = ViewModel_Instance;
        }

        private void WindowID_Loaded(object sender, RoutedEventArgs e)
        {
            inputFolderSelect.FolderPath =
              GlobalSetting.get("ID_InputDir");
            ouputFolderSelect.FolderPath =
              GlobalSetting.get("ID_OutputDir");
        }

        private void WindowID_Unloaded(object sender, RoutedEventArgs e)
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
