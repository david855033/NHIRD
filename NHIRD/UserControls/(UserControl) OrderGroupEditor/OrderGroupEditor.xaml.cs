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
    /// OrderGroupEditor.xaml 的互動邏輯
    /// </summary>
    public partial class OrderGroupEditor : UserControl, INotifyPropertyChanged
    {
        public List<OrderGroup> orderGroupList = new List<OrderGroup>();

        private ObservableCollection<string> _orderGroupNameList;
        public ObservableCollection<string> orderGroupNameList
        {
            get
            {
                return _orderGroupNameList;
            }
            set
            {
                _orderGroupNameList = value;
            }
        }
        private ObservableCollection<string> _ordersInSelectedGroup;
        public ObservableCollection<string> ordersInSelectedGroup
        {
            get { return _ordersInSelectedGroup; }
            set
            {
                _ordersInSelectedGroup = value;
            }
        }

        public OrderGroupEditor()
        {
            InitializeComponent();
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupSelector.SelectedIndex >= 0)
            {
                GroupNameTextBox.Text = getSelectedGroupName();
                renewLists();
            }
        }
        private string getSelectedGroupName()
        {
            if (GroupSelector.SelectedIndex >= 0)
            {
                return orderGroupList[GroupSelector.SelectedIndex].name;
            }
            return "";
        }

        private void orderSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (GroupSelector.SelectedIndex >= 0 && orderSelector.SelectedIndex >= 0)
            {
                OrderNameTextBox.Text = getSelectedOrderName();
            }
        }
        private string getSelectedOrderName()
        {
            if (GroupSelector.SelectedIndex >= 0 && orderSelector.SelectedIndex >= 0)
            {
                return orderGroupList[GroupSelector.SelectedIndex].getOrderList()[orderSelector.SelectedIndex];
            }
            return "";
        }

        private void addGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (addGroup(GroupNameTextBox.Text))
                GroupNameTextBox.Text = "";
        }
        private bool addGroup(string inputGroupName)
        {
            bool alreadyHasThisGroup = orderGroupList.Any(x => x.name == inputGroupName);
            if (!alreadyHasThisGroup && inputGroupName != "")
            {
                orderGroupList.Add(new OrderGroup() { name = inputGroupName });
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
            bool alreadyHasThisGroup = orderGroupList.Any(x => x.name == inputGroupName);
            bool changeToSameName = orderGroupList.Any(x => x.name == GroupNameTextBox.Text);
            if (alreadyHasThisGroup && !changeToSameName && GroupNameTextBox.Text != "")
            {
                orderGroupList.Find(x => x.name == inputGroupName).name = GroupNameTextBox.Text;
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
            bool alreadyHasThisGroup = orderGroupList.Any(x => x.name == inputGroupName);
            if (alreadyHasThisGroup)
            {
                orderGroupList.RemoveAt(orderGroupList.FindIndex(x => x.name == inputGroupName));
                GroupSelector.SelectedIndex = -1;
                renewLists();
                return true;
            }
            return false;
        }

        private void addOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            if (GroupSelector.SelectedIndex >= 0)
            {
                if (addOrder(OrderNameTextBox.Text))
                    OrderNameTextBox.Text = "";
            }
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }
        }
        private bool addOrder(string inputOrder)
        {
            bool alreadyHasThisOrder = orderGroupList[GroupSelector.SelectedIndex].getOrderList().Any(x => x == inputOrder);
            if (!alreadyHasThisOrder && inputOrder != "")
            {
                orderGroupList[GroupSelector.SelectedIndex].addOrder(inputOrder);
                renewLists();
                orderSelector.SelectedIndex = orderSelector.Items.Count - 1;
                return true;
            }
            return false;
        }

        private void editOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            if (GroupSelector.SelectedIndex >= 0)
            {
                editOrder(OrderNameTextBox.Text);
            }
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }
        }
        private bool editOrder(string inputOrder)
        {
            bool alreadyHasThisOrder = orderGroupList[GroupSelector.SelectedIndex].getOrderList().Any(x => x == getSelectedOrderName());
            if (alreadyHasThisOrder && OrderNameTextBox.Text != "")
            {
                orderGroupList[GroupSelector.SelectedIndex].editOrder(getSelectedOrderName(), OrderNameTextBox.Text);
                renewLists();
                return true;
            }
            return false;
        }

        private void delOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            int indexOrder = orderSelector.SelectedIndex;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            if (GroupSelector.SelectedIndex >= 0)
            {
                if (delOrder(getSelectedOrderName()))
                    GroupSelector.SelectedIndex = -1;
                if (indexOrder > orderSelector.Items.Count - 1)
                    indexOrder--;
                if (orderSelector.Items.Count > 0)
                    orderSelector.SelectedIndex = indexOrder;
            }
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }

        }
        private bool delOrder(string inputOrder)
        {

            bool alreadyHasThisOrder = orderGroupList[GroupSelector.SelectedIndex].getOrderList().Any(x => x == inputOrder);
            if (alreadyHasThisOrder)
            {
                orderGroupList[GroupSelector.SelectedIndex].deleteOrder(inputOrder);
                renewLists();
                return true;
            }
            return false;
        }

        private void loadOrderButtone_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            if (GroupSelector.SelectedItem != null)
            {
                index = GroupSelector.SelectedIndex;
            }
            loadOrder();
            if (index >= 0)
            {
                GroupSelector.SelectedIndex = index;
            }
        }
        private bool loadOrder()
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
                        if (stringtoAdd.Length > 0 && stringtoAdd.Length <= 12)
                        {
                            orderGroupList[GroupSelector.SelectedIndex].addOrder(stringtoAdd);
                        }
                    }
                }
                renewLists();
                return true;
            }
            return false;
        }


        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var newOrderGroupList = new List<OrderGroup>();
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
                        var splitline = sr.ReadLine().Split('\t');
                        if (splitline.Length > 1)
                        {
                            var groupToAdd = new OrderGroup() { name = splitline[0] };
                            for (int i = 1; i < splitline.Length; i++)
                                groupToAdd.addOrder(splitline[i]);
                            newOrderGroupList.Add(groupToAdd);
                        }
                    }
                }
                if (newOrderGroupList.Count > 0)
                {
                    orderGroupList = newOrderGroupList;
                    renewLists();
                }
            }
        }

        void renewLists()
        {
            orderGroupNameList = new ObservableCollection<string>();
            ordersInSelectedGroup = new ObservableCollection<string>();
            foreach (var orderGroup in orderGroupList)
            {
                orderGroupNameList.Add(orderGroup.name + $" [{orderGroup.getOrderCount()}]");
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
                OrderNameTextBox.Text = orderSelector.SelectedItem.ToString() ;
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

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.Filter = "Text File(*.txt)|*.txt";
            saveFileDialog1.Title = "Export Order Setting";
            saveFileDialog1.ShowDialog();
        }
    }
}
