using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NHIRD
{
    /// <summary>
    /// MainFIleListControl.xaml 的互動邏輯WPFDataTemplates 
    /// </summary>
    public partial class FileListControl : UserControl
    {
        public FileListControl()
        {
            InitializeComponent();
            (this as FrameworkElement).DataContext = this;
        }

        /// <summary>
        /// 目標檔案縮寫清單 如CD,DD等
        /// </summary>
        public List<string> selectedFileType
        {
            get { return (List<string>)GetValue(selectedFileTypeProperty); }
            set
            {
                SetValue(selectedFileTypeProperty, value);
            }
        }
        public static readonly DependencyProperty selectedFileTypeProperty =
        DependencyProperty.Register(nameof(selectedFileType), typeof(List<string>), typeof(FileListControl),new PropertyMetadata(new List<string> ()));

        /// <summary>
        /// 儲存讀取資料夾路徑
        /// </summary>
        public string inputDir
        {
            get { return (string)GetValue(inputDirProperty); }
            set
            {
                SetValue(inputDirProperty, value);       
            }
        }
        public static readonly DependencyProperty inputDirProperty =
        DependencyProperty.Register(nameof(inputDir), typeof(string), typeof(FileListControl));
        
        /// <summary>
        /// 公用變數，用來給設定input dir / selected file type並且觸發清單重製
        /// </summary>
        /// <param name="inputDir"></param>
        /// <param name="selectedFileType"></param>
        public void Renew(string inputDir, List<string> selectedFileType)
        {
            this.inputDir = inputDir;
            this.selectedFileType = selectedFileType;
            makeFileList();
        }
        
        /// <summary>
        /// 更動input dir時更新檔案清單
        /// </summary>
        /// <param name="inputDir"></param>
        void makeFileList()
        {
            try
            {
                List<string> paths = new List<string>();
                foreach (var currentFileType in selectedFileType)
                {
                    paths.AddRange(Directory.EnumerateFiles(inputDir, "*" + currentFileType + "*.DAT", SearchOption.AllDirectories));
                }
                paths.Sort();
                // -- file
                var newfiles = new List<File>();
                foreach (string str_filepath in paths)
                {
                    newfiles.Add(new File(str_filepath));
                }
                inputFileList.Clear();
                inputFileList = new ObservableCollection<File>(newfiles);
                // -- year
                var newyears = new List<Year>();
                foreach (string s in inputFileList.Select(x => x.year).Distinct())
                {
                    newyears.Add(new Year(s));
                }
                inputYearList.Clear();
                inputYearList = newyears;
                // -- group
                var newgroups = new List<Group>();
                foreach (string s in inputFileList.Select(x => x.group).Distinct())
                {
                    newgroups.Add(new Group(s));
                }
                inputGroupList.Clear();
                inputGroupList = newgroups;
                RefreshFileStatus();
            }
            catch
            {
                System.Windows.MessageBox.Show("不正確的路徑\r\n提示：不可以使用磁碟機之根目錄");
            }
        }
        /// <summary>
        /// 更動group或year選取狀態時，更新檔案選取清單
        /// </summary>
        void checkFileByCriteria()
        {
            foreach (File f in inputFileList)
            {
                f.selected = false;
            }
            var query = (
                            from p in inputFileList
                            where inputYearList.Where(x => x.selected == true)
                            .Select(x => x.str_year)
                            .Contains(p.year) &&
                            inputGroupList.Where(x => x.selected == true)
                            .Select(x => x.str_group)
                            .Contains(p.@group)
                            select p
                          )
                         .ToList();
            foreach (File f in query)
            {
                f.selected = true;
            }
        }
        /// <summary>
        /// 重繪list並且計算選取檔案數量
        /// </summary>
        void RefreshFileStatus()
        {
            inputFileList = new ObservableCollection<File>(inputFileList);
            inputYearList = inputYearList.ToList();
            inputGroupList = inputGroupList.ToList();
            FileStatus = inputFileList.Count(x => x.selected == true) + " / " + inputFileList.Count() + " file(s) selected.";
        }

        #region CodeBehind for files listview
        private void FilesCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            if (listview_files.ItemsSource == null) return;
            foreach (File item in listview_files.ItemsSource)
            {
                item.selected = true;
            }
            RefreshFileStatus();
        }
        private void FilesCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_files.ItemsSource == null) return;
            foreach (File item in listview_files.ItemsSource)
            {
                item.selected = false;
            }
            RefreshFileStatus();
        }
        private void FilesCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as File).selected = true;
            RefreshFileStatus();
        }
        private void FilesCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as File).selected = false;
            RefreshFileStatus();
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
            checkFileByCriteria();
            RefreshFileStatus();
        }
        private void yearsCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_years.ItemsSource == null) return;
            foreach (Year item in listview_years.ItemsSource)
            {
                item.selected = false;
            }
            checkFileByCriteria();
            RefreshFileStatus();
        }
        private void yearsCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Year).selected = true;
            checkFileByCriteria();
            RefreshFileStatus();
        }
        private void yearsCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Year).selected = false;
            checkFileByCriteria();
            RefreshFileStatus();
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
            checkFileByCriteria();
            RefreshFileStatus();
        }
        private void groupsCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (listview_groups.ItemsSource == null) return;
            foreach (Group item in listview_groups.ItemsSource)
            {
                item.selected = false;
            }
            checkFileByCriteria();
            RefreshFileStatus();
        }
        private void groupsCheckOne_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Group).selected = true;
            checkFileByCriteria();
            RefreshFileStatus();
        }
        private void groupsCheckOne_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkbox = sender as System.Windows.Controls.CheckBox;
            (checkbox.DataContext as Group).selected = false;
            checkFileByCriteria();
            RefreshFileStatus();
        }
        #endregion
        
        public static readonly DependencyProperty inputFileListProperty =
        DependencyProperty.Register(nameof(inputFileList), typeof(ObservableCollection<File>), typeof(FileListControl),
            new PropertyMetadata(new ObservableCollection<File>()));
        public ObservableCollection<File> inputFileList
        {
            get { return (ObservableCollection<File>)GetValue(inputFileListProperty); }
            set
            {
                SetValue(inputFileListProperty, value);
            }
        }

        public static readonly DependencyProperty inputYearListProperty =
        DependencyProperty.Register(nameof(inputYearList), typeof(List<Year>), typeof(FileListControl), new PropertyMetadata(new List<Year>()));
        public List<Year> inputYearList
        {
            get { return (List<Year>)GetValue(inputYearListProperty); }
            set
            {
                SetValue(inputYearListProperty, value);
            }
        }

        public static readonly DependencyProperty inputGroupListProperty =
        DependencyProperty.Register(nameof(inputGroupList), typeof(List<Group>), typeof(FileListControl), new PropertyMetadata(new List<Group>()));
        public List<Group> inputGroupList
        {
            get { return (List<Group>)GetValue(inputGroupListProperty); }
            set
            {
                SetValue(inputGroupListProperty, value);
            }
        }

        public static readonly DependencyProperty FileStatusProperty =
          DependencyProperty.Register(nameof(FileStatus), typeof(string), typeof(FileListControl));
        public string FileStatus
        {
            get { return (string)GetValue(FileStatusProperty); }
            set
            {
                SetValue(FileStatusProperty, value);
            }
        }

    }
}
