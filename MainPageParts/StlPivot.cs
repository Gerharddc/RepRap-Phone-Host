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
using RepRap_Phone_Host.Stl;
using RepRap_Phone_Host.FileSystem;
using RepRap_Phone_Host.GlobalValues;
using RepRap_Phone_Host.ListItems;
using RepRap_Phone_Host.RenderUtils;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace RepRap_Phone_Host
{
    public partial class MainPage : PhoneApplicationPage
    {
        //This is the constructor for the pivot which gets called from the main constructor
        public void Construct_StlPivot()
        {
            Values.stl_IsBusyChangedEvent += ApplicationSettings_stl_IsBusyChanged;
            Values.Stl_ItemsChangedEvent += Stl_Stl_ItemsChangedEvent;
            Values.Stl_ListIndexChangedEvent += Stl_Stl_ListIndexChangedEvent;

            stl_Filelist.ManipulationCompleted += stl_Filelist_ManipulationCompleted;
        }

        public void StlPivot_Load()
        {
            if (Values.stl_IsBusy)
                return;

            Values.currentStlFileChangedEvent += Stl_currentStlFileChangedEvent;

            if (Values.currentStlFile == "")
                return;

            try
            {
                XamlGame.assignDrawingSurface(renderingController, StlSurface, ref drawingSurfaceUpdateHandler);
                renderingController.Components.Add(new StlComponent(renderingController, this));
                (renderingController.Components[0] as StlComponent).loadStlContent();
            }
            catch (Exception e) 
            {
                Debug.WriteLine(e);
            }
        }

        public void StlPivot_Unload()
        {
            Values.currentStlFileChangedEvent -= Stl_currentStlFileChangedEvent;

            if (renderingController.Components.Count < 1)
                return;

            (renderingController.Components[0] as StlComponent).Dispose();
            renderingController.Components.RemoveAt(0);

            GC.Collect();
        }

        #region Event handlers
        //If we use the selction changed event of the list then there comes this strange effect where everything constantly changes because
        //once a item on the list changes then the event is called to change the item on the list and then the first listcahnged event happens again and so forth
        //The solution is to only respond to manipulation completed events on the list, the thing is though that the event occurs just before the current
        //value actually changes and we therefore use a timer to respond a 100millis after the event when the selected item has been updated
        Timer updateStlTimer;

        private void Stl_currentStlFileChangedEvent(object value)
        {
            (renderingController.Components[0] as StlComponent).loadStlContent();
        }

        private void Stl_Stl_ListIndexChangedEvent(object value)
        {
            stl_Filelist.SelectedIndex = (int)value;
        }

        private void Stl_Stl_ItemsChangedEvent(object value)
        {
            stl_Filelist.ItemsSource = (List<FileItems>)value;
            updateStl(null);
        }

        void stl_Filelist_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            updateStlTimer = new Timer(new TimerCallback(this.updateStl), null, 100, -1);
        }

        void updateStl(object o)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (stl_Filelist.Items.Count > 0)
                {
                    Values.currentStlFile = (stl_Filelist.SelectedItem as FileItems).FilePath;
                    Values.Stl_ListIndex = stl_Filelist.SelectedIndex;
                }
                else
                    Values.currentStlFile = "";
            });
        }

        void ApplicationSettings_stl_IsBusyChanged(object property_Value)
        {
            Dispatcher.BeginInvoke(() =>
            {
                stl_Filelist.IsEnabled = !(bool)property_Value;
                scale_Textbox.IsEnabled = !(bool)property_Value;
                xOffset_Textbox.IsEnabled = !(bool)property_Value;
                yOffset_Textbox.IsEnabled = !(bool)property_Value;
                xRotation_TextBox.IsEnabled = !(bool)property_Value;
                yRotation_TextBox.IsEnabled = !(bool)property_Value;
                zRotation_TextBox.IsEnabled = !(bool)property_Value;
                xRotation_Slider.IsEnabled = !(bool)property_Value;
                yRotation_Slider.IsEnabled = !(bool)property_Value;
                zRotation_Slider.IsEnabled = !(bool)property_Value;
                saveButtonStl.IsEnabled = !(bool)property_Value;             

                SystemTray.ProgressIndicator.Text = "Working...";
                SystemTray.ProgressIndicator.IsIndeterminate = (bool)property_Value;
                SystemTray.ProgressIndicator.IsVisible = (bool)property_Value;
            });
        }
        #endregion

        #region Ui events
        void rotation_Slider_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            //Because the model is rotated once the ApplicationSetting is changed we only change it once
            //a manipulation has completed for performance reasons
            //But we still want to update the text of the appropriate textblock
            var slider = sender as Slider;

            if (slider.Name[0] == 'x')//slider.Name.Contains("x"))
            {
                //xRotation_TextBlock.Text = "X Rotation: " + (int)slider.Value;
                xRotation_TextBox.Text = ((int)slider.Value).ToString();
            }
            else if (slider.Name[0] == 'y')//slider.Name.Contains("y"))
            {
                //yRotation_TextBlock.Text = "Y Rotation: " + (int)slider.Value;
                yRotation_TextBox.Text = ((int)slider.Value).ToString();
            }
            else
            {
                //zRotation_TextBlock.Text = "Z Rotation: " + (int)slider.Value;
                zRotation_TextBox.Text = ((int)slider.Value).ToString();
            }
        }

        void rotation_Slider_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            var slider = sender as Slider;

            if (slider.Name[0] == 'x')//slider.Name.Contains("x"))
            {
                //xRotation_TextBlock.Text = "X Rotation: " + (int)slider.Value;
                xRotation_TextBox.Text = ((int)slider.Value).ToString();
                (renderingController.Components[0] as StlComponent).changeXRot((float)slider.Value);
            }
            else if (slider.Name[0] == 'y')//slider.Name.Contains("y"))
            {
                //yRotation_TextBlock.Text = "Y Rotation: " + (int)slider.Value;
                yRotation_TextBox.Text = ((int)slider.Value).ToString();
                (renderingController.Components[0] as StlComponent).changeYRot((float)slider.Value);
            }
            else
            {
                //zRotation_TextBlock.Text = "Z Rotation: " + (int)slider.Value;
                zRotation_TextBox.Text = ((int)slider.Value).ToString();
                (renderingController.Components[0] as StlComponent).changeZRot((float)slider.Value);
            }
        }

        private void rotation_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;

            float rot = 0;
            float.TryParse(textBox.Text, out rot);

            if (rot > 360)
            {
                int count = (int)(Math.Floor(rot / 360));
                rot -= 360 * count;
            }

            textBox.Text = rot.ToString();

            if (textBox.Name[0] == 'x')
            {
                xRotation_Slider.Value = rot;
                (renderingController.Components[0] as StlComponent).changeXRot(rot);
            }
            else if (textBox.Name[0] == 'y')
            {
                yRotation_Slider.Value = rot;
                (renderingController.Components[0] as StlComponent).changeYRot(rot);
            }
            else
            {
                zRotation_Slider.Value = rot;
                (renderingController.Components[0] as StlComponent).changeZRot(rot);
            }
        }

        private void rotateUpButtonStl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.rotateUp();
                        Dispatcher.BeginInvoke(() => pressed = rotateUpButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void rotateLeftButtonStl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.rotateLeft();
                        Dispatcher.BeginInvoke(() => pressed = rotateLeftButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void rotateRightButtonStl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.rotateRight();
                        Dispatcher.BeginInvoke(() => pressed = rotateRightButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void rotateDownButtonStl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.rotateDown();
                        Dispatcher.BeginInvoke(() => pressed = rotateDownButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void moveUpButtonStl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.moveUp();
                        Dispatcher.BeginInvoke(() => pressed = moveUpButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void moveLeftButtonStl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.moveLeft();
                        Dispatcher.BeginInvoke(() => pressed = moveLeftButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void moveRightButtonStl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.moveRight();
                        Dispatcher.BeginInvoke(() => pressed = moveRightButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void moveDownButtonStl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.moveDown();
                        Dispatcher.BeginInvoke(() => pressed = moveDownButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void zoomButtonStl_Press(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.zoomIn();
                        Dispatcher.BeginInvoke(() => pressed = zoomButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void unZoomButtonStl_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            new Thread(() =>
            {
                bool pressed = true;
                while (pressed)
                {
                    try
                    {
                        renderingController.zoomOut();
                        Dispatcher.BeginInvoke(() => pressed = unZoomButtonStl.IsPressed);
                        Thread.Sleep(100);
                    }
                    catch (Exception) { break; }
                }
            }).Start();
        }

        private void saveButtonStl_Click(object sender, RoutedEventArgs e)
        {
            //Save the modified stl
            var newFile = (renderingController.Components[0] as StlComponent).saveModelandReset();

            //Update the list of stl files
            Values.Stl_Items = FileFinder.findFilesAndCreateList("Stl", ".stl");

            //Reset the sliders
            xRotation_Slider.Value = 0;
            xRotation_TextBlock.Text = "X Rotation: 0";
            yRotation_Slider.Value = 0;
            yRotation_TextBlock.Text = "Y Rotation: 0";
            zRotation_Slider.Value = 0;
            zRotation_TextBlock.Text = "Z Rotation: 0";

            //Reset scale 
            scale_Textbox.LostFocus -= scale_Textbox_LostFocus;
            scale_Textbox.Text = "1.0";
            scale_Textbox.LostFocus += scale_Textbox_LostFocus;

            //Reset offset
            /*xOffset_Textbox.LostFocus -= xOffset_Textbox_LostFocus;
            xOffset_Textbox.Text = "0.0";
            xOffset_Textbox.LostFocus += xOffset_Textbox_LostFocus;
            yOffset_Textbox.LostFocus -= yOffset_Textbox_LostFocus;
            yOffset_Textbox.Text = "0.0";
            yOffset_Textbox.LostFocus += yOffset_Textbox_LostFocus;*/

            //Determine the index of the new stl file and assign it
            for (int i = 0; i < Values.Stl_Items.Count; i++)
            {
                if (Values.Stl_Items[i].FilePath == newFile)
                {
                    Values.Stl_ListIndex = i;
                    break;
                }
            }

            //Move to the slicer pivot
            MainPivot.SelectedItem = SlicerPivot;

            //Update the current stl files
            Values.currentStlFile = newFile;
        }

        private void scale_Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            float newScale;

            if (float.TryParse(scale_Textbox.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out newScale))
                (renderingController.Components[0] as StlComponent).scaleModel(newScale);
        }

        private void xOffset_Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            float newOffset;

            if (float.TryParse(xOffset_Textbox.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out newOffset))
                (renderingController.Components[0] as StlComponent).changeXOffset(newOffset);
        }

        private void yOffset_Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            float newOffset;

            if (float.TryParse(yOffset_Textbox.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out newOffset))
                (renderingController.Components[0] as StlComponent).changeYOffset(newOffset);
        }
        #endregion
    }
}
