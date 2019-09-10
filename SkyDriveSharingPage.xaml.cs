using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RepRap_Phone_Host.FileSystem;
using System.Threading;
using System.Diagnostics;

namespace RepRap_Phone_Host
{
    public partial class SkyDriveSharingPage : PhoneApplicationPage
    {
        Button loginBtn;
        Timer loginTimer;

        public SkyDriveSharingPage()
        {
            InitializeComponent();

            loginBtn = new Button() { Content = "Login" };
            loginBtn.Click += loginBtn_Click;
            //ContentPanel.Children.Add(loginBtn);

            /*SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "Trying to login...";
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;*/

            App.skydriveController.AuthenticationSuccessful += skydriveController_AuthenticationSuccessful;
            App.skydriveController.AuthenticationFailed += skydriveController_AuthenticationFailed;
            App.skydriveController.gotFolders += skydriveController_gotFolders;

            //App.skydriveController.Login();
            //loginBtn_Click(null, null);

            loginTimer = new Timer(new TimerCallback(this.timerCallback), null, 100, -1);
        }

        void timerCallback(object o)
        {
            Dispatcher.BeginInvoke(() => { loginBtn_Click(null, null); });
        }

        Dictionary<string, string> folderList;

        void skydriveController_gotFolders(Dictionary<string, string> folders)
        {
            ContentPanel.Children.Clear();

            SystemTray.ProgressIndicator.IsVisible = false;
            folderList = folders;

            if (folderList.Count < 1)
            {
                MessageBox.Show("No folders were found");
                return;
            }
            else
            {
                TextBlock textBlock = new TextBlock() { Text = "Choose folder to upload file(s) to" };
                ContentPanel.Children.Add(textBlock);
            }

            foreach (string folderName in folderList.Keys)
            {
                Button temp = new Button() { Content = folderName };
                temp.Click += temp_Click;
                ContentPanel.Children.Add(temp);
            }
        }

        void temp_Click(object sender, RoutedEventArgs e)
        {
            /*foreach (string fileName in App.filesToSave)
                Debug.WriteLine(fileName);*/
            App.skydriveController.uploadFilesToFolder(folderList[(sender as Button).Content.ToString()]);

        }

        void skydriveController_AuthenticationFailed()
        {
            SystemTray.ProgressIndicator.IsVisible = false;
            MessageBox.Show("Sorry but you could not be logged in, please try again");

            if (!ContentPanel.Children.Contains(loginBtn))
                ContentPanel.Children.Add(loginBtn);
        }

        void skydriveController_AuthenticationSuccessful()
        {
            //Remove the login button if it was present
            if (ContentPanel.Children.Contains(loginBtn))
                ContentPanel.Children.Remove(loginBtn);

            //We have now successfully loged in and can now request a list of folders
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "Finding folders...";
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;

            App.skydriveController.getFolderList();
        }

        void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            loginBtn.IsEnabled = false;

            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "Trying to login...";
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;

            App.skydriveController.Login();
        }
    }
}