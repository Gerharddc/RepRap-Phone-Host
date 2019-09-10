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

namespace RepRap_Phone_Host.Stl
{
    static class StlUriHandler
    {
        static public async void importStlFile(string fileToken)
        {
            //Check if the stl folder exists
            //if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Stl"))
              //  Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Stl");

            try
            {
                //Create the target folder
                StorageFolder targetFolder = ApplicationData.Current.LocalFolder;

                // Get the full file name of the route (.GPX file) from the file association.
                string incomingRouteFilename = SharedStorageAccessManager.GetSharedFileName(fileToken);

                //Change the extension to lower case ".gcode" as to avoid confusion
                incomingRouteFilename = Path.ChangeExtension(incomingRouteFilename, ".stl");

                // Copy the route (.GPX file) to the Routes folder.
                IStorageFile routeFile = await SharedStorageAccessManager.CopySharedFileAsync(targetFolder, incomingRouteFilename, NameCollisionOption.ReplaceExisting, fileToken);

                //Set the starting Stl file
                Values.currentStlFile = incomingRouteFilename;

                //Tell the mainpage to open on the stl pivot
                Values.startFileType = "stl";

                Values.Stl_Items = FileFinder.findFilesAndCreateList("Stl", ".stl");

                //Determine the index of the gcode file and assign it
                for (int i = 0; i < Values.Stl_Items.Count; i++)
                {
                    if (Values.Stl_Items[i].FilePath == Values.currentStlFile)
                    {
                        Values.Stl_ListIndex = i;
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
