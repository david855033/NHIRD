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
        // -- public setting Property
        static public string inputDir
        {
            get
            {
                try
                {
                    return settings.Find(x => x.name == "Input Dir").value;
                }
                catch
                {
                    settings.Add(new setting()
                    {
                        name = "Input Dir",
                        value = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    });
                    return settings.Find(x => x.name == "Input Dir").value;
                }
            }
            set
            {
                try
                {
                    settings.Find(x => x.name == "Input Dir").value = value;
                }
                catch
                {
                    settings.Add(new setting()
                    {
                        name = "Input Dir",
                        value = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    });
                }
                saveSetting();
            }
        }
        static public string outputDir
        {
            get
            {
                try
                {
                    return settings.Find(x => x.name == "Output Dir").value;
                }
                catch
                {
                    settings.Add(new setting()
                    {
                        name = "Output Dir",
                        value = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    });
                    return settings.Find(x => x.name == "Output Dir").value;
                }
            }
            set
            {
                try
                {
                    settings.Find(x => x.name == "Output Dir").value = value;
                }
                catch
                {
                    settings.Add(new setting()
                    {
                        name = "Output Dir",
                        value = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    });
                }
                saveSetting();
            }
        }

        // -- internal saving logic
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\NHIRD_Settings.txt";
        static List<setting> settings = new List<setting>();

        static GlobalSetting()
        {
            loadSetting();
        }

        static void saveSetting()
        {
            using (var sw = new StreamWriter(path, false))
            {
                foreach (var set in settings)
                {
                    sw.WriteLine(set.name + "=" + set.value);
                }
            }
        }

        static void loadSetting()
        {
            if (System.IO.File.Exists(path))
            {
                using (var sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split('=');
                        settings.Add(new setting { name = line[0], value = line[1] });
                    }
                }
            }
        }



    }

    class setting
    {
        public string name;
        public string value;
    }
}
