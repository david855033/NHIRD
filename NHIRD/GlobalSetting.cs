using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace NHIRD
{
    static class GlobalSetting
    {
        static List<setting> settings = new List<setting>();
        static public string inputDir
        {
            get { return settings.Find(x => x.name == "Input Dir").value; }
            set {
                settings.Find(x => x.name == "Input Dir").value = value;
                saveSetting();
            }
        }
        static void saveSetting()
        {
            using (var sr = new StreamReader(Environment.SpecialFolder.MyDocuments+@"\NHIRD_Settings.txt"))
            {
                string[] line = sr.ReadLine().Split('=');
                settings.Add(new setting { name = line[0], value = line[1] });
            }
        }
        static void loadSetting()
        {
        }
    }

    class setting
    {
        public string name;
        public string value;
    }
}
