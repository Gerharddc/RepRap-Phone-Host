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
using RepRap_Phone_Host.GlobalValues;

namespace RepRap_Phone_Host
{
    public partial class MainPage : PhoneApplicationPage
    {
        IApplicationBar filesBar = new ApplicationBar();

        //This is the constructor for the pivot which gets called from the main constructor
        private void Construct_FilesPivot()
        {
            buildFilesApplicationBar();

            FileLister.listFilesOnPivot(stlFiles_StackPanel, gcodeFiles_StackPanel);

            Values.GCode_ItemsChangedEvent += filesChanged;
            Values.Stl_ItemsChangedEvent += filesChanged;

            App.skydriveController = new FileSystem.SkyDriveController();

            ApplicationBar = filesBar;
            ApplicationBar.IsVisible = false;
        }

        //This gets called when the control pivot gets focus
        public void FilesPivot_Load()
        {
            ApplicationBar = filesBar;
            ApplicationBar.IsVisible = true;
        }

        //This gets called when the control pivot loses focus
        public void FilesPivot_Unload()
        {
            ApplicationBar.IsVisible = false;
        }

        private void filesChanged(object o)
        {
            FileLister.listFilesOnPivot(stlFiles_StackPanel, gcodeFiles_StackPanel);
        }

        ApplicationBarIconButton deleteBarButton;

        private void buildFilesApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            //ApplicationBar = new ApplicationBar();
            filesBar.Mode = ApplicationBarMode.Default;

            // Create a new button and set the text value to the localized string from AppResources.
            deleteBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/delete.png", UriKind.Relative));
            deleteBarButton.Text = "Delete";
            filesBar.Buttons.Add(deleteBarButton);
            deleteBarButton.Click += deleteButtonClicked;

            ApplicationBarIconButton appBarButton2 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/share.png", UriKind.Relative));
            appBarButton2.Text = "Share";
            filesBar.Buttons.Add(appBarButton2);
            appBarButton2.Click += appBarButton2_Click;

            // Create a new menu item
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem("Select all");
            filesBar.MenuItems.Add(appBarMenuItem);
            appBarMenuItem.Click += selectAll_Click;

            ApplicationBarMenuItem appBarMenuItem2 = new ApplicationBarMenuItem("Unselect all");
            filesBar.MenuItems.Add(appBarMenuItem2);
            appBarMenuItem2.Click += unSelectAll_Click;
        }

        private void signInSkydrive(object sender, EventArgs e)
        {
            //skydriveController.loginSkyDrive();
        }

        void appBarButton2_Click(object sender, EventArgs e)
        {
            FileLister.determineFilesToShare(stlFiles_StackPanel, gcodeFiles_StackPanel);
            this.NavigationService.Navigate(new Uri("/SkyDriveSharingPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void unSelectAll_Click(object sender, EventArgs e)
        {
            FileLister.unSelectAll(stlFiles_StackPanel, gcodeFiles_StackPanel);
        }

        void selectAll_Click(object sender, EventArgs e)
        {
            FileLister.selectAll(stlFiles_StackPanel, gcodeFiles_StackPanel);
        }

        private void deleteButtonClicked(object sender, EventArgs e)
        {
            bool allStlSelected = true;
            foreach (UIElement element in stlFiles_StackPanel.Children)
            {
                var checkBox = element as CheckBox;
                if (checkBox != null && checkBox.IsChecked == false)
                    allStlSelected = false;
            }

            bool allGCodeSelected = true;
            foreach (UIElement element in gcodeFiles_StackPanel.Children)
            {
                var checkBox = element as CheckBox;
                if (checkBox != null && checkBox.IsChecked == false)
                    allGCodeSelected = false;
            }

            if (allStlSelected || allGCodeSelected)
            {
                MessageBox.Show("Sorry, but have to have at least 1 STL and G-Code file left");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the selected files?", "", MessageBoxButton.OKCancel);

            if (result != MessageBoxResult.OK)
                return;

            deleteBarButton.IsEnabled = false;

            //We run this in an independant thread because it may take a while
            new Thread(new ThreadStart(() =>
            {
                //Thread.Sleep(1000);

                FileLister.deleteCheckedFiles(stlFiles_StackPanel, gcodeFiles_StackPanel, this.Dispatcher);

                this.Dispatcher.BeginInvoke(() =>
                {
                    Values.GCode_Items = FileFinder.findFilesAndCreateList("GCode", ".gcode");
                    Values.Stl_Items = FileFinder.findFilesAndCreateList("Stl", ".stl");

                    deleteBarButton.IsEnabled = true;
                });
            })).Start();
        }
    }
}
