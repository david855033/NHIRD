﻿#pragma checksum "..\..\..\GetOrder\window_GetOrder.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "25F079DFE48A607613A0EE1FE49E5B03"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using NHIRD;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace NHIRD {
    
    
    /// <summary>
    /// Window_GetOrder
    /// </summary>
    public partial class Window_GetOrder : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 17 "..\..\..\GetOrder\window_GetOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtbox_InputDir;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\GetOrder\window_GetOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview_files;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\GetOrder\window_GetOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkWspSelectAll;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\GetOrder\window_GetOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview_years;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\GetOrder\window_GetOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview_groups;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\GetOrder\window_GetOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextBlock_FileStatus;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\GetOrder\window_GetOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal NHIRD.StringListControl OrderCriteria;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\GetOrder\window_GetOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtbox_OutputDir;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\GetOrder\window_GetOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextBlockMessage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/NHIRD;component/getorder/window_getorder.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\GetOrder\window_GetOrder.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((NHIRD.Window_GetOrder)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.WindowGetOrder_Unloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtbox_InputDir = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            
            #line 18 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SelectInputDir_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.listview_files = ((System.Windows.Controls.ListView)(target));
            return;
            case 5:
            this.chkWspSelectAll = ((System.Windows.Controls.CheckBox)(target));
            
            #line 39 "..\..\..\GetOrder\window_GetOrder.xaml"
            this.chkWspSelectAll.Checked += new System.Windows.RoutedEventHandler(this.FilesCheckAll_Checked);
            
            #line default
            #line hidden
            
            #line 40 "..\..\..\GetOrder\window_GetOrder.xaml"
            this.chkWspSelectAll.Unchecked += new System.Windows.RoutedEventHandler(this.FilesCheckAll_Unchecked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.listview_years = ((System.Windows.Controls.ListView)(target));
            return;
            case 8:
            
            #line 65 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.yearsCheckAll_Checked);
            
            #line default
            #line hidden
            
            #line 65 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.yearsCheckAll_Unchecked);
            
            #line default
            #line hidden
            return;
            case 10:
            this.listview_groups = ((System.Windows.Controls.ListView)(target));
            return;
            case 11:
            
            #line 88 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.groupsCheckAll_Checked);
            
            #line default
            #line hidden
            
            #line 88 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.groupsCheckAll_Unchecked);
            
            #line default
            #line hidden
            return;
            case 13:
            this.TextBlock_FileStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 14:
            this.OrderCriteria = ((NHIRD.StringListControl)(target));
            return;
            case 15:
            
            #line 111 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SelectOutputDir_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.txtbox_OutputDir = ((System.Windows.Controls.TextBox)(target));
            return;
            case 17:
            this.TextBlockMessage = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 44 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.FilesCheckOne_Checked);
            
            #line default
            #line hidden
            
            #line 44 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.FilesCheckOne_UnChecked);
            
            #line default
            #line hidden
            break;
            case 9:
            
            #line 71 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.yearsCheckOne_Checked);
            
            #line default
            #line hidden
            
            #line 71 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.yearsCheckOne_UnChecked);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 93 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.groupsCheckOne_Checked);
            
            #line default
            #line hidden
            
            #line 93 "..\..\..\GetOrder\window_GetOrder.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.groupsCheckOne_UnChecked);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

