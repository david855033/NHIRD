using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows;

namespace NHIRD
{
    public class GetCD_ViewModel : INotifyPropertyChanged
    {
        private readonly GetCD_Model Model_Instance = new GetCD_Model();
        private Window parentWindow;
        public GetCD_ViewModel(Window parent) { parentWindow = parent; }
        public string InputDir
        {
            get
            {
                return Model_Instance.str_inputDir;
            }
            set
            {
                Model_Instance.str_inputDir = value;
                string[] paths;
                // -- 初始化 file / year / group list
                try
                {
                    paths = Directory.EnumerateFiles(value, "*.DAT", SearchOption.AllDirectories).ToArray();
                    Array.Sort(paths);
                    // -- file
                    var newfiles = new ObservableCollection<file>();
                    foreach (string str_filepath in paths)
                    {
                        newfiles.Add(new file(str_filepath));
                    }
                    files.Clear();
                    files = newfiles;
                    // -- year
                    var newyears = new ObservableCollection<year>();
                    foreach (string s in files.Select(x => x.year).Distinct())
                    {
                        newyears.Add(new year(s));
                    }
                    years.Clear();
                    years = newyears;
                    // -- group
                    var newgroups = new ObservableCollection<group>();
                    foreach (string s in files.Select(x => x.group).Distinct())
                    {
                        newgroups.Add(new group(s));
                    }
                    groups.Clear();
                    groups = newgroups;
                }
                catch
                {
                    System.Windows.MessageBox.Show("不正確的路徑\r\n提示：不可以使用磁碟機之根目錄");
                }
                OnPropertyChanged("");
            }
        }


        public ObservableCollection<file> files
        {
            get
            {
                return Model_Instance.list_file;
            }
            set
            {
                Model_Instance.list_file = value;
                OnPropertyChanged("");
            }
        }
        public ObservableCollection<year> years
        {
            get
            {
                return Model_Instance.list_year;
            }
            set
            {
                Model_Instance.list_year = value;
                OnPropertyChanged("");
            }
        }
        public ObservableCollection<group> groups
        {
            get
            {
                return Model_Instance.list_group;
            }
            set
            {
                Model_Instance.list_group = value;
                OnPropertyChanged("");
            }
        }

        public string str_outputDir
        {
            get
            {
                return Model_Instance.str_outputDir;
            }
            set
            {
                Model_Instance.str_outputDir = value;
                OnPropertyChanged("");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


}
