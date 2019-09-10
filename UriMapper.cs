using System;
using System.IO;
using System.Windows.Navigation;
using Windows.Phone.Storage.SharedAccess;
using RepRap_Phone_Host.GCode;
using RepRap_Phone_Host.Stl;
using RepRap_Phone_Host.GlobalValues;

namespace RepRap_Phone_Host
{
    /// <summary>
    /// This class contains the method that will route incoming uris to their designated
    /// pages
    /// </summary>
    class AssociationUriMapper : UriMapperBase
    {
        private string tempUri;

        public override Uri MapUri(Uri uri)
        {
            tempUri = uri.ToString();

            // File association launch
            if (tempUri.Contains("/FileTypeAssociation"))
            {
                // Get the file ID (after "fileToken=").
                int fileIDIndex = tempUri.IndexOf("fileToken=") + 10;
                string fileID = tempUri.Substring(fileIDIndex);

                // Get the file name.
                string incomingFileName =
                    SharedStorageAccessManager.GetSharedFileName(fileID);

                // Get the file extension.
                string incomingFileType = Path.GetExtension(incomingFileName).ToLower();

                // Map the .sdkTest1 and .sdkTest2 files to different pages.
                switch (incomingFileType)
                {
                    case ".gcode":
                        Values.startFileType = "gcode";
                        GCodeUriHandler.importGCodeFile(fileID);
                        return new Uri("/MainPage.xaml?fileToken=" + fileID, UriKind.Relative);
                    case ".stl":
                        Values.startFileType = "stl";
                        StlUriHandler.importStlFile(fileID);
                        return new Uri("/MainPage.xaml?fileToken=" + fileID, UriKind.Relative);
                    default:
                        return new Uri("/MainPage.xaml", UriKind.Relative);
                }
            }
            // Otherwise perform normal launch.
            return uri;
        }
    }
}
