using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace NHIRD
{
    public class file
    {
        public string name { get; set; }
        public string path { get; set; }
        double size { get; set; }
        public string sizeMB { get { return (size / 1024 / 1024).Round() + "MB"; } }
        public string year { get; set; }
        public string group { get; set; }
        public bool? selected{ get; set; }
        public file(string filepath)
        {
            path = filepath;
            name = path.PathToFileName();
            try
            {
                size = new FileInfo(path).Length;
            }
            catch
            {
                size = 0;
            }

            selected = true;
            Regex GroupRegex = new Regex(@"R\d{1,3}", RegexOptions.IgnoreCase);
            Match GroupMatch = GroupRegex.Match(name);
            try
            {
                group = GroupMatch.Groups[0].ToString();
                if (group == "") group = "NA";
            }
            catch
            {
                group = "NA";
            }
            Regex YearRegex = new Regex(@"\d{4}");
            Match YearMatch = YearRegex.Match(name);
            try
            {
                year = YearMatch.Groups[0].ToString();
                if (year == "") year = "NA";
            }
            catch
            {
                year = "NA";
            }

        }
    }

    public class year
    {
        public string str_year { get; set; }
        public bool? selected { get; set; }
        public year(string yr)
        {
            str_year = yr;
            selected = true;
        }
    }

    public class group
    {
        public string str_group { get; set; }
        public bool? selected { get; set; }
        public group(string grp)
        {
            str_group = grp;
            selected = true;
        }
    }

    public class RawDataFormat
    {
        public string FileName { get; set; }
        public string FileNameCH { get; set; }
        public int start_year { get; set; }
        public int end_year { get; set; }
        public string ColumnName{ get; set; }
        public string ColumnNameCH { get; set; }
        public string DataType { get; set; }
        public int Postion { get; set; }
        public int Lengths { get; set; }
        public string Description { get; set; }
        public RawDataFormat()
        {
            start_year = 0;
            end_year = 0;
            Postion = 0;
            Lengths = 0;
        }
    }
}
