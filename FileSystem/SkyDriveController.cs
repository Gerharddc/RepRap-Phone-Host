using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Live;
using Microsoft.Live.Controls;
using System.IO;
using Microsoft.Phone.Shell;
using System.Windows;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.FileSystem
{
    public class SkyDriveController
    {
        private static LiveConnectSession _session;
        private static readonly string[] scopes = new[] { "wl.signin", "wl.offline_access", "wl.skydrive_update" };

        public async void Login()
        {
            await Authenticate();
        }

        public delegate void AuthenticationEventHandler();
        public event AuthenticationEventHandler AuthenticationSuccessful;
        public event AuthenticationEventHandler AuthenticationFailed;
        private async Task Authenticate()
        {
            try
            {
                var liveIdClient = new LiveAuthClient("000000004810306D");
                var initResult = await liveIdClient.InitializeAsync(scopes);

                _session = initResult.Session;

                if (null == _session)
                {
                    LiveLoginResult result = await liveIdClient.LoginAsync(scopes);

                    if (result.Status == LiveConnectSessionStatus.Connected)
                    {
                        _session = result.Session;
                        if (AuthenticationSuccessful != null)
                            AuthenticationSuccessful();
                    }
                    else
                    {
                        _session = null;
                        if (AuthenticationFailed != null)
                            AuthenticationFailed();
                        //MessageBox.Show("Unable to authenticate with Windows Live.", "Login failed :(", MessageBoxButton.OK);
                    }
                }

                if (AuthenticationSuccessful != null)
                    AuthenticationSuccessful();
            }
            catch (Exception)
            {
            }
        }

        public delegate void GotFolderHandler(Dictionary<string, string> folders);
        public event GotFolderHandler gotFolders;

        public async void getFolderList()
        {
            if (_session == null)
            {
                //MessageBox.Show("You must be logged in first");
                return;
            }
            else
            {
                try
                {
                    Dictionary<string, string> folderList = new Dictionary<string, string>();

                    var client = new LiveConnectClient(_session);
                    var result = await client.GetAsync("me/skydrive/files?filter=folders");
                    IDictionary<string, object> folderData = result.Result;
                    List<object> folders = (List<object>)folderData["data"];

                    foreach (object item in folders)
                    {
                        IDictionary<string, object> folder = (IDictionary<string, object>)item;
                        //Debug.WriteLine(folder["name"].ToString());
                        folderList.Add(folder["name"].ToString(), folder["id"].ToString());
                    }

                    if (gotFolders != null)
                        gotFolders(folderList);
                }
                catch (Exception)
                {
                    if (gotFolders != null)
                        gotFolders(new Dictionary<string, string>());
                }
            }
        }

        public async void uploadFilesToFolder(string folderId)
        {
            if (_session == null)
                return;

            try
            {
                var client = new LiveConnectClient(_session);

                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.Text = "Uploading...";
                SystemTray.ProgressIndicator.IsIndeterminate = true;
                SystemTray.ProgressIndicator.IsVisible = true;

                var isf = IsolatedStorageFile.GetUserStoreForApplication();

                foreach (string filePath in App.filesToSave)
                {
                    string[] segments = filePath.Split('\\');
                    var fileName = segments[segments.Length - 1];

                    IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(filePath, FileMode.OpenOrCreate, isf);
                    //FileStream fileStream = new FileStream(filePath, FileMode.Open);
                    await client.UploadAsync(folderId, fileName, fileStream, OverwriteOption.Overwrite);
                    fileStream.Dispose();
                }

                SystemTray.ProgressIndicator.IsVisible = false;

                MessageBox.Show("Finsihed uploading: " + App.filesToSave.Count + " files");
            }
            catch (Exception) { }
        }
    }
}
