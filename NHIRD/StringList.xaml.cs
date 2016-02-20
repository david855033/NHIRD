using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static readonly DependencyProperty ListProperty =
        DependencyProperty.Register("_currentList", typeof(ObservableCollection<string>),
        typeof(StringList), new PropertyMetadata(""));

        public List<string> currentList
        {
            get { return (List<string>)GetValue(ListProperty); }
            set { SetValue(ListProperty, value); }
        }

        public event Propert

        ObservableCollection<string> _currentList = new ObservableCollection<string>();
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var stringtoAdd = Tx_input.Text.Replace("_", " ");
            if (stringtoAdd.Length > 0 && !_currentList.Any(x => x == stringtoAdd))
            {
                _currentList.Add(stringtoAdd);
                Tx_input.Text = "";
                refresh();
            }
        }
        private void ButtonEdt_Click(object sender, RoutedEventArgs e)
        {
            var stringtoAdd = Tx_input.Text.Replace("_", " ");
            if (stringtoAdd.Length > 0 && Lv_StringList.SelectedItem != null && !_currentList.Any(x => x == stringtoAdd))
            {
                _currentList.RemoveAt(Lv_StringList.SelectedIndex);
                _currentList.Insert(Lv_StringList.SelectedIndex, stringtoAdd);
                refresh();
            }
        }


        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (Lv_StringList.SelectedItem != null)
            {
                _currentList.RemoveAt(Lv_StringList.SelectedIndex);
                refresh();
            }
        }

        private void ButtonClr_Click(object sender, RoutedEventArgs e)
        {
            _currentList.Clear();
            refresh();
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "txt files (*.txt)|*.txt|All files(*.*)|*.*";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var sr = new System.IO.StreamReader(dialog.FileName, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        var stringtoAdd = sr.ReadLine().Replace("_", " ");
                        if (stringtoAdd.Length > 0 && stringtoAdd.Length <= 12 && !_currentList.Any(x => x == stringtoAdd))
                        {
                            _currentList.Add(stringtoAdd);
                            refresh();
                        }
                    }
                }
            }
        }

        void refresh()
        {
            Lv_StringList.Items.Clear();
            foreach (var s in _currentList)
            {
                Lv_StringList.Items.Add(s.Replace(" ", "_"));
            }

            Cb_EnabaleCriteria.IsChecked = _currentList.Count > 0 ? true : false;
        }
    }
}
