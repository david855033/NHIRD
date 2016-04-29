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
    /// PatientBasedDataWindow.xaml 的互動邏輯
    /// </summary>
    public partial class PatientBasedDataWindow : Window
    {
        public MainWindow parentWindow;
        PatientBasedDataViewModal ViewModel_Instance;
        public PatientBasedDataWindow(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            ViewModel_Instance = new PatientBasedDataViewModal(this);
            this.DataContext = ViewModel_Instance;

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ActionFolderSelector.FolderPath =
               GlobalSetting.get("PatientBasedData_InputDir");
            OutputFolderSelector.FolderPath =
                GlobalSetting.get("PatientBasedData_OutputDir");
        }

        private void Do_Click(object sender, RoutedEventArgs e)
        {
            ViewModel_Instance.outputDir = "Now";
        }
    }
}