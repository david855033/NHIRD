﻿#pragma checksum "..\..\..\..\WPF Forms\ID\WindowID.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3D88DD0A3D6E483B9BF66A4737919172"
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
    /// Window_ID
    /// </summary>
    public partial class Window_ID : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\..\..\WPF Forms\ID\WindowID.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal NHIRD.Window_ID thisWindow;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\WPF Forms\ID\WindowID.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal NHIRD.IOFolderSelector inputFolderSelect;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\WPF Forms\ID\WindowID.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal NHIRD.FileListControl fileListControl;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\WPF Forms\ID\WindowID.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal NHIRD.IOFolderSelector ouputFolderSelect;
        
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
            System.Uri resourceLocater = new System.Uri("/NHIRD;component/wpf%20forms/id/windowid.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\WPF Forms\ID\WindowID.xaml"
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
            this.thisWindow = ((NHIRD.Window_ID)(target));
            
            #line 9 "..\..\..\..\WPF Forms\ID\WindowID.xaml"
            this.thisWindow.Unloaded += new System.Windows.RoutedEventHandler(this.WindowID_Unloaded);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\..\WPF Forms\ID\WindowID.xaml"
            this.thisWindow.Loaded += new System.Windows.RoutedEventHandler(this.WindowID_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.inputFolderSelect = ((NHIRD.IOFolderSelector)(target));
            return;
            case 3:
            this.fileListControl = ((NHIRD.FileListControl)(target));
            return;
            case 4:
            this.ouputFolderSelect = ((NHIRD.IOFolderSelector)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

