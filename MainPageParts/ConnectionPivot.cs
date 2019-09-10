using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RepRap_Phone_Host.Communication;
using RepRap_Phone_Host.ListItems;
using RepRap_Phone_Host.GlobalValues;

namespace RepRap_Phone_Host
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ComController comController;

        //This is the constructor for the pivot item which gets called from the main constructor
        private void Construct_ConnectionPivot()
        {
            comController = new ComController(this, device_Listpicker, comlog_Stackpanel, temperature_Textblock);

            comController.ConnectedEvent += comController_ConnectedEvent;
            comController.DisconnectedEvent += comController_DisconnectedEvent;
            comController.ConnectionFailedEvent += comController_ConnectionFailedEvent;

            repetier_CheckBox.Click += repetier_CheckBox_Click;
            repetier_CheckBox.IsChecked = Settings.repetierProtocol;
        }

        private void repetier_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Settings.repetierProtocol = (sender as CheckBox).IsChecked.Value;
        }

        void comController_ConnectionFailedEvent()
        {
            connect_Button.IsEnabled = true;
            connect_Button.Content = "Connect";
        }

        void comController_DisconnectedEvent()
        {
            connect_Button.IsEnabled = true;
            connect_Button.Content = "Connect";
        }

        void comController_ConnectedEvent()
        {
            connect_Button.IsEnabled = true;
            connect_Button.Content = "Disconnect";
        }

        //This gets called when the pivot item gets focus
        private void ConnectionPivot_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: load memory intensive ui items here
        }

        //This gets called when the pivot item loses focus
        private void ConnectionPivot_Unloaded(object sender, RoutedEventArgs e)
        {
            //TDOD: unload memory intensive ui items here
        }

        private void connect_Button_Click(object sender, RoutedEventArgs e)
        {
            if (connect_Button.Content.ToString() == "Connect")
            {
                if (device_Listpicker.SelectedItem == null)
                {
                    MessageBox.Show("There are currently no paired devices to connect to");
                    return;
                }

                if (comController.connectToDevice((device_Listpicker.SelectedItem as DeviceItems).hostName))
                {
                    connect_Button.Content = "Connecting";
                    connect_Button.IsEnabled = false;
                }
                else
                    connect_Button.Content = "Connect";
            }
            else//Disconnect
            {
                connect_Button.Content = "Disconnecting";
                connect_Button.IsEnabled = false;

                comController.disconnectFromDevice();
            }
        }
    }
}
