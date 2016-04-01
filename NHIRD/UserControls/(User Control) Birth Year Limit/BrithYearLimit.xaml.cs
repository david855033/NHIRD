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
    public partial class BrithYearLimit : UserControl
    {
        public BrithYearLimit()
        {
            InitializeComponent();
            (this as FrameworkElement).DataContext = this;
            if (Title == null) Title = "Birth Year Criteria:";
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(BrithYearLimit));

        public string upperLimit
        {
            get { return (string)GetValue(upperLimitProperty); }
            set { SetValue(upperLimitProperty, value); }
        }
        public static readonly DependencyProperty upperLimitProperty =
            DependencyProperty.Register(nameof(upperLimit), typeof(string), typeof(BrithYearLimit));

        public string lowerLimit
        {
            get { return (string)GetValue(lowerLimitProperty); }
            set { SetValue(lowerLimitProperty, value); }
        }
        public static readonly DependencyProperty lowerLimitProperty =
            DependencyProperty.Register(nameof(lowerLimit), typeof(string), typeof(BrithYearLimit));

        public bool isUpperLimitEnabled
        {
            get { return (bool)GetValue(isUpperLimitEnableProperty); }
            set { SetValue(isUpperLimitEnableProperty, value); }
        }
        public static readonly DependencyProperty isUpperLimitEnableProperty =
        DependencyProperty.Register(nameof(isUpperLimitEnabled), typeof(bool), typeof(BrithYearLimit));

        public bool isLowerLimitEnabled
        {
            get { return (bool)GetValue(isLowerLimitEnableProperty); }
            set { SetValue(isLowerLimitEnableProperty, value); }
        }
        public static readonly DependencyProperty isLowerLimitEnableProperty =
        DependencyProperty.Register(nameof(isLowerLimitEnabled), typeof(bool), typeof(BrithYearLimit));

        private void Textbox_FloatVerify_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var s = sender as System.Windows.Controls.TextBox;
            s.Text = s.Text.RegexFloat();
            s.Select(s.Text.Length, 0);
        }

    }
}
