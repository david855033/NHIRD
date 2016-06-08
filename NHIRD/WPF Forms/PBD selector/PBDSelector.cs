using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace NHIRD
{
    class PBDSelector
    {
        IEnumerable<File> PBDFiles;
        string outputFolder;
        string[] selectedFields, excludeFields;
        public void setPBDFiles(IEnumerable<File> PBDFiles)
        {
            this.PBDFiles = PBDFiles;
        }
        public void setOutputFolder(string s)
        {
            outputFolder = s;
        }
        public void setSelectedField(string s)
        {
            selectedFields = s.Split(',');
        }
        public void setExcludeField(string s)
        {
            excludeFields = s.Split(',');
        }

        public void Do()
        {
            foreach (File file in PBDFiles)
            {
                if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
                using (var sr = new StreamReader(file.path, Encoding.Default))
                using (var sw = new StreamWriter(outputFolder + "\\" + file.name, false, Encoding.Default))
                {
                    string title = sr.ReadLine();
                    sw.WriteLine(title);
                    string[] titles = title.Split('\t');

                    List<int> selectedIndex = new List<int>();
                    foreach (string s in selectedFields)
                    {
                        int i = Array.FindIndex(titles, x => x.IndexOf(s) >= 0);
                        if (i >= 0 && s != "") selectedIndex.Add(i);
                    }

                    List<int> excludeIndex = new List<int>();
                    foreach (string s in excludeFields)
                    {
                        int i = Array.FindIndex(titles, x => x.IndexOf(s) >= 0);
                        if (i >= 0 && s != "") excludeIndex.Add(i);
                    }

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] splitline = line.Split('\t');
                        bool pass = true;
                        foreach (int i in selectedIndex)
                        {
                            if (splitline[i] == "")
                            {
                                pass = false;
                                break;
                            }
                        }
                        if (pass)
                        {
                            foreach (int i in excludeIndex)
                            {
                                if (splitline[i] != "")
                                {
                                    pass = false;
                                    break;
                                }
                            }
                        }
                        if (pass)
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
            }
        }
    }
}
