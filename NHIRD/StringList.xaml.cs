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
    /// StringList.xaml 的互動邏輯
    /// </summary>
    public partial class StringList : UserControl
    {
        public StringList()
        {
            InitializeComponent();
        }
        public List<string> currentList = new List<string>();
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var stringtoAdd = Tx_input.Text.Replace("_", " ");
            if (stringtoAdd.Length > 0 && !currentList.Any(x => x == stringtoAdd))
            {
                currentList.Add(stringtoAdd);
                Tx_input.Text = "";
                refresh();
            }
        }
        private void ButtonEdt_Click(object sender, RoutedEventArgs e)
        {
            var stringtoAdd = Tx_input.Text.Replace("_", " ");
            if (stringtoAdd.Length > 0 && Lv_StringList.SelectedItem != null && !currentList.Any(x => x == stringtoAdd))
            {
                currentList.RemoveAt(Lv_StringList.SelectedIndex);
                currentList.Insert(Lv_StringList.SelectedIndex, stringtoAdd);
                refresh();
            }
        }


        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (Lv_StringList.SelectedItem != null)
            {
                currentList.RemoveAt(Lv_StringList.SelectedIndex);
                refresh();
            }
        }

        private void ButtonClr_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        void refresh()
        {
            Lv_StringList.Items.Clear();
            foreach (var s in currentList)
            {
                Lv_StringList.Items.Add(s.Replace(" ", "_"));
            }
        }
    }
}
