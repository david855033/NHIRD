using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class StringListControl : UserControl
    {
        public StringListControl()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }


        public static readonly DependencyProperty CurrentListProperty =
        DependencyProperty.Register("CurrentList", typeof(ObservableCollection<string>), typeof(StringListControl),
            new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<string> CurrentList
        {
            get { return (ObservableCollection<string>)GetValue(CurrentListProperty); }
            set
            {
                SetValue(CurrentListProperty, value);
            }
        }

        public static readonly DependencyProperty IsCriteriaEnabledProperty =
        DependencyProperty.Register("IsCriteriaEnabled", typeof(bool), typeof(StringListControl));

        public bool IsCriteriaEnabled
        {
            get { return (bool)GetValue(IsCriteriaEnabledProperty); }
            set
            {
                SetValue(IsCriteriaEnabledProperty, value);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var stringtoAdd = Tx_input.Text.Replace("_", " ");
            if (stringtoAdd.Length > 0 && !CurrentList.Any(x => x == stringtoAdd))
            {
                CurrentList.Add(stringtoAdd);
                Tx_input.Text = "";
                refresh();
            }
        }
        private void ButtonEdt_Click(object sender, RoutedEventArgs e)
        {
            var stringtoAdd = Tx_input.Text.Replace("_", " ");
            if (stringtoAdd.Length > 0 && Lv_StringList.SelectedItem != null && !CurrentList.Any(x => x == stringtoAdd))
            {
                CurrentList.RemoveAt(Lv_StringList.SelectedIndex);
                CurrentList.Insert(Lv_StringList.SelectedIndex, stringtoAdd);
                refresh();
            }
        }


        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (Lv_StringList.SelectedItem != null)
            {
                CurrentList.RemoveAt(Lv_StringList.SelectedIndex);
                refresh();
            }
        }

        private void ButtonClr_Click(object sender, RoutedEventArgs e)
        {
            CurrentList.Clear();
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
                        if (stringtoAdd.Length > 0 && stringtoAdd.Length <= 12 && !CurrentList.Any(x => x == stringtoAdd))
                        {
                            CurrentList.Add(stringtoAdd);
                            refresh();
                        }
                    }
                }
            }
        }

        void refresh()
        {
            Lv_StringList.Items.Clear();
            foreach (var s in CurrentList)
            {
                Lv_StringList.Items.Add(s.Replace(" ", "_"));
            }
            IsCriteriaEnabled = CurrentList.Count > 0 ? true : false;
        }
    }
}
