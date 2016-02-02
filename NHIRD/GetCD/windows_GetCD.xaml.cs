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
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace NHIRD
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>

    public partial class GetCD_Window : Window
    {
        public readonly GetCD_ViewModel ViewModal_Instance;
        public MainWindow parentWindow;
        /// <summary>
        /// 初始化元件(constructior)
        /// </summary>
        /// <param name="parent"></param>
        public GetCD_Window(MainWindow parent)
        {
            InitializeComponent();
            ViewModal_Instance = new GetCD_ViewModel(this);
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            this.DataContext = ViewModal_Instance;
            refresh_Listviews();
        }
        /// <summary>
        /// 切回menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowGetCD_Unloaded(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
        }
        /// <summary>
        /// 選取資料夾
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectInputDir_Click(object sender, RoutedEventArgs e)
        {
            var FolderSelector = new FolderBrowserDialog();
            if (FolderSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModal_Instance.InputDir = FolderSelector.SelectedPath;
            }
        }

        #region CodeBehind for files listview
        private void FilesCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            if (listview_files.ItemsSource == null) return;
            foreach (File item in listview_files.ItemsSource)
            {
                item.selected = true;
            }
            refresh_Listviews();
        }
        private void FilesCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_files.ItemsSource == null) return;
            foreach (File item in listview_files.ItemsSource)
            {
                item.selected = false;
            }
            refresh_Listviews();
        }
        private void FilesCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as File).selected = true;
            refresh_Listviews();
        }
        private void FilesCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as File).selected = false;
            refresh_Listviews();
        }
        #endregion

        #region CodeBehind for years listview
        private void yearsCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            if (listview_years.ItemsSource == null) return;
            foreach (Year item in listview_years.ItemsSource)
            {
                item.selected = true;
            }
            ViewModal_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void yearsCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_years.ItemsSource == null) return;
            foreach (Year item in listview_years.ItemsSource)
            {
                item.selected = false;
            }
            ViewModal_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void yearsCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Year).selected = true;
            ViewModal_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void yearsCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Year).selected = false;
            ViewModal_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        #endregion

        #region CodeBehind for groups listview
        private void groupsCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            if (listview_groups.ItemsSource == null) return;
            foreach (Group item in listview_groups.ItemsSource)
            {
                item.selected = true;
            }
            ViewModal_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void groupsCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_groups.ItemsSource == null) return;
            foreach (Group item in listview_groups.ItemsSource)
            {
                item.selected = false;
            }
            ViewModal_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void groupsCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Group).selected = true;
            ViewModal_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void groupsCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Group).selected = false;
            ViewModal_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        #endregion

        public void refresh_Listviews()
        {
            ViewModal_Instance.years = new ObservableCollection<Year>(ViewModal_Instance.years);
            ViewModal_Instance.files = new ObservableCollection<File>(ViewModal_Instance.files);
            ViewModal_Instance.groups = new ObservableCollection<Group>(ViewModal_Instance.groups);
            ViewModal_Instance.FileStatus = "Selected " + ViewModal_Instance.files.Where(x => x.selected == true).Count() +
                " / " + ViewModal_Instance.files.Count + " files.";
        }

        private void SelectOutputDir_Click(object sender, RoutedEventArgs e)
        {
            var FolderSelector = new FolderBrowserDialog();
            if (FolderSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModal_Instance.str_outputDir = FolderSelector.SelectedPath;
            }
        }

    
    }
}
