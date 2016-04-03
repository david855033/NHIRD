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
    /// IDHashSplitter.xaml 的互動邏輯
    /// </summary>
    public partial class Window_IDHashSplitter : Window
    {
        public readonly IDHashSplitter_ViewModel ViewModel_Instance;
        public MainWindow parentWindow;
        public Window_IDHashSplitter(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            ViewModel_Instance = new IDHashSplitter_ViewModel(this);
            this.DataContext = ViewModel_Instance;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            inputFolderSelect.FolderPath =
             GlobalSetting.get("IDSplit_InputDir");
            ouputFolderSelect.FolderPath =
              GlobalSetting.get("IDSplit_OutputDir");
        }

        
    }
}
