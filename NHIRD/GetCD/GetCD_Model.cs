using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;

namespace NHIRD
{
    public class GetCD_Model
    {
        Window parentWindow;
        public GetCD_Model(Window parent)
        {
            parentWindow = parent;
        }

        public string str_inputDir { get; set; }
        public string str_outputDir { get; set; }
        public ObservableCollection<file> list_file = new ObservableCollection<file>();
        public ObservableCollection<year> list_year = new ObservableCollection<year>();
        public ObservableCollection<group> list_group = new ObservableCollection<group>();
        public string str_filestatus { get; set; }
        public void checkFileByCriteria()
        {
            foreach (file f in list_file)
            {
                f.selected = false;
            }
            var query =  (
                            from p in list_file
                             where list_year.Where(x => x.selected == true)
                             .Select(x => x.str_year)
                             .Contains(p.year) &&
                             list_group.Where(x => x.selected == true)
                             .Select(x => x.str_group)
                             .Contains(p.@group)
                            select p
                          )
                         .ToList();
            foreach (file f in query)
            {
                f.selected = true;
            }

        }
        
    }

}
