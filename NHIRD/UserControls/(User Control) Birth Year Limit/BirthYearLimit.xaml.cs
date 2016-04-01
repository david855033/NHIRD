using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NHIRD
{
    /// <summary>
    /// BrithYearLimit.xaml 的互動邏輯
    /// </summary>
    public partial class BirthYearLimit : UserControl
    {
        public BirthYearLimit()
        {
            InitializeComponent();
            if (Title == null) Title = "Birth Year Criteria:";
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(BirthYearLimit));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set {
                SetValue(TitleProperty, value
            ); }
        }

        public static readonly DependencyProperty isLowerLimitEnabledProperty =
        DependencyProperty.Register(nameof(isLowerLimitEnabled), typeof(bool), typeof(BirthYearLimit));
        public bool isLowerLimitEnabled
        {
            get { return (bool)GetValue(isLowerLimitEnabledProperty); }
            set { SetValue(isLowerLimitEnabledProperty, value); }
        }

        public static readonly DependencyProperty lowerLimitProperty =
            DependencyProperty.Register(nameof(lowerLimit), typeof(string), typeof(BirthYearLimit));
        public string lowerLimit
        {
            get { return (string)GetValue(lowerLimitProperty); }
            set { SetValue(lowerLimitProperty, value); }
        }

        public static readonly DependencyProperty isUpperLimitEnabledProperty =
        DependencyProperty.Register(nameof(isUpperLimitEnabled), typeof(bool), typeof(BirthYearLimit));
        public bool isUpperLimitEnabled
        {
            get { return (bool)GetValue(isUpperLimitEnabledProperty); }
            set {
                SetValue(isUpperLimitEnabledProperty, value);
            }
        }

        public static readonly DependencyProperty upperLimitProperty =
            DependencyProperty.Register(nameof(upperLimit), typeof(string), typeof(BirthYearLimit));
        public string upperLimit
        {
            get { return (string)GetValue(upperLimitProperty); }
            set { SetValue(upperLimitProperty, value); }
        }

        private void Textbox_FloatVerify_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var s = sender as System.Windows.Controls.TextBox;
            s.Text = s.Text.RegexFloat();
            s.Select(s.Text.Length, 0);
        }

    }
}
