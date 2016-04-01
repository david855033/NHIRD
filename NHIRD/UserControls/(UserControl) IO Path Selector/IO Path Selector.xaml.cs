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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NHIRD
{
    /// <summary>
    /// IO_Path_Selector.xaml 的互動邏輯
    /// </summary>
    public partial class IOFolderSelector : UserControl
    {
        public IOFolderSelector()
        {
            InitializeComponent();
            if (Title == null) Title = "Default Title";
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(IOFolderSelector));

       
        public string FolderPath
        {
            get { return (string)GetValue(FolderPathProperty); }
            set { SetValue(FolderPathProperty, value);
                this.OnFolderChanged(); }
        }
        public static readonly DependencyProperty FolderPathProperty =
            DependencyProperty.Register(nameof(FolderPath), typeof(string), typeof(IOFolderSelector),
                new FrameworkPropertyMetadata(string.Empty,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public delegate void FolderChangeEventHandler();
        public event FolderChangeEventHandler OnFolderChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var FolderSelector = new System.Windows.Forms.FolderBrowserDialog();
            if (FolderSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPath = FolderSelector.SelectedPath;
            }
        }

        private void txtbox_InputDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            FolderPath = (sender as TextBox).Text;
        }
    }
}
