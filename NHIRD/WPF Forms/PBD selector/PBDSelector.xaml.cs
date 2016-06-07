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


    public partial class PBDSelectorWindow : Window
    {
        public MainWindow parentWindow;
        PBDSelectorViewModel ViewModel_Instance;
        public PBDSelectorWindow(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            ViewModel_Instance = new PBDSelectorViewModel(this);
            this.DataContext = ViewModel_Instance;
        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel_Instance.patientBasedDataFolderPath =
                  GlobalSetting.get("PBDSelector_PBD");
            ViewModel_Instance.outputDir =
                GlobalSetting.get("PBDSelector_outputDir");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel_Instance.Do();
        }
    }
}
