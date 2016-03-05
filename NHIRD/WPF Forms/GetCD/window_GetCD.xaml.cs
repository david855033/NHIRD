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
        }
        /// <summary>
        /// 從global setting載入預設值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowGetCD_Loaded(object sender, RoutedEventArgs e)
        {
            inputFolderSelect.FolderPath =
                GlobalSetting.get("CD_InputDir");
            ouputFolderSelect.FolderPath =
                GlobalSetting.get("CD_OutputDir");
            IDCrieteria_FolderSelect.FolderPath =
                GlobalSetting.get("CD_IDCriteriaDir");
            OrderCrieteria_FolderSelect.FolderPath =
                GlobalSetting.get("CD_ActionCriteriaDir");
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
       
        private void ResetAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in Extentions.FindVisualChildren<System.Windows.Controls.CheckBox>(criteriaStackPanel))
            {
                obj.IsChecked = false;
            }
        }

        /// <summary>
        /// 限制Text Box只能輸入float for Age criteria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Textbox_FloatVerify_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var s = sender as System.Windows.Controls.TextBox;
            s.Text = s.Text.RegexFloat();
            s.Select(s.Text.Length, 0);
        }
        
        
        private void inputFolderSelect_OnFolderChanged()
        {
            ViewModel_Instance.InputDir = inputFolderSelect.FolderPath;
        }

        private void OuputFolderSelect_OnFolderChanged()
        {
            ViewModel_Instance.str_outputDir = ouputFolderSelect.FolderPath;
        }
    }
}
