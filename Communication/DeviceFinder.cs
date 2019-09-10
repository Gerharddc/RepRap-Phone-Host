using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Networking.Proximity;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using RepRap_Phone_Host.ListItems;

namespace RepRap_Phone_Host.Communication
{
    /// <summary>
    /// This class contains the function needed to find all the paired bluetooth devices
    /// and add them to a listpicker.
    /// </summary>
    static class Devicefinder
    {
        /// <summary>
        /// Retrieve a list of paired bluetooth devices and add them to a listpicker. The appropriate
        /// handler for the listpicker item being changed is also attatched.
        /// </summary>
        /// <param name="deviceList">The lispicker that will contain the list od fevices</param>
        /// <param name="eventHandler">The handler that will respond to the listpicker selection being changed</param>
        static public async void getListOfBluetoothDevices(ListPicker deviceList)
        {
            try
            {
                List<DeviceItems> source = new List<DeviceItems>();

                PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";

                var findPeerResult = await PeerFinder.FindAllPeersAsync();

                foreach (PeerInformation peerInformation in findPeerResult)
                {
                    source.Add(new DeviceItems() { Name = peerInformation.DisplayName, hostName = peerInformation.HostName });
                }

                deviceList.ItemsSource = source;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
