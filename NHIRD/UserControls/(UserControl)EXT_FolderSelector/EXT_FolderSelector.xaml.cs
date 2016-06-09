using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// EXT_FolderSelector.xaml 的互動邏輯
    /// </summary>
    public partial class EXT_FolderSelector : UserControl
    {
        public EXT_FolderSelector()
        {
            InitializeComponent();
            if (Title == null) Title = "Default Title";
        }

        // -- Property for Title
        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(EXT_FolderSelector));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        // -- Property for IsCriteriaChecked
        public static readonly DependencyProperty IsCriteriaCheckedProperty =
        DependencyProperty.Register(nameof(IsCriteriaChecked), typeof(bool), typeof(EXT_FolderSelector));
        public bool IsCriteriaChecked
        {
            get { return (bool)GetValue(IsCriteriaCheckedProperty); }
            set
            {
                SetValue(IsCriteriaCheckedProperty, value);
            }
        }

        // -- Property for IsCriteriaChecked
        public static readonly DependencyProperty IsEXTOProperty =
        DependencyProperty.Register(nameof(IsEXTO), typeof(bool), typeof(EXT_FolderSelector));
        public bool IsEXTO
        {
            get { return (bool)GetValue(IsEXTOProperty); }
            set
            {
                SetValue(IsEXTOProperty, value);
            }
        }

        // -- Property for FolderPath
        public static readonly DependencyProperty FolderPathProperty =
        DependencyProperty.Register(nameof(FolderPath), typeof(string), typeof(EXT_FolderSelector));
        public string FolderPath
        {
            get { return (string)GetValue(FolderPathProperty); }
            set
            {
                SetValue(FolderPathProperty, value);
                renewFileList();
            }
        }

        // -- Property for Message
        public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register(nameof(Message), typeof(string), typeof(EXT_FolderSelector));
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set
            {
                SetValue(MessageProperty, value);
            }
        }

        // -- Property for FileList
        public static readonly DependencyProperty FileListProperty =
         DependencyProperty.Register(nameof(FileList), typeof(ObservableCollection<File>), typeof(EXT_FolderSelector),
             new PropertyMetadata(new ObservableCollection<File>()));
        public ObservableCollection<File> FileList
        {
            get { return (ObservableCollection<File>)GetValue(FileListProperty); }
            set
            {
                SetValue(FileListProperty, value);
            }
        }

        public String FileType { get; set; }

        public String SubFileName { get; set; }

        void renewFileList()
        {
            try
            {
                var paths = new List<string>();
                List<string> subFileNames = new List<string>();
                if (SubFileName != null)
                {
                    foreach (var s in SubFileName.Split(','))
                    {
                        subFileNames.Add("*." + s);
                    }
                }
                else
                {
                    if (IsEXTO)
                    {
                        subFileNames.Add("*.EXTO");
                        subFileNames.Add("*.EXT");
                    }
                }
                var FileTypes = new List<string>();
                if (FileType != null)
                {
                    foreach (var f in FileType.Split(','))
                    {
                        FileTypes.Add(f);
                    }
                }

                foreach (var F in FileTypes)
                {
                    paths.AddRange(Directory.EnumerateFiles(FolderPath, "*" + F + "*.*", SearchOption.AllDirectories).ToArray());
                }
                foreach (var subname in subFileNames)
                {
                    paths.AddRange(Directory.EnumerateFiles(FolderPath, "*" + subname, SearchOption.AllDirectories).ToArray());
                }
                var newfiles = new ObservableCollection<File>();
                paths = new List<string>(paths.Distinct());
                foreach (string str_filepath in paths)
                {
                    if (str_filepath.Split('\\').Last().IndexOf("All") >= 0) continue;
                    newfiles.Add(new File(str_filepath));
                }
                FileList.Clear();
                FileList = newfiles;
                var groupCount = (from q in FileList group q by q.@group into g select g).Count();
                Message = "Total " + FileList.Count() + " files was loaded. Group count: " + groupCount;
            }
            catch
            {
                Message = "Invalid path";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var FolderSelector = new System.Windows.Forms.FolderBrowserDialog();
            if (FolderSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPath = FolderSelector.SelectedPath;
                IsCriteriaChecked = true;
            }
        }

        private void Tx_ID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists((sender as TextBox).Text))
            {
                FolderPath = (sender as TextBox).Text;
            }
            else
            {
                Message = "Invalid path";
            }
        }
    }
}
