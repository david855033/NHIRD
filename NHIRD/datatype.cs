using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace NHIRD
{
    /// <summary>
    /// 定義讀入檔案清單之欄位(Main menu時載入，後面查詢)
    /// </summary>
    public class File
    {
        public string name { get; set; }
        public string FileType { get; set; }
        public string path { get; set; }
        double size { get; set; }
        public string sizeMB { get { return (size / 1024 / 1024).Round() + "MB"; } }
        /// <summary>
        /// 西元年(字串)
        /// </summary>
        public string year { get; set; }
        /// <summary>
        /// 民國年(數值)
        /// </summary>
        public int MKyear {get { return Convert.ToInt32(year) - 1911; } }
        public string group { get; set; }
        public bool? selected{ get; set; }
        public File(string filepath)
        {
            path = filepath;
            name = path.PathToFileName();
            string[] AvailableFileType = new string[] { "CD", "DD","GO","OO","DO" };
            FileType = AvailableFileType.First(x => name.IndexOf(x) >= 0);
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
    /// <summary>
    /// 定義年份群組欄位
    /// </summary>
    public class Year
    {
        public string str_year { get; set; }
        public bool? selected { get; set; }
        public Year(string yr)
        {
            str_year = yr;
            selected = true;
        }
    }
    /// <summary>
    /// 定義R group欄位
    /// </summary>
    public class Group
    {
        public string str_group { get; set; }
        public bool? selected { get; set; }
        public Group(string grp)
        {
            str_group = grp;
            selected = true;
        }
    }

    /// <summary>
    /// 定義儲存NHIRD_FORMATS之資料欄位
    /// </summary>
    public class RawDataFormat
    {
        public string FileType { get; set; }
        public string FileTypeCH { get; set; }
        public int start_year { get; set; }
        public int end_year { get; set; }
        public string ColumnName { get; set; }
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
