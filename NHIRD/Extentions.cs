﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace NHIRD
{
    static class Extentions
    {
        /// <summary>
        /// 取出檔名(含附檔名)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathToFileName(this string path)
        {
            var pathsplit = path.Split('\\');
            return pathsplit.Last();
        }
        public static string Round(this double input, int deci = 0)
        {
            try
            {
                return Math.Round(Convert.ToDouble(input), deci).ToString();
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 去除"-"後，轉換成日期(若為年月，日自動為1)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime StringToDate(this string input)
        {
            try
            {
                input = input.TrimEnd(); input = input.Replace("-", "");
                if (input.Length == 8)
                {
                    return Convert.ToDateTime(input.Substring(0, 4) + "-" + input.Substring(4, 2) + "-" + input.Substring(6, 2));
                }
                else if (input.Length == 6)
                {
                    return Convert.ToDateTime(input.Substring(0, 4) + "-" + input.Substring(4, 2) + "-01");
                }
                return DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static string RegexFloat(this string text)
        {
            Regex regex = new Regex(@"[0-9]+\.?[0-9]*"); //regex that matches disallowed text
            return regex.Match(text).ToString();
        }
        public static string DateToString(this DateTime input)
        {
            return input.ToString("yyyy-MM-dd");
        }
        
        //找到元件的所有層級的children
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }


}
