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

namespace NHIRD
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class GetCD : Window
    {
        private Window _parentWindow;
        public GetCD(Window parent)
        {
            InitializeComponent();
            _parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
        }

        private void WindowGetCD_Unloaded(object sender, RoutedEventArgs e)
        {
            _parentWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var FolderSelector = new FolderBrowserDialog();
            if (FolderSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.MessageBox.Show(FolderSelector.SelectedPath);
            }
        }
    }
}
