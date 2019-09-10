using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;

namespace RepRap_Phone_Host.ListItems
{
    /// <summary>
    /// This class is a container for elements in a listpicker that represent bluetooth devices
    /// </summary>
    class DeviceItems
    {
        public string Name
        {
            get;
            set;
        }

        public HostName hostName
        {
            get;
            set;
        }
    }
}
