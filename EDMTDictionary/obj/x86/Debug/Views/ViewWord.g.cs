﻿#pragma checksum "c:\users\reale\documents\visual studio 2015\Projects\EDMTDictionary\EDMTDictionary\Views\ViewWord.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "02DD6F44A9607190C8CC2AA7F6F32012"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EDMTDictionary.Views
{
    partial class ViewWord : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.txtType = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 2:
                {
                    this.txtDescription = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3:
                {
                    this.txtWord = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    this.btnSpeak = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    #line 42 "..\..\..\Views\ViewWord.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.btnSpeak).Tapped += this.btnSpeak_Tapped;
                    #line default
                }
                break;
            case 5:
                {
                    this.BottomAppBar = (global::Windows.UI.Xaml.Controls.CommandBar)(target);
                }
                break;
            case 6:
                {
                    this.btnFavorites = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 59 "..\..\..\Views\ViewWord.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.btnFavorites).Click += this.btnFavorites_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.btnTranslate = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 62 "..\..\..\Views\ViewWord.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.btnTranslate).Click += this.btnTranslate_Click;
                    #line default
                }
                break;
            case 8:
                {
                    this.btnSettings = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 66 "..\..\..\Views\ViewWord.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.btnSettings).Click += this.btnSettings_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
