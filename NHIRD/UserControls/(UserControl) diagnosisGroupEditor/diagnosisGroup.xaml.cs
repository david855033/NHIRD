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


        private void addIncludeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            bool addSuccess = false;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            if (GroupSelector.SelectedIndex >= 0)
            {
                addSuccess = addInclude(includeNameTextBox.Text);
            }
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
                GroupNameTextBox.Text = "";
            }
            if (addSuccess)
            {
                includeNameTextBox.Focus();
                includeNameTextBox.Text = "";
            }
        }
        private bool addInclude(string inputinclude)
        {
            bool alreadyHasThisinclude = diagnosisGroupList[GroupSelector.SelectedIndex].getIncludeList().Any(x => x == inputinclude);
            if (!alreadyHasThisinclude && inputinclude != "")
            {
                diagnosisGroupList[GroupSelector.SelectedIndex].addInclude(inputinclude);
                renewLists();
                includeSelector.SelectedIndex = includeSelector.Items.Count - 1;
                return true;
            }
            return false;
        }

        private void editIncludeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            bool successEdit = false;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            if (GroupSelector.SelectedIndex >= 0)
            {
                successEdit = editInclude(includeNameTextBox.Text);
            }
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }
            if (successEdit)
                includeNameTextBox.Text = "";
        }
        private bool editInclude(string inputinclude)
        {
            bool alreadyHasThisinclude = diagnosisGroupList[GroupSelector.SelectedIndex].getIncludeList().Any(x => x == getSelectedIncludeName());
            if (alreadyHasThisinclude && includeNameTextBox.Text != "")
            {
                diagnosisGroupList[GroupSelector.SelectedIndex].editInclude(getSelectedIncludeName(), includeNameTextBox.Text);
                renewLists();
                return true;
            }
            return false;
        }

        private void delIncludeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            int indexinclude = includeSelector.SelectedIndex;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            if (GroupSelector.SelectedIndex >= 0)
            {
                if (delInclude(getSelectedIncludeName()))
                    GroupSelector.SelectedIndex = -1;
                if (indexinclude > includeSelector.Items.Count - 1)
                    indexinclude--;
                if (includeSelector.Items.Count > 0)
                    includeSelector.SelectedIndex = indexinclude;
            }
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }

        }
        private bool delInclude(string inputinclude)
        {

            bool alreadyHasThisinclude = diagnosisGroupList[GroupSelector.SelectedIndex].getIncludeList().Any(x => x == inputinclude);
            if (alreadyHasThisinclude)
            {
                diagnosisGroupList[GroupSelector.SelectedIndex].deleteInclude(inputinclude);
                renewLists();
                return true;
            }
            return false;
        }

        private void loadIncludeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            loadInclude();
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }
        }
        private bool loadInclude()
        {

            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.InitialDirectory = GlobalSetting.get("Diaganosis_LoadIncludeButton");
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
                        if (stringtoAdd.Length > 0 && stringtoAdd.Length <= 12)
                        {
                            diagnosisGroupList[GroupSelector.SelectedIndex].addInclude(stringtoAdd);
                        }
                    }
                    GlobalSetting.set("Diaganosis_LoadIncludeButton",
                                         dialog.FileName.Substring(0, dialog.FileName.LastIndexOf('\\')));
                }
                renewLists();
                return true;
            }
            return false;
        }


        private void addExcludeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            bool addSuccess = false;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            if (GroupSelector.SelectedIndex >= 0)
            {
                addSuccess = addExclude(excludeNameTextBox.Text);
            }
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
                GroupNameTextBox.Text = "";
            }
            if (addSuccess)
            {
                excludeNameTextBox.Focus();
                excludeNameTextBox.Text = "";
            }
        }
        private bool addExclude(string inputexclude)
        {
            bool alreadyHasThisexclude = diagnosisGroupList[GroupSelector.SelectedIndex].getExcludeList().Any(x => x == inputexclude);
            if (!alreadyHasThisexclude && inputexclude != "")
            {
                diagnosisGroupList[GroupSelector.SelectedIndex].addExclude(inputexclude);
                renewLists();
                excludeSelector.SelectedIndex = excludeSelector.Items.Count - 1;
                return true;
            }
            return false;
        }

        private void editExcludeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            bool successEdit = false;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            if (GroupSelector.SelectedIndex >= 0)
            {
                successEdit = editExclude(excludeNameTextBox.Text);
            }
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }
            if (successEdit)
                excludeNameTextBox.Text = "";
        }
        private bool editExclude(string inputexclude)
        {
            bool alreadyHasThisexclude = diagnosisGroupList[GroupSelector.SelectedIndex].getExcludeList().Any(x => x == getSelectedExcludeName());
            if (alreadyHasThisexclude && excludeNameTextBox.Text != "")
            {
                diagnosisGroupList[GroupSelector.SelectedIndex].editExclude(getSelectedExcludeName(), excludeNameTextBox.Text);
                renewLists();
                return true;
            }
            return false;
        }

        private void delExcludeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            int indexexclude = excludeSelector.SelectedIndex;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            if (GroupSelector.SelectedIndex >= 0)
            {
                if (delExclude(getSelectedExcludeName()))
                    GroupSelector.SelectedIndex = -1;
                if (indexexclude > excludeSelector.Items.Count - 1)
                    indexexclude--;
                if (excludeSelector.Items.Count > 0)
                    excludeSelector.SelectedIndex = indexexclude;
            }
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }

        }
        private bool delExclude(string inputexclude)
        {

            bool alreadyHasThisexclude = diagnosisGroupList[GroupSelector.SelectedIndex].getExcludeList().Any(x => x == inputexclude);
            if (alreadyHasThisexclude)
            {
                diagnosisGroupList[GroupSelector.SelectedIndex].deleteExclude(inputexclude);
                renewLists();
                return true;
            }
            return false;
        }

        private void loadExcludeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            loadExclude();
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }
        }
        private bool loadExclude()
        {

            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.InitialDirectory = GlobalSetting.get("Diaganosis_LoadExcludeButton");
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
                        if (stringtoAdd.Length > 0 && stringtoAdd.Length <= 12)
                        {
                            diagnosisGroupList[GroupSelector.SelectedIndex].addExclude(stringtoAdd);
                        }
                    }
                    GlobalSetting.set("Diaganosis_LoadExcludeButton",
                                         dialog.FileName.Substring(0, dialog.FileName.LastIndexOf('\\')));
                }
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
