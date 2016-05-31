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
    public partial class AgeSpecificIncidenceWindow : Window
    {
        public MainWindow parentWindow;
        AgeSpecificIncidenceViewModel ViewModel_Instance;
        public AgeSpecificIncidenceWindow(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            ViewModel_Instance = new AgeSpecificIncidenceViewModel(this);
            this.DataContext = ViewModel_Instance;

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel_Instance.patientBasedDataFolderPath =
                  GlobalSetting.get("ASI_PBD");
            ViewModel_Instance.standarizedIDFolderPath =
                  GlobalSetting.get("ASI_SIT");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel_Instance.generateMatchResult();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewModel_Instance.generateAgeSpecificIncidence();
        }
    }
}