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

    public partial class GetCD : Window
    {
        private readonly GetCD_ViewModel ViewModal_Instance;
        private Window _parentWindow;
        // -- contructor
        public GetCD(Window parent)
        {
            InitializeComponent();
            ViewModal_Instance = new GetCD_ViewModel(this);
            _parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            this.DataContext = ViewModal_Instance;
        }

        private void WindowGetCD_Unloaded(object sender, RoutedEventArgs e)
        {
            _parentWindow.Show();
        }

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

            foreach (file item in listview_files.ItemsSource)
            {
                item.selected = true;
            }
            ViewModal_Instance.files = new ObservableCollection<file>(ViewModal_Instance.files);
        }
        private void FilesCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_files.ItemsSource == null) return;
            foreach (file item in listview_files.ItemsSource)
            {
                item.selected = false;
            }
            ViewModal_Instance.files = new ObservableCollection<file>(ViewModal_Instance.files);
        }
        private void FilesCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as file).selected = true;
            ViewModal_Instance.files = new ObservableCollection<file>(ViewModal_Instance.files);
        }
        private void FilesCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as file).selected = false;
            ViewModal_Instance.files = new ObservableCollection<file>(ViewModal_Instance.files);
        }
        #endregion

        #region CodeBehind for years listview
        private void yearsCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            if (listview_years.ItemsSource == null) return;
            foreach (year item in listview_years.ItemsSource)
            {
                item.selected = true;
            }
            ViewModal_Instance.years = new ObservableCollection<year>(ViewModal_Instance.years);
        }
        private void yearsCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_years.ItemsSource == null) return;
            foreach (year item in listview_years.ItemsSource)
            {
                item.selected = false;
            }
            ViewModal_Instance.years = new ObservableCollection<year>(ViewModal_Instance.years);
        }
        private void yearsCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as year).selected = true;
            ViewModal_Instance.years = new ObservableCollection<year>(ViewModal_Instance.years);
        }
        private void yearsCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as year).selected = false;
            ViewModal_Instance.years = new ObservableCollection<year>(ViewModal_Instance.years);
        }
        #endregion

        #region CodeBehind for groups listview
        private void groupsCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            if (listview_groups.ItemsSource == null) return;
            foreach (group item in listview_groups.ItemsSource)
            {
                item.selected = true;
            }
            ViewModal_Instance.groups = new ObservableCollection<group>(ViewModal_Instance.groups);
        }
        private void groupsCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_groups.ItemsSource == null) return;
            foreach (group item in listview_groups.ItemsSource)
            {
                item.selected = false;
            }
            ViewModal_Instance.groups = new ObservableCollection<group>(ViewModal_Instance.groups);
        }
        private void groupsCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as group).selected = true;
            ViewModal_Instance.groups = new ObservableCollection<group>(ViewModal_Instance.groups);
        }
        private void groupsCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as group).selected = false;
            ViewModal_Instance.groups = new ObservableCollection<group>(ViewModal_Instance.groups);
        }
        #endregion

        
    }
}
