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
using RepRap_Phone_Host.GlobalValues;
using RepRap_Phone_Host.FileSystem;
using System.Threading;

namespace RepRap_Phone_Host
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string dir = "X";

        //This is the constructor for the pivot which gets called from the main constructor
        private void Construct_ControlPivot()
        {
            //FileFinder.findFilesAndAddToListpicker(gcodeprint_Filelist, "GCode", ".GCode");
            Values.GCode_ListIndexChangedEvent += Control_GCode_ListIndexChangedEvent;
            Values.GCode_ItemsChangedEvent += Control_GCode_ItemsChangedEvent;

            comController.FileReadingStartedEvent += comController_FileReadingStartedEvent;
            comController.FileReadingStoppedEvent += comController_FileReadingStoppedEvent;

            Values.GCode_IsBusyChangedEvent += ApplicationSettings_GCode_IsBusyChanged;
            Values.progress_PercentageDoneChangedEvent += ApplicationSettings_progress_PercentageDoneChanged;
            Values.progress_SecondsLeftChangedEvent += ApplicationSettings_progress_SecondsLeftChanged;
            Settings.printingTemperatureChangedEvent += ApplicationSettings_printingTemperatureControlChanged;
            Values.isHeatingChangedEvent += ApplicationSettings_isHeatingChanged;

            //GCodeFilesUpdatedEvent += Controller_GCodeFilesUpdatedEvent;

            gcodeprint_Filelist.ManipulationCompleted += gcodeprint_Filelist_ManipulationCompleted;
            //temperature_Textbox.TextChanged += temperature_Textbox_TextChanged;
        }

        private void Control_GCode_ItemsChangedEvent(object value)
        {
            gcodeprint_Filelist.ItemsSource = (List<FileItems>)value;
            updateGCodePrint(null);
        }

        private void Control_GCode_ListIndexChangedEvent(object value)
        {
            gcodeprint_Filelist.SelectedIndex = (int)value;
        }

        //This gets called when the control pivot gets focus
        public void ControlPivot_Load()
        {
            Values.currentGCodeFileChangedEvent += Control_currentGCodeFileChangedEvent;

            new Thread(new ThreadStart(() =>
            {
                Values.progress_SecondsLeft = new GCode.GCodeProgressEstimator().estimateTotalTimeForFile(Values.currentGCodeFile);
            })).Start();
        }

        //This gets called when the control pivot loses focus
        public void ContolPivot_Unload()
        {
            Values.currentGCodeFileChangedEvent -= Control_currentGCodeFileChangedEvent;
        }

        private void Control_currentGCodeFileChangedEvent(object value)
        {
            new Thread(new ThreadStart(() =>
            {
                Values.progress_SecondsLeft = new GCode.GCodeProgressEstimator().estimateTotalTimeForFile(Values.currentGCodeFile);
            })).Start();
        }

        void ApplicationSettings_isHeatingChanged(object property_Value)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (heat_Checkbox.IsChecked == (bool)property_Value)
                    return;

                heat_Checkbox.IsChecked = (bool)property_Value;
            });
        }

        private void ApplicationSettings_printingTemperatureControlChanged(object property_Value)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (temperature_Textbox.Text == property_Value.ToString())
                    return;

                temperature_Textbox.Text = property_Value.ToString();
            });
        }

        void temperature_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            short result;
            Dispatcher.BeginInvoke(() =>
            {
                if (short.TryParse(temperature_Textbox.Text, out result))
                    Settings.printingTemperature = result;
            });
        }

        void ApplicationSettings_progress_SecondsLeftChanged(object property_Value)
        {
            //property_Value = (int)(property_Value * 1.2f);
            double secs = (double)property_Value * 1.2;

            double _hours = secs / 3600;
            int hours = (int)Math.Floor(_hours);

            double secondsLeft = secs - (hours * 3600);

            double _minutes = secondsLeft / 60;
            int minutes = (int)Math.Floor(_minutes);

            secondsLeft -= minutes * 60;

            this.Dispatcher.BeginInvoke(() =>
            {
                eta_TextBlock.Text = "ETA: " + hours + "h " + minutes + "m " + (int)secondsLeft + "s";
            });
        }

        void ApplicationSettings_progress_PercentageDoneChanged(object property_Value)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                printing_ProgressBar.Value = (int)property_Value;
                printing_TextBlock.Text = property_Value + "% Complete";
            });
        }

        Timer updateGCodePrintTimer;

        bool firsttimeGCodePrint = false;

        private void updateGCodePrint(object o)
        {
            if (!firsttimeGCodePrint && Values.startGCodeFile != "")
            {
                firsttimeGCodePrint = false;
                return;
            }

            Dispatcher.BeginInvoke(() =>
            {
                if (gcodeprint_Filelist.Items.Count > 0)
                {
                    Values.currentGCodeFile = (gcodeprint_Filelist.SelectedItem as FileItems).FilePath;
                    Values.GCode_ListIndex = gcodeprint_Filelist.SelectedIndex;
                }
                else
                    Values.currentGCodeFile = "";
            });
        }

        void gcodeprint_Filelist_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            updateGCodePrintTimer = new Timer(new TimerCallback(this.updateGCodePrint), null, 100, -1);
        }

        void ApplicationSettings_GCode_IsBusyChanged(object property_Value)
        {
            //We want to disable the selector when the current GCode file is busy
            this.Dispatcher.BeginInvoke(() => 
            { 
                gcodeprint_Filelist.IsEnabled = !(bool)property_Value; 
            });
        }

        void comController_FileReadingStoppedEvent()
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                Values.GCode_IsBusy = false;
                Values.stl_IsBusy = false;

                print_Button.Content = "Print";
                //print_Button.IsEnabled = true;
            });
        }

        void comController_FileReadingStartedEvent()
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                Values.GCode_IsBusy = true;
                Values.stl_IsBusy = true;

                print_Button.Content = "Cancel";
                //print_Button.IsEnabled = false;
            });
        }

        private void print_Button_Click(object sender, RoutedEventArgs e)
        {
            if (print_Button.Content.ToString() == "Print")
            {
                comController.tryPrintingFromFile();

                MessageBox.Show("The print will continue under the lock screen!");
                PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
                PhoneApplicationFrame rootFrame = App.Current.RootVisual as PhoneApplicationFrame;
            }
            else//Cancel
            {
                new Thread(new ThreadStart(() =>
                    {
                        comController.cancelPrintingFromFile();
                    })).Start();
            }
        }

        private void x_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            dir = "X";
        }

        private void y_Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            dir = "Y";
        }

        private void z_Radiobutton_Checked_1(object sender, RoutedEventArgs e)
        {
            dir = "Z";
        }

        private void e_Radiobutton_Checked_1(object sender, RoutedEventArgs e)
        {
            dir = "E";
        }

        private void up_Button_Click(object sender, RoutedEventArgs e)
        {
            int distance, feedrate;
            if (int.TryParse(distance_Textbox.Text, out distance) && int.TryParse(speed_Textbox.Text, out feedrate))
                comController.moveAxis(distance, true, feedrate * 60, dir);
        }

        private void down_Button_Click(object sender, RoutedEventArgs e)
        {
            int distance, feedrate;
            if (int.TryParse(distance_Textbox.Text, out distance) && int.TryParse(speed_Textbox.Text, out feedrate))
                comController.moveAxis(distance, false, feedrate * 60, dir);
        }

        private void xhome_Button_Click(object sender, RoutedEventArgs e)
        {
            comController.homeXaxis();
        }

        private void yhome_Button_Click(object sender, RoutedEventArgs e)
        {
            comController.homeYaxis();
        }

        private void zhome_Button_Click(object sender, RoutedEventArgs e)
        {
            comController.homeZaxis();
        }

        private void allhome_Button_Click(object sender, RoutedEventArgs e)
        {
            comController.homeAll();
        }

        private void emergencystop_Button_Click(object sender, RoutedEventArgs e)
        {
            comController.emergencyStop();
        }

        private void gcode_Textbox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Check if enter was pressed
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Focus();//Focus the page thereby removing focus from the textbox
                comController.trySendingCommand(gcode_Textbox.Text);
                gcode_Textbox.Text = "";//Clear the textbox
            }
        }

        private void heat_Checkbox_Click(object sender, RoutedEventArgs e)
        {
            //int temperature;
            if ((sender as CheckBox).IsChecked.Value /*&& int.TryParse(temperature_Textbox.Text, out temperature)*/)
                comController.heatExtruder(Settings.printingTemperature);
            else
                comController.coolExtruder();
        }
    }
}
