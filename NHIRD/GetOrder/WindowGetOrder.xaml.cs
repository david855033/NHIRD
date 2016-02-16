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

namespace NHIRD
{
    /// <summary>
    /// WindowGetOrder.xaml 的互動邏輯
    /// </summary>
    public partial class Window_GetOrder : Window
    {
        public MainWindow parentWindow;
        // -- constructor
        public Window_GetOrder(MainWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Left = parent.Left + parent.Width;
            this.Top = parent.Top;
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
    }
}
