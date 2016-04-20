using System;
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
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class DiagnosisGroupEditor : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty diagnosisGroupListProperty =
        DependencyProperty.Register("diagnosisGroupList", typeof(List<DiagnosisGroup>), typeof(DiagnosisGroupEditor),
           new PropertyMetadata(new List<DiagnosisGroup>()));
        public List<DiagnosisGroup> diagnosisGroupList
        {
            get { return (List<DiagnosisGroup>)GetValue(diagnosisGroupListProperty); }
            set
            {
                SetValue(diagnosisGroupListProperty, value);
            }
        }

        private ObservableCollection<string> _diagnosisGroupNameList;
        public ObservableCollection<string> diagnosisGroupNameList
        {
            get
            {
                return _diagnosisGroupNameList;
            }
            set
            {
                _diagnosisGroupNameList = value;
            }
        }
        private ObservableCollection<string> _includesInSelectedGroup;
        public ObservableCollection<string> includesInSelectedGroup
        {
            get { return _includesInSelectedGroup; }
            set
            {
                _includesInSelectedGroup = value;
            }
        }
        private ObservableCollection<string> _excludesInSelectedGroup;
        public ObservableCollection<string> excludesInSelectedGroup
        {
            get { return _excludesInSelectedGroup; }
            set
            {
                _excludesInSelectedGroup = value;
            }
        }

        public DiagnosisGroupEditor()
        {
            InitializeComponent();
        }





        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupSelector.SelectedIndex >= 0)
            {
                renewLists();
            }
        }
        private string getSelectedGroupName()
        {
            if (GroupSelector.SelectedIndex >= 0)
            {
                return diagnosisGroupList[GroupSelector.SelectedIndex].name;
            }
            return "";
        }
        void renewLists()
        {
            diagnosisGroupNameList = new ObservableCollection<string>();
            includesInSelectedGroup = new ObservableCollection<string>();
            excludesInSelectedGroup = new ObservableCollection<string>();
            foreach (var diagnosisGroup in diagnosisGroupList)
            {
                diagnosisGroupNameList.Add(diagnosisGroup.name + $" [{diagnosisGroup.getIncludeCount()}/{diagnosisGroup.getExcludeCount()}]");
            }
            if (GroupSelector.SelectedItem != null)
            {
                int index = GroupSelector.SelectedIndex;
                ordersInSelectedGroup = new ObservableCollection<string>(orderGroupList[index].getOrderList());
            }
            if (orderSelector.SelectedItem == null)
            {
                OrderNameTextBox.Text = "";
            }
            else
            {
                OrderNameTextBox.Text = orderSelector.SelectedItem.ToString();
            }
            OnPropertyChanged();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
