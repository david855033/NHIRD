using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NHIRD
{
    public class GetCD_Model
    {
        public string str_inputDir { get; set; }
        public string str_outputDir { get; set; }
        public ObservableCollection<file> list_file = new ObservableCollection<file>();
        public ObservableCollection<year> list_year = new ObservableCollection<year>();
        public ObservableCollection<group> list_group = new ObservableCollection<group>();
    }

}
