using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using MonoGame.Framework.WindowsPhone;
using RepRap_Phone_Host.GCode;
using RepRap_Phone_Host.FileSystem;
using RepRap_Phone_Host.GlobalValues;
using RepRap_Phone_Host.ListItems;
using System.Threading;
using System.Diagnostics;

namespace RepRap_Phone_Host
{
    public partial class MainPage : PhoneApplicationPage
    {
        //This is the constructor for the pivot which gets called from the main constructor
        public void Construct_GCodePivot()
        {
            Values.GCode_ItemsChangedEvent += GCode_GCode_ItemsChangedEvent;
            Values.GCode_ListIndexChangedEvent += GCode_GCode_ListIndexChangedEvent;
            Values.GCode_IsBusyChangedEvent += GCode_GCode_IsBusyChangedEvent;

            gcode_Filelist.ManipulationCompleted += GCode_Filelist_ManipulationCompleted;

            Values.layerCountChangedEvent += layerCountChangedEvent;
            Values.minLayerChangedEvent += values_minLayerChanged;
            Values.maxLayerChangedEvent += values_maxLayerChanged;

            min_Slider.ValueChanged += min_Slider_ValueChanged;
            max_Slider.ValueChanged += max_Slider_ValueChanged;
        }

        public void GCodePivot_Load()
        {
            if (Values.GCode_IsBusy)
                return;

            Values.currentGCodeFileChangedEvent += GCode_currentGCodeFileChangedEvent;

            if (Values.currentGCodeFile == "")
                return;

            try
            {
                XamlGame.assignDrawingSurface(renderingController, GCodeSurface, ref drawingSurfaceUpdateHandler);
                renderingController.Components.Add(new GCodeComponent(renderingController, this));
                (renderingController.Components[0] as GCodeComponent).loadGCodeContent();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void GCodePivot_Unload()
        {
            Values.currentGCodeFileChangedEvent -= GCode_currentGCodeFileChangedEvent;
            
            if (renderingController.Components.Count < 1)
                return;

            try
            {
                (renderingController.Components[0] as GCodeComponent).Dispose();
                renderingController.Components.RemoveAt(0);
            }
            catch (Exception e) 
            {
                Debug.WriteLine(e);
            }

            GC.Collect();
        }

        private void layerCountChangedEvent(object value)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                min_Slider.Maximum = Values.layerCount - 1;
                max_Slider.Maximum = Values.layerCount - 1;
            });
        }

        private void GCode_currentGCodeFileChangedEvent(object value)
        {
            try
            {
                (renderingController.Components[0] as GCodeComponent).loadGCodeContent();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        #region Event handlers
        private void values_minLayerChanged(object o)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                min_Slider.Value = Values.minLayer;
                bottomLayer_TextBlock.Text = "Bottom layer: " + Values.minLayer;
            });
        }

        private void values_maxLayerChanged(object o)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                max_Slider.Value = Values.maxLayer;
                topLayer_TextBlock.Text = "Top layer: " + Values.maxLayer;
            });
        }

        private void GCode_GCode_ListIndexChangedEvent(object value)
        {
            gcode_Filelist.SelectedIndex = (int)value;
        }

        private void GCode_GCode_ItemsChangedEvent(object value)
        {
            gcode_Filelist.ItemsSource = (List<FileItems>)value;
            updateGCode(null);
        }

        void GCode_GCode_IsBusyChangedEvent(object property_Value)
        {
            //We want to disable the selector when the current GCode file is busy
            this.Dispatcher.BeginInvoke(() =>
            {
                gcode_Filelist.IsEnabled = !(bool)property_Value;

                try
                {
                    SystemTray.ProgressIndicator = new ProgressIndicator();
                    SystemTray.ProgressIndicator.Text = "Working...";
                    SystemTray.ProgressIndicator.IsIndeterminate = (bool)property_Value;
                    SystemTray.ProgressIndicator.IsVisible = (bool)property_Value;
                }
                catch (Exception) { }

            });
        }

        Timer updateGocdeTimer;

        void GCode_Filelist_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            updateGocdeTimer = new Timer(new TimerCallback(this.updateGCode), null, 100, -1);
        }

        void updateGCode(object o)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (gcode_Filelist.Items.Count > 0)
                {
                    Values.currentGCodeFile = (gcode_Filelist.SelectedItem as FileItems).FilePath;
                    Values.GCode_ListIndex = gcode_Filelist.SelectedIndex;
                }
                else
                    Values.currentGCodeFile = "";           
            });
        }
        #endregion

        #region Ui events
        void max_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Values.maxLayer = (int)max_Slider.Value;
        }

        void min_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Values.minLayer = (int)min_Slider.Value;
        }

        private void rotateUpButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.rotateUp();
                        Dispatcher.BeginInvoke(() => pressed = rotateUpButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void rotateLeftButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.rotateLeft();
                        Dispatcher.BeginInvoke(() => pressed = rotateLeftButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void rotateRightButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.rotateRight();
                        Dispatcher.BeginInvoke(() => pressed = rotateRightButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void rotateDownButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.rotateDown();
                        Dispatcher.BeginInvoke(() => pressed = rotateDownButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void moveUpButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.moveUp();
                        Dispatcher.BeginInvoke(() => pressed = moveUpButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void moveLeftButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.moveLeft();
                        Dispatcher.BeginInvoke(() => pressed = moveLeftButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void moveRightButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.moveRight();
                        Dispatcher.BeginInvoke(() => pressed = moveRightButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void moveDownButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.moveDown();
                        Dispatcher.BeginInvoke(() => pressed = moveDownButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void zoomButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.zoomIn();
                        Dispatcher.BeginInvoke(() => pressed = zoomButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void unZoomButtonGCode_ManipulationStarted_1(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.zoomOut();
                        Dispatcher.BeginInvoke(() => pressed = unZoomButtonGCode.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }
        #endregion
    }
}
