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
    /// EXT_FolderSelector.xaml 的互動邏輯
    /// </summary>
    public partial class EXT_FolderSelector : UserControl
    {
        public EXT_FolderSelector()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
            if (Title == null) Title = "Defult Title";
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

        // -- Property for FolderPath
        public static readonly DependencyProperty FolderPathProperty =
        DependencyProperty.Register(nameof(FolderPath), typeof(string), typeof(EXT_FolderSelector));
        public string FolderPath
        {
            get { return (string)GetValue(FolderPathProperty); }
            set
            {
                SetValue(FolderPathProperty, value);
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
         DependencyProperty.Register(nameof(FileList), typeof(List<File>), typeof(EXT_FolderSelector),
             new PropertyMetadata(new List<File>()));
        public List<File> FileList
        {
            get { return (List<File>)GetValue(FileListProperty); }
            set
            {
                SetValue(FileListProperty, value);
            }
        }

        void renewFileList()
        {
            try
            {
                var paths = new List<string>();
                paths.AddRange(Directory.EnumerateFiles(FolderPath, "*CD*.EXT", SearchOption.AllDirectories).ToArray());
                paths.AddRange(Directory.EnumerateFiles(FolderPath, "*DD*.EXT", SearchOption.AllDirectories).ToArray());
                var newfiles = new List<File>();
                foreach (string str_filepath in paths)
                {
                    newfiles.Add(new File(str_filepath));
                }
                FileList.Clear();
                FileList = newfiles;
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
    }
}
