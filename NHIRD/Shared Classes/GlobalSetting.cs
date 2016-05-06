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
        static public string get(string key)
        {
            try
            {
                return settings.Find(x => x.name == key).value;
            }
            catch
            {
                settings.Add(new setting()
                {
                    name = key,
                    value = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                });
                return settings.Find(x => x.name == key).value;
            }
        }
        static public void set(string key, string input)
        {
            try
            {
                settings.Find(x => x.name == key).value = input;
            }
            catch
            {
                settings.Add(new setting()
                {
                    name = key,
                    value = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                });
            }
            saveSetting();
        }
        // -- internal sasving logic
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\NHIRD_Settings.txt";
        static List<setting> settings = new List<setting>();

        static GlobalSetting()
        {
            loadSetting();
        }
        static void saveSetting()
        {
            using (var sw = new StreamWriter(path, false,  Encoding.Default))
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
