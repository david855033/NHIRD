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
            ViewModel_Instance.InputDir = GlobalSetting.get("Order_InputDir");
            ViewModel_Instance.str_outputDir = GlobalSetting.get("Order_OutputDir");
            refresh_Listviews();
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
        
        /// <summary>
        /// 更動各種list後呼叫，後重建三個list以觸發OnPropertyChanged
        /// </summary>
        public void refresh_Listviews()
        {
            ViewModel_Instance.inputYearList = new ObservableCollection<Year>(ViewModel_Instance.inputYearList);
            ViewModel_Instance.inputFileList = new ObservableCollection<File>(ViewModel_Instance.inputFileList);
            ViewModel_Instance.inputGroupList = new ObservableCollection<Group>(ViewModel_Instance.inputGroupList);
            ViewModel_Instance.FileStatus = "Selected " + ViewModel_Instance.inputFileList.Where(x => x.selected == true).Count() +
                " / " + ViewModel_Instance.inputFileList.Count + " files.";
        }
    }
}
