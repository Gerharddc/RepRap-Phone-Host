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
using RepRap_Phone_Host.GlobalValues;
using RepRap_Phone_Host.Containers;
using RepRap_Phone_Host.ListItems;
using System.Threading;

namespace RepRap_Phone_Host
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GCodeViewer GCodeViewer;

        //This is the constructor for the pivot which gets called from the main constructor
        public void Construct_GCode_TextPivot()
        {
            //gcodetext_Filelist.SelectionChanged += gcodetext_Filelist_SelectionChanged;
            Values.GCode_IsBusyChangedEvent += GCodeText_GCode_IsBusyChanged;
            Values.GCode_ItemsChangedEvent += GCodeText_GCode_ItemsChangedEvent;
            Values.GCode_ListIndexChangedEvent += GCodeText_GCode_ListIndexChangedEvent;

            GCodeViewer = new GCodeViewer(gcodePanel, gcodeScroller, 200, 20, 200);

            gcodetext_Filelist.ManipulationCompleted += gcodetext_Filelist_ManipulationCompleted;
        }

        public void GCode_TextPivot_Load()
        {
            if (Values.GCode_IsBusy)
                return;

            Values.currentGCodeFileChangedEvent += gcodetext_currentGCodeFileChangedEvent;

            new Thread(new ThreadStart(() =>
            {
                Values.GCode_IsBusy = true;
                GCodeViewer.loadGCodeFromFile(Values.currentGCodeFile);
                Values.GCode_IsBusy = false;
            })).Start();
        }

        public void GCode_TextPivot_Unload()
        {
            Values.currentGCodeFileChangedEvent -= gcodetext_currentGCodeFileChangedEvent;
        }

        private void gcodetext_currentGCodeFileChangedEvent(object value)
        {
            new Thread(new ThreadStart(() =>
            {
                Values.GCode_IsBusy = true;
                GCodeViewer.loadGCodeFromFile(Values.currentGCodeFile);
                Values.GCode_IsBusy = false;
            })).Start();
        }

        void GCodeText_GCode_IsBusyChanged(object property_Value)
        {
            //We want to disable the selector when the current GCode file is busy
            this.Dispatcher.BeginInvoke(() => { gcodetext_Filelist.IsEnabled = !(bool)property_Value; });
        }

        private void GCodeText_GCode_ListIndexChangedEvent(object value)
        {
            gcodetext_Filelist.SelectedIndex = (int)value;
        }

        private void GCodeText_GCode_ItemsChangedEvent(object value)
        {
            gcodetext_Filelist.ItemsSource = (List<FileItems>)value;
            updateGCodeText(null);
        }

        Timer updateGCodeTextTimer;

        void gcodetext_Filelist_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            updateGCodeTextTimer = new Timer(new TimerCallback(this.updateGCodeText), null, 100, -1);
        }

        void updateGCodeText(object o)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (gcodetext_Filelist.Items.Count > 0)
                {
                    Values.currentGCodeFile = (gcodetext_Filelist.SelectedItem as FileItems).FilePath;
                    Values.GCode_ListIndex = gcodetext_Filelist.SelectedIndex;
                }
                else
                    Values.currentGCodeFile = "";
            });
        }
    }
}
