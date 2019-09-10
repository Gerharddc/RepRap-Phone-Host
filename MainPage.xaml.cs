using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Reflection;
using System.IO;
using Microsoft.Phone.Tasks;
using System.Threading;
using System.Diagnostics;
using RepRap_Phone_Host.Resources;
using RepRap_Phone_Host.GlobalValues;
using RepRap_Phone_Host.Stl;
using RepRap_Phone_Host.GCode;
using RepRap_Phone_Host.FileSystem;
using RepRap_Phone_Host.RenderUtils;
using MonoGame.Framework.WindowsPhone;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host
{
    public partial class MainPage : PhoneApplicationPage
    {
        RenderingController renderingController;
        object drawingSurfaceUpdateHandler;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //Copy test files to isolatedstorage if needed
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("Copiedinitialfiles"))
            {
                IsolatedStorageSettings.ApplicationSettings.Add("Copiedinitialfiles", true);

                var resource = App.GetResourceStream(new Uri("/RepRap Phone Host;component/test.stl", UriKind.Relative));

                var file = IsolatedStorageFile.GetUserStoreForApplication();

                var stream = new IsolatedStorageFileStream("test.stl", System.IO.FileMode.CreateNew, file);

                byte[] bytes = new byte[resource.Stream.Length];

                resource.Stream.Read(bytes, 0, bytes.Length);

                stream.Write(bytes, 0, bytes.Length);
                
                resource.Stream.Dispose();
                stream.Dispose();

                var resource2 = App.GetResourceStream(new Uri("/RepRap Phone Host;component/test.gcode", UriKind.Relative));

                var stream2 = new IsolatedStorageFileStream("test.gcode", System.IO.FileMode.CreateNew, file);

                bytes = new byte[resource2.Stream.Length];

                resource2.Stream.Read(bytes, 0, bytes.Length);

                stream2.Write(bytes, 0, bytes.Length);

                resource2.Stream.Dispose();
                stream2.Dispose();
            }

            renderingController = XamlGame<RenderingController>.Create("", this);

            //All the pivot item constructrs should be called here   
            Construct_StlPivot();
            Construct_SlicerPivot();
            Construct_GCodePivot();
            Construct_GCode_TextPivot();
            Construct_ConnectionPivot();//This has to be called before the controlpivot because the cocontroller gets initialised first
            Construct_ControlPivot();
            Construct_SettingsPivot();
            Construct_FilesPivot();

            BackKeyPress += PhoneApplicationPage_BackKeyPress;

            Values.GCode_Items = FileFinder.findFilesAndCreateList("GCode", ".gcode");
            Values.Stl_Items = FileFinder.findFilesAndCreateList("Stl", ".stl");

            if (Values.startFileType == "gcode")
                MainPivot.SelectedItem = GCodePivot;
        }

        /// <summary>
        /// This method initialises the stl and gcode file once the timer calls it
        /// </summary>
        /// <param name="o"></param>
        void initFiles(object o)
        {
            Dispatcher.BeginInvoke(() =>
            {
                //We need to move to the gcode pivot if needeb before starting to initialise the stl
                if (Values.startFileType == "gcode")
                    MainPivot.SelectedItem = GCodePivot;
            });
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //When a XamlGame element is loaded it disables the normal operration of that back button and forces us to use its "Exit" method to close the application
            //if (renderingController != null)
            //    renderingController.Exit();
            //else
                App.Current.Terminate();
        }

        private void unloadingPivotItem(object sender, PivotItemEventArgs e)
        {
            string methodName = e.Item.Name + "_Unload";
            MethodInfo method = typeof(MainPage).GetMethod(methodName);
            if (method != null)
                method.Invoke(this, null);
        }

        private void loadingPivotItem(object sender, PivotItemEventArgs e)
        {
            string methodName = e.Item.Name + "_Load";
            MethodInfo method = typeof(MainPage).GetMethod(methodName);
            if (method != null)
                method.Invoke(this, null);
        }
    }
}