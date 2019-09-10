using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepRap_Phone_Host.ListItems
{
    /// <summary>
    /// This class is a container for elements in a listpicker that represent files
    /// </summary>
    public class FileItems
    {
        public string Name
        {
            get;
            set;
        }

        public string FilePath
        {
            get;
            set;
        }
    }
}
