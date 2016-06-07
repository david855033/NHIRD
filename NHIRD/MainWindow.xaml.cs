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
using System.IO;
using System.Windows.Forms;

namespace NHIRD
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Screen s1 = Screen.AllScreens.Last();
            System.Drawing.Rectangle r1 = s1.WorkingArea;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            this.Top = r1.Top+30;
            this.Left = r1.Left+30;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var GetCD_instance = new Window_GetCD(this);
            GetCD_instance.Show();
        }
        private void ButtonGetOrder_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var GetOrder_instance = new Window_GetOrder(this);
            GetOrder_instance.Show();
        }
        private void ButtonID_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var GetID_instance = new Window_ID(this);
            GetID_instance.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rawDataFormats= new List<RawDataFormat>();
            LoadRawDataFormat(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + @"\NHIRD Format.txt", rawDataFormats);
            
        }

        /// <summary>
        /// 儲存NHIRD_FORMATS，實作於選單視窗中
        /// </summary>
        public List<RawDataFormat> rawDataFormats;
        /// <summary>
        /// 於選單視窗載入時候觸發，載入NHIRD_FORMATS
        /// </summary>
        /// <param name="path"></param>
        /// <param name="RawDataFormats"></param>
        public void LoadRawDataFormat(string path, List<RawDataFormat> RawDataFormats)
        {
            using (var sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string title = sr.ReadLine();
                string[] titles = title.Split('\t');
                int Index_FileName = Array.IndexOf(titles, "英文檔案名");
                int Index_FileNameCH = Array.IndexOf(titles, "中文檔案名");
                int Index_start_year = Array.IndexOf(titles, "適用年分開始");
                int Index_end_year = Array.IndexOf(titles, "適用年份結束");
                int Index_ColumnName = Array.IndexOf(titles, "英文欄位");
                int Index_ColumnNameCH = Array.IndexOf(titles, "中文欄位");
                int Index_DataType = Array.IndexOf(titles, "資料型態");
                int Index_Lengths = Array.IndexOf(titles, "長度");
                int Index_Postion = Array.IndexOf(titles, "起始位置");
                int Index_Description = Array.IndexOf(titles, "資料描述");
                while (!sr.EndOfStream)
                {
                    string[] cols = sr.ReadLine().Split('\t');
                    var FormatToAdd = new RawDataFormat();
                    FormatToAdd.FileType = cols[Index_FileName];
                    if (FormatToAdd.FileType == "")
                        continue;
                    FormatToAdd.FileTypeCH = cols[Index_FileNameCH];
                    if (cols[Index_start_year] == "")
                    {
                        FormatToAdd.start_year = 0;
                    }
                    else
                    {
                        FormatToAdd.start_year = Convert.ToInt32(cols[Index_start_year]);
                    }
                    if (cols[Index_end_year] == "")
                    {
                        FormatToAdd.end_year = 0;
                    }
                    else
                    {
                        FormatToAdd.end_year = Convert.ToInt32(cols[Index_end_year]);
                    }
                    FormatToAdd.ColumnName = cols[Index_ColumnName];
                    FormatToAdd.ColumnNameCH = cols[Index_ColumnNameCH];
                    FormatToAdd.DataType = cols[Index_DataType];
                    FormatToAdd.Lengths = Convert.ToInt32(cols[Index_Lengths]);
                    FormatToAdd.Postion = Convert.ToInt32(cols[Index_Postion]) - 1;
                    FormatToAdd.Description = cols[Index_Description];
                    RawDataFormats.Add(FormatToAdd);
                }
            }
        }

        private void ButtonIDSplit_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var GetIDSplit_instance = new Window_IDHashSplitter(this);
            GetIDSplit_instance.Show();
        }
              

        private void ButtonGetGD_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var GetGDWindow_instance = new GetGDWindow(this);
            GetGDWindow_instance.Show();
        }

        private void ButtonJoinOrder_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var JoinOrderWindow_instance = new JoinOrderWindow(this);
            JoinOrderWindow_instance.Show();
        }

        private void PatientBasedData_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var PatientBasedDataWindow_instance = new PatientBasedDataWindow(this);
            PatientBasedDataWindow_instance.Show();
        }


        private void AgeSpecificIncidence_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();
            var AgeSpecificIncidenceWindow_instance = new AgeSpecificIncidenceWindow(this);
            AgeSpecificIncidenceWindow_instance.Show();
        }

        private void PBDSelector_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var PBDSelectorWindow_instance = new PBDSelectorWindow(this);
            PBDSelectorWindow_instance.Show();
        }
    }
}
