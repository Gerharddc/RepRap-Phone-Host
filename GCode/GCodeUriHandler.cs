using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RepRap_Phone_Host.GlobalValues;
using System.IO;
using Windows.Phone.Storage.SharedAccess;
using Windows.Storage;
using System.IO.IsolatedStorage;
using RepRap_Phone_Host.FileSystem;
using System.Windows;

namespace RepRap_Phone_Host.GCode
{
    static class GCodeUriHandler
    {
        static public async void importGCodeFile(string fileToken)
        {
            //Check if the stl folder exists
            //if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\GCode"))
              //  Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\GCode");

            try
            {
                //Create the target folder
                StorageFolder targetFolder = ApplicationData.Current.LocalFolder;

                // Get the full file name of the route (.GPX file) from the file association.
                string incomingRouteFilename = SharedStorageAccessManager.GetSharedFileName(fileToken);

                //Change the extension to lower case ".gcode" as to avoid confusion
                incomingRouteFilename = Path.ChangeExtension(incomingRouteFilename, ".gcode");

                // Copy the route (.GPX file) to the Routes folder.
                IStorageFile routeFile = await SharedStorageAccessManager.CopySharedFileAsync(targetFolder, incomingRouteFilename, NameCollisionOption.ReplaceExisting, fileToken);

                //Set the starting GCode file
                Values.currentGCodeFile = incomingRouteFilename;

                //Tell the mainpage to open on the gcode pivot
                Values.startFileType = "gcode";

                Values.GCode_Items = FileFinder.findFilesAndCreateList("GCode", ".gcode");

                //Determine the index of the gcode file and assign it
                for (int i = 0; i < Values.GCode_Items.Count; i++)
                {
                    if (Values.GCode_Items[i].FilePath == Values.currentGCodeFile)
                    {
                        Values.GCode_ListIndex = i;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                /*App.RootFrame.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(e.ToString());
                });*/
            }
        }
    }
}
