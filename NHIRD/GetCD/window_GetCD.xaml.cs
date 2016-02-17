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

    public partial class Window_GetCD : Window
    {
        public readonly GetCD_ViewModel ViewModel_Instance;
        public MainWindow parentWindow;
        // -- constructor
        public Window_GetCD(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
            ViewModel_Instance = new GetCD_ViewModel(this);
            this.DataContext = ViewModel_Instance;
            ViewModel_Instance.InputDir = GlobalSetting.get("CD_InputDir");
            ViewModel_Instance.str_outputDir = GlobalSetting.get("CD_OutputDir");
            ViewModel_Instance.IDCriteriaFolderPath = GlobalSetting.get("CD_IDCriteriaDir");
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
            ViewModel_Instance.inputYearList = new ObservableCollection<Year>(ViewModel_Instance.inputYearList);
            ViewModel_Instance.inputFileList = new ObservableCollection<File>(ViewModel_Instance.inputFileList);
            ViewModel_Instance.inputGroupList = new ObservableCollection<Group>(ViewModel_Instance.inputGroupList);
            ViewModel_Instance.FileStatus = "Selected " + ViewModel_Instance.inputFileList.Where(x => x.selected == true).Count() +
                " / " + ViewModel_Instance.inputFileList.Count + " files.";
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
        private void ButtonAddICDIncl_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel_Instance.ICDIncludes.Any(x => x == inputICDIncl.Text) && inputICDIncl.Text != "")
            {
                this.ViewModel_Instance.ICDIncludes.Add(inputICDIncl.Text);
                inputICDIncl.Text = "";
                Cb_ICDinclude.IsChecked = true;
            }
        }
        private void ButtonEdtICDIncl_Click(object sender, RoutedEventArgs e)
        {
            if (lv_ICDIncl.SelectedItem != null && !ViewModel_Instance.ICDIncludes.Any(x => x == inputICDIncl.Text))
            {
                var index = lv_ICDIncl.SelectedIndex;
                this.ViewModel_Instance.ICDIncludes.RemoveAt(index);
                this.ViewModel_Instance.ICDIncludes.Insert(index, inputICDIncl.Text);
            }
        }
        private void Button_DelICDInclClick(object sender, RoutedEventArgs e)
        {
            if (lv_ICDIncl.SelectedItem != null)
            {
                var index = lv_ICDIncl.SelectedIndex;
                this.ViewModel_Instance.ICDIncludes.RemoveAt(index);
                if (this.ViewModel_Instance.ICDIncludes.Count() == 0)
                {
                    Cb_ICDinclude.IsChecked = false;
                }
            }
        }
        private void ButtonClrICDIncl_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel_Instance.ICDIncludes.Clear();
            Cb_ICDinclude.IsChecked = false;
        }
        private void ButtonAddICDExcl_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel_Instance.ICDExcludes.Any(x => x == inputICDExcl.Text) && inputICDExcl.Text != "")
            {
                this.ViewModel_Instance.ICDExcludes.Add(inputICDExcl.Text);
                inputICDExcl.Text = "";
                Cb_ICDexclude.IsChecked = true;
            }
        }
        private void ButtonEdtICDExcl_Click(object sender, RoutedEventArgs e)
        {
            if (lv_ICDExcl.SelectedItem != null && !ViewModel_Instance.ICDExcludes.Any(x => x == inputICDExcl.Text))
            {
                var Exdex = lv_ICDExcl.SelectedIndex;
                this.ViewModel_Instance.ICDExcludes.RemoveAt(Exdex);
                this.ViewModel_Instance.ICDExcludes.Insert(Exdex, inputICDExcl.Text);
            }
        }
        private void Button_DelICDExclClick(object sender, RoutedEventArgs e)
        {
            if (lv_ICDExcl.SelectedItem != null)
            {
                var Exdex = lv_ICDExcl.SelectedIndex;
                this.ViewModel_Instance.ICDExcludes.RemoveAt(Exdex);
                if (this.ViewModel_Instance.ICDExcludes.Count() == 0)
                {
                    Cb_ICDexclude.IsChecked = false;
                }
            }
        }
        private void ButtonClrICDExcl_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel_Instance.ICDExcludes.Clear();
            Cb_ICDexclude.IsChecked = false;
        }
        #endregion

        #region PROC include & Exclude按鈕功能
        private void ButtonAddPROCIncl_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel_Instance.PROCIncludes.Any(x => x == inputPROCIncl.Text) && inputPROCIncl.Text != "")
            {
                this.ViewModel_Instance.PROCIncludes.Add(inputPROCIncl.Text);
                inputPROCIncl.Text = "";
                Cb_PROCinclude.IsChecked = true;
            }
        }
        private void ButtonEdtPROCIncl_Click(object sender, RoutedEventArgs e)
        {
            if (lv_PROCIncl.SelectedItem != null && !ViewModel_Instance.PROCIncludes.Any(x => x == inputPROCIncl.Text))
            {
                var index = lv_PROCIncl.SelectedIndex;
                this.ViewModel_Instance.PROCIncludes.RemoveAt(index);
                this.ViewModel_Instance.PROCIncludes.Insert(index, inputPROCIncl.Text);
            }
        }
        private void Button_DelPROCInclClick(object sender, RoutedEventArgs e)
        {
            if (lv_PROCIncl.SelectedItem != null)
            {
                var index = lv_PROCIncl.SelectedIndex;
                this.ViewModel_Instance.PROCIncludes.RemoveAt(index);
                if (this.ViewModel_Instance.PROCIncludes.Count() == 0)
                {
                    Cb_PROCinclude.IsChecked = false;
                }
            }
        }
        private void ButtonClrPROCIncl_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel_Instance.PROCIncludes.Clear();
            Cb_PROCinclude.IsChecked = false;
        }
        private void ButtonAddPROCExcl_Click(object sender, RoutedEventArgs e)
        {
            if (!ViewModel_Instance.PROCExcludes.Any(x => x == inputPROCExcl.Text) && inputPROCExcl.Text != "")
            {
                this.ViewModel_Instance.PROCExcludes.Add(inputPROCExcl.Text);
                inputPROCExcl.Text = "";
                Cb_PROCexclude.IsChecked = true;
            }
        }
        private void ButtonEdtPROCExcl_Click(object sender, RoutedEventArgs e)
        {
            if (lv_PROCExcl.SelectedItem != null && !ViewModel_Instance.PROCExcludes.Any(x => x == inputPROCExcl.Text))
            {
                var Exdex = lv_PROCExcl.SelectedIndex;
                this.ViewModel_Instance.PROCExcludes.RemoveAt(Exdex);
                this.ViewModel_Instance.PROCExcludes.Insert(Exdex, inputPROCExcl.Text);
            }
        }
        private void Button_DelPROCExclClick(object sender, RoutedEventArgs e)
        {
            if (lv_PROCExcl.SelectedItem != null)
            {
                var Exdex = lv_PROCExcl.SelectedIndex;
                this.ViewModel_Instance.PROCExcludes.RemoveAt(Exdex);
                if (this.ViewModel_Instance.PROCExcludes.Count() == 0)
                {
                    Cb_PROCexclude.IsChecked = false;
                }
            }
        }
        private void ButtonClrPROCExcl_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel_Instance.PROCExcludes.Clear();
            Cb_PROCexclude.IsChecked = false;
        }
        #endregion

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

        private void ResetAll_Click(object sender, RoutedEventArgs e)
        {
            ButtonClrICDIncl_Click(sender, e);
            ButtonClrICDExcl_Click(sender, e);
            ButtonClrPROCIncl_Click(sender, e);
            ButtonClrPROCExcl_Click(sender, e);
            foreach (var obj in Extentions.FindVisualChildren<System.Windows.Controls.CheckBox>(criteriaStackPanel))
            {
                obj.IsChecked = false;
            }

        }


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
    }
}
