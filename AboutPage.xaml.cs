using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using Microsoft.Phone.Tasks;

namespace RepRap_Phone_Host
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webbrowser = new WebBrowserTask();
            webbrowser.URL = "http://www.youtube.com/watch?v=Sfd0ggeTKBw";
            webbrowser.Show();
        }

        private void Hyperlink_Click_2(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webbrowser = new WebBrowserTask();
            webbrowser.URL = "http://www.bananna3d.com";
            webbrowser.Show();
        }
    }
}