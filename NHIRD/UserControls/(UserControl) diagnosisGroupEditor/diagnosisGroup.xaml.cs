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

        private void includeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupSelector.SelectedIndex >= 0 && includeSelector.SelectedIndex >= 0)
            {
                renewLists();
            }
        }
        private string getSelectedIncludeName()
        {
            if (GroupSelector.SelectedIndex >= 0 && includeSelector.SelectedIndex >= 0)
            {
                return diagnosisGroupList[GroupSelector.SelectedIndex].getIncludeList()[includeSelector.SelectedIndex];
            }
            return "";
        }

        private void excludeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupSelector.SelectedIndex >= 0 && excludeSelector.SelectedIndex >= 0)
            {
                renewLists();
            }
        }
        private string getSelectedExcludeName()
        {
            if (GroupSelector.SelectedIndex >= 0 && excludeSelector.SelectedIndex >= 0)
            {
                return diagnosisGroupList[GroupSelector.SelectedIndex].getExcludeList()[excludeSelector.SelectedIndex];
            }
            return "";
        }


        private void addGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (addGroup(GroupNameTextBox.Text))
            {
                GroupNameTextBox.Text = "";
                includeNameTextBox.Text = "";
                excludeNameTextBox.Text = "";
                GroupNameTextBox.Focus();
                GroupSelector.SelectedIndex = GroupSelector.Items.Count - 1;
            }
        }
        private bool addGroup(string inputGroupName)
        {
            bool alreadyHasThisGroup = diagnosisGroupList.Any(x => x.name == inputGroupName);
            if (!alreadyHasThisGroup && inputGroupName != "")
            {
                diagnosisGroupList.Add(new DiagnosisGroup() { name = inputGroupName });
                renewLists();
                GroupSelector.SelectedIndex = GroupSelector.Items.Count - 1;
                return true;
            }
            return false;
        }

        private void editGroupButton_Click(object sender, RoutedEventArgs e)
        {
            editGroupButton(getSelectedGroupName());
        }
        private bool editGroupButton(string inputGroupName)
        {
            bool alreadyHasThisGroup = diagnosisGroupList.Any(x => x.name == inputGroupName);
            bool changeToSameName = diagnosisGroupList.Any(x => x.name == GroupNameTextBox.Text);
            if (alreadyHasThisGroup && !changeToSameName && GroupNameTextBox.Text != "")
            {
                diagnosisGroupList.Find(x => x.name == inputGroupName).name = GroupNameTextBox.Text;
                renewLists();
                return true;
            }
            return false;
        }

        private void deleteGroupButton_Click(object sender, RoutedEventArgs e)
        {
            int index = GroupSelector.SelectedIndex;
            if (deleteGroup(getSelectedGroupName()))
                GroupNameTextBox.Text = "";
            if (index > GroupSelector.Items.Count - 1)
                index--;
            if (GroupSelector.Items.Count > 0)
                GroupSelector.SelectedIndex = index;
        }
        private bool deleteGroup(string inputGroupName)
        {
            bool alreadyHasThisGroup = diagnosisGroupList.Any(x => x.name == inputGroupName);
            if (alreadyHasThisGroup)
            {
                diagnosisGroupList.RemoveAt(diagnosisGroupList.FindIndex(x => x.name == inputGroupName));
                GroupSelector.SelectedIndex = -1;
                renewLists();
                return true;
            }
            return false;
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
                includesInSelectedGroup  = new ObservableCollection<string>(diagnosisGroupList[index].getIncludeList());
                excludesInSelectedGroup  = new ObservableCollection<string>(diagnosisGroupList[index].getExcludeList());
            }
            if (includeSelector.SelectedItem == null)
            {
                includeNameTextBox.Text = "";
            }
            else
            {
                includeNameTextBox.Text = includeSelector.SelectedItem.ToString();
            }
            if (excludeSelector.SelectedItem == null)
            {
                excludeNameTextBox.Text = "";
            }
            else
            {
                excludeNameTextBox.Text = excludeSelector.SelectedItem.ToString();
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
