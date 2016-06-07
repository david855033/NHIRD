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
        List<File> PBDFiles;
        string outputFolder;
        string selectedField;
        public void setPBDFiles(List<File> PBDFiles)
        { this.PBDFiles = PBDFiles; }
        public void setOutputFolder(string s)
        {
            outputFolder = s;
        }
        public void setselectedField(string s)
        {
            selectedField = s;
        }

        public void Do()
        {
            foreach (File file in PBDFiles)
            {
                using (var sr = new StreamReader(file.path, Encoding.Default))
                {
                    string[] titles = sr.ReadLine().Split('\t');
                    //**********
                }
            }
        }
    }
}
