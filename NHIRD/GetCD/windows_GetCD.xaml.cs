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
using System.Text.RegularExpressions;
namespace NHIRD
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>

    public partial class GetCD_Window : Window
    {
        public readonly GetCD_ViewModel ViewModel_Instance;
        public MainWindow parentWindow;
        // -- constructor
        public GetCD_Window(MainWindow parent)
        {
            InitializeComponent();
            ViewModel_Instance = new GetCD_ViewModel(this);
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            this.DataContext = ViewModel_Instance;
            ViewModel_Instance.InputDir = GlobalSetting.inputDir;
            ViewModel_Instance.str_outputDir = GlobalSetting.outputDir;
            ViewModel_Instance.IDCriteriaFolderPath = GlobalSetting.IDcriteriaDir;
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
        /// 選取讀入資料夾
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectInputDir_Click(object sender, RoutedEventArgs e)
        {
            var FolderSelector = new FolderBrowserDialog();
            if (FolderSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModel_Instance.InputDir = FolderSelector.SelectedPath;
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
            ViewModel_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void yearsCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_years.ItemsSource == null) return;
            foreach (Year item in listview_years.ItemsSource)
            {
                item.selected = false;
            }
            ViewModel_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void yearsCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Year).selected = true;
            ViewModel_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void yearsCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Year).selected = false;
            ViewModel_Instance.Model_Instance.checkFileByCriteria();
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
            ViewModel_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void groupsCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_groups.ItemsSource == null) return;
            foreach (Group item in listview_groups.ItemsSource)
            {
                item.selected = false;
            }
            ViewModel_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void groupsCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Group).selected = true;
            ViewModel_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        private void groupsCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Group).selected = false;
            ViewModel_Instance.Model_Instance.checkFileByCriteria();
            refresh_Listviews();
        }
        #endregion

        /// <summary>
        /// 更動各種list後呼叫，後重建三個list以觸發OnPropertyChanged
        /// </summary>
        public void refresh_Listviews()
        {
            ViewModel_Instance.years = new ObservableCollection<Year>(ViewModel_Instance.years);
            ViewModel_Instance.files = new ObservableCollection<File>(ViewModel_Instance.files);
            ViewModel_Instance.groups = new ObservableCollection<Group>(ViewModel_Instance.groups);
            ViewModel_Instance.FileStatus = "Selected " + ViewModel_Instance.files.Where(x => x.selected == true).Count() +
                " / " + ViewModel_Instance.files.Count + " files.";
        }
        /// <summary>
        /// 選取輸出資料夾
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectOutputDir_Click(object sender, RoutedEventArgs e)
        {
            var FolderSelector = new FolderBrowserDialog();
            if (FolderSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModel_Instance.str_outputDir = FolderSelector.SelectedPath;
            }
        }
        #region ICD include & Exclude按鈕功能
        private void ButtonAddIncl_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel_Instance.ICDIncludes.Any(x => x == inputIncl.Text) && inputIncl.Text != "")
            {
                this.ViewModel_Instance.ICDIncludes.Add(inputIncl.Text);
                inputIncl.Text = "";
                Cb_ICDinclude.IsChecked = true;
            }
        }
        private void ButtonEdtIncl_Click(object sender, RoutedEventArgs e)
        {
            if (lv_Incl.SelectedItem != null && !ViewModel_Instance.ICDIncludes.Any(x => x == inputIncl.Text))
            {
                var index = lv_Incl.SelectedIndex;
                this.ViewModel_Instance.ICDIncludes.RemoveAt(index);
                this.ViewModel_Instance.ICDIncludes.Insert(index, inputIncl.Text);
            }
        }
        private void Button_DelInclClick(object sender, RoutedEventArgs e)
        {
            if (lv_Incl.SelectedItem != null)
            {
                var index = lv_Incl.SelectedIndex;
                this.ViewModel_Instance.ICDIncludes.RemoveAt(index);
                if (this.ViewModel_Instance.ICDIncludes.Count() == 0)
                {
                    Cb_ICDinclude.IsChecked = false;
                }
            }
        }
        private void ButtonClrIncl_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel_Instance.ICDIncludes.Clear();
            Cb_ICDinclude.IsChecked = false;
        }
        private void ButtonAddExcl_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel_Instance.ICDExcludes.Any(x => x == inputExcl.Text) && inputExcl.Text != "")
            {
                this.ViewModel_Instance.ICDExcludes.Add(inputExcl.Text);
                inputExcl.Text = "";
                Cb_ICDexclude.IsChecked = true;
            }
        }
        private void ButtonEdtExcl_Click(object sender, RoutedEventArgs e)
        {
            if (lv_Excl.SelectedItem != null && !ViewModel_Instance.ICDExcludes.Any(x => x == inputExcl.Text))
            {
                var Exdex = lv_Excl.SelectedIndex;
                this.ViewModel_Instance.ICDExcludes.RemoveAt(Exdex);
                this.ViewModel_Instance.ICDExcludes.Insert(Exdex, inputExcl.Text);
            }
        }
        private void Button_DelExclClick(object sender, RoutedEventArgs e)
        {
            if (lv_Excl.SelectedItem != null)
            {
                var Exdex = lv_Excl.SelectedIndex;
                this.ViewModel_Instance.ICDExcludes.RemoveAt(Exdex);
                if (this.ViewModel_Instance.ICDExcludes.Count() == 0)
                {
                    Cb_ICDexclude.IsChecked = false;
                }
            }
        }
      
        private void ButtonClrExcl_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel_Instance.ICDExcludes.Clear();
            Cb_ICDexclude.IsChecked = false;
        }
        #endregion
        /// <summary>
        /// 限制Text Box只能輸入float
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Textbox_FloatVerify_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var s = sender as System.Windows.Controls.TextBox;
            s.Text = s.Text.RegexFloat();
            s.Select(s.Text.Length, 0);
        }
        /// <summary>
        /// 選取ID Criteriea list 資料夾
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectIDCriteria_Click(object sender, RoutedEventArgs e)
        {
            var FolderSelector = new FolderBrowserDialog();
            if (FolderSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModel_Instance.IDCriteriaFolderPath = FolderSelector.SelectedPath;
                Cb_ID.IsChecked = true;
            }
        }
    }
}
