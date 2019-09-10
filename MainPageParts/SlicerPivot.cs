using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RepRap_Phone_Host.ListItems;
using RepRap_Phone_Host.Slicer;
using RepRap_Phone_Host.GlobalValues;
using RepRap_Phone_Host.FileSystem;
using System.Threading;

namespace RepRap_Phone_Host
{
    public partial class MainPage : PhoneApplicationPage
    {
        SlicerController slicerController;

        //This is the constructor for the pivot which gets called from the main constructor
        public void Construct_SlicerPivot()
        {
            FileFinder.findFilesAndAddToListpicker(slicer_Filelist, "Stl", ".stl");

            slicerController = new SlicerController(outputStackPanel, this);
            slicerController.SlicingFinishedEvent += slicerController_SlicingFinishedEvent;

            Values.stl_IsBusyChangedEvent += ApplicationSettings_stl_IsBusyChangedSlicer;
            Values.Stl_ListIndexChangedEvent += slicer_Stl_ListIndexChangedEvent;
            Values.Stl_ItemsChangedEvent += slicer_Stl_ItemsChangedEvent;

            slicer_Filelist.ManipulationCompleted += slicer_Filelist_ManipulationCompleted;
        }

        private void slicer_Stl_ItemsChangedEvent(object value)
        {
            slicer_Filelist.ItemsSource = (List<FileItems>)value;
            updateSlicerStl(null);
        }

        private void slicer_Stl_ListIndexChangedEvent(object value)
        {
            slicer_Filelist.SelectedIndex = (int)value;
        }

        void slicer_Filelist_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            Timer updateStlTimer = new Timer(new TimerCallback(this.updateSlicerStl), null, 100, -1);
        }

        bool firstTimeSlicer = false;

        void updateSlicerStl(object o)
        {
            if (!firstTimeSlicer && Values.startStlFile != "")
            {
                firstTimeSlicer = false;
                return;
            }

            Dispatcher.BeginInvoke(() =>
            {
                if (slicer_Filelist.Items.Count > 0)
                    Values.currentStlFile = (slicer_Filelist.SelectedItem as FileItems).FilePath;
                else
                    Values.currentStlFile = "";
            });
        }

        private void MainPage_StlFilesUpdatedEvent_Slicer()
        {
            FileFinder.findFilesAndAddToListpicker(slicer_Filelist, "Stl", ".stl");
            updateSlicerStl(null);
        }

        void slicerController_SlicingFinishedEvent()
        {
            //We need to use the dispatcher because this event is raised outside the ui thread
            Dispatcher.BeginInvoke(() =>
            {
                Values.GCode_Items = FileFinder.findFilesAndCreateList("GCode", ".gcode");

                //At the moment it is demanded that the slicing engine produce a GCode file with the same name as the stl file 
                Values.currentGCodeFile = Values.currentStlFile.Replace(".stl", ".gcode");
                Values.startGCodeFile = Values.currentGCodeFile;//Values.currentStlFile.Replace(".stl", ".GCode").Replace("Stl", "GCode");

                //Determine the index of the new gcode file and assign it
                for (int i = 0; i < Values.GCode_Items.Count; i++)
                {
                    if (Values.GCode_Items[i].FilePath == Values.currentGCodeFile)
                    {
                        Values.GCode_ListIndex = i;
                        break;
                    }
                }

                MainPivot.SelectedItem = GCodePivot;
            });
        }

        private void Slice_Button_Click_1(object sender, RoutedEventArgs e)
        {
            slicerController.startSlicer();
        }

        private void CancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            //If a file is busy printing then the gcode file will also be busy and should we not do anything here or else stlbusy will
            //be made false
            if (Values.GCode_IsBusy)
                return;

            slicerController.stopSlicer();
        }

        private void ApplicationSettings_stl_IsBusyChangedSlicer(object property_Value)
        {
            Dispatcher.BeginInvoke(() => 
            { 
                slicer_Filelist.IsEnabled = !(bool)property_Value;
                Slice_Button.IsEnabled = !(bool)property_Value;
            });
        }
    }
}
