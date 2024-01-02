using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using DeepGamingSocial.ChatCore;
using DeepGamingSocial.Net;


#nullable enable
namespace DeepGamingSocial
{
    public partial class chatbox 
    {


        public chatbox()
        {
            this.InitializeComponent();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.chat.Text = "";
        }
    }
}