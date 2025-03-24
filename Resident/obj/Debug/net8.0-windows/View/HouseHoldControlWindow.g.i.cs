﻿#pragma checksum "..\..\..\..\View\HouseHoldControlWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ED5A5BCB3E9EC00389F533C6A5CD19C7CBEA8F79"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Resident.Service;
using Resident.View;
using Resident.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace Resident.View {
    
    
    /// <summary>
    /// HouseHoldControlWindow
    /// </summary>
    public partial class HouseHoldControlWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 102 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox StreetTextBox;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox WardTextBox;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DistrictTextBox;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox CityTextBox;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox CountryTextBox;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMemberFullName;
        
        #line default
        #line hidden
        
        
        #line 148 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMemberIdentityNumber;
        
        #line default
        #line hidden
        
        
        #line 152 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMemberRelationship;
        
        #line default
        #line hidden
        
        
        #line 161 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgHouseholdMembers;
        
        #line default
        #line hidden
        
        
        #line 197 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSoHoKhau;
        
        #line default
        #line hidden
        
        
        #line 200 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtDiaChiHoHienTai;
        
        #line default
        #line hidden
        
        
        #line 210 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgMembersToSeparate;
        
        #line default
        #line hidden
        
        
        #line 229 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkSameAddress;
        
        #line default
        #line hidden
        
        
        #line 232 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridNewAddress;
        
        #line default
        #line hidden
        
        
        #line 247 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNewStreet;
        
        #line default
        #line hidden
        
        
        #line 250 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNewWard;
        
        #line default
        #line hidden
        
        
        #line 253 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNewDistrict;
        
        #line default
        #line hidden
        
        
        #line 256 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNewCity;
        
        #line default
        #line hidden
        
        
        #line 259 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNewCountry;
        
        #line default
        #line hidden
        
        
        #line 266 "..\..\..\..\View\HouseHoldControlWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtLyDoTachHo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Resident;V1.0.0.0;component/view/householdcontrolwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\HouseHoldControlWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.StreetTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.WardTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.DistrictTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.CityTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.CountryTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.txtMemberFullName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.txtMemberIdentityNumber = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.txtMemberRelationship = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            
            #line 157 "..\..\..\..\View\HouseHoldControlWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddMember_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 158 "..\..\..\..\View\HouseHoldControlWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteMember_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.dgHouseholdMembers = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 12:
            
            #line 174 "..\..\..\..\View\HouseHoldControlWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Register_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.txtSoHoKhau = ((System.Windows.Controls.TextBox)(target));
            return;
            case 14:
            this.txtDiaChiHoHienTai = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            this.dgMembersToSeparate = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 16:
            this.chkSameAddress = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 17:
            this.gridNewAddress = ((System.Windows.Controls.Grid)(target));
            return;
            case 18:
            this.txtNewStreet = ((System.Windows.Controls.TextBox)(target));
            return;
            case 19:
            this.txtNewWard = ((System.Windows.Controls.TextBox)(target));
            return;
            case 20:
            this.txtNewDistrict = ((System.Windows.Controls.TextBox)(target));
            return;
            case 21:
            this.txtNewCity = ((System.Windows.Controls.TextBox)(target));
            return;
            case 22:
            this.txtNewCountry = ((System.Windows.Controls.TextBox)(target));
            return;
            case 23:
            this.txtLyDoTachHo = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

