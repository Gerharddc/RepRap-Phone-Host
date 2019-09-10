using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.FileSystem
{
    static class FileLister
    {
        public static void listFilesOnPivot(StackPanel stlStack, StackPanel GCodeStack)
        {
            var isf = IsolatedStorageFile.GetUserStoreForApplication();

            //Get info on the stl and GCode directories
            //DirectoryInfo stlDirectoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Stl");
            //DirectoryInfo GCodeDirectoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\GCode");

            //Clear the stl list and add all the stl files to it
            //stlStack = new StackPanel();
            stlStack.Children.Clear();
            stlStack.Children.Add(new System.Windows.Shapes.Rectangle());//We need to add a rectangle for the stackpanel to resize

            foreach (string fileName in isf.GetFileNames("*.stl"))//FileInfo fileInfo in stlDirectoryInfo.GetFiles("*.stl"))
            {
                var fileCheckBox = new CheckBox();
                fileCheckBox.Content = fileName.Replace(".stl", "");

                stlStack.Children.Add(fileCheckBox);
            }

            //Clear the stl list and add all the GCode files to it
            //GCodeStack = new StackPanel();
            GCodeStack.Children.Clear();
            GCodeStack.Children.Add(new System.Windows.Shapes.Rectangle());

            foreach (string fileName in isf.GetFileNames("*.gcode"))//FileInfo fileInfo in GCodeDirectoryInfo.GetFiles("*.gcode"))
            {
                var fileCheckBox = new CheckBox();
                fileCheckBox.Content = fileName.Replace(".gcode", "");

                GCodeStack.Children.Add(fileCheckBox);
            }
        }

        /// <summary>
        /// This method dteremines which files should be shared and adds them to the the global list
        /// </summary>
        /// <param name="stlStack">The stackpanel of stl files</param>
        /// <param name="GCodeStack">The stackpanel of GCodew files</param>
        public static void determineFilesToShare(StackPanel stlStack, StackPanel GCodeStack)
        {
            App.filesToSave.Clear();

            foreach (UIElement element in stlStack.Children)
            {
                var checkBox = element as CheckBox;
                if (checkBox != null && checkBox.IsChecked == true)
                {
                    App.filesToSave.Add(checkBox.Content.ToString() + ".stl");
                }
            }

            foreach (UIElement element in GCodeStack.Children)
            {
                var checkBox = element as CheckBox;
                if (checkBox != null && checkBox.IsChecked == true)
                {
                    App.filesToSave.Add(checkBox.Content.ToString() + ".gcode");
                }
            }
        }

        public static void deleteCheckedFiles(StackPanel stlStack, StackPanel GCodeStack, Dispatcher dispatcher)
        {
            //Get info on the stl and GCode directories
            //DirectoryInfo stlDirectoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Stl");
            //DirectoryInfo GCodeDirectoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\GCode");
            var isf = IsolatedStorageFile.GetUserStoreForApplication();

            //We need to remove all files reprsented by checked checkboxes
            //We do not need to remove the checkboxes because we should raise the refresh events after we finsihed deleting

            //First we delete the stl files
            int childrenCount = -1;
            dispatcher.BeginInvoke(() =>
            {
                childrenCount = stlStack.Children.Count;
            });

            //We need to wait for the above dispatcher code to finish
            while (childrenCount == -1)
                Thread.Sleep(5);

            for (int i = 0; i < childrenCount; i++)
            {
                //1 = true, 0 = false, -1 = null
                int childCheckBoxChecked = -1;

                dispatcher.BeginInvoke(() =>
                {
                    var child = stlStack.Children[i];
                    childCheckBoxChecked = (child.GetType() == typeof(CheckBox) && (child as CheckBox).IsChecked.Value) ? 1 : 0;
                });

                while (childCheckBoxChecked == -1)
                    Thread.Sleep(5);

                if (childCheckBoxChecked == 1)
                {
                    string fileName = null;

                    dispatcher.BeginInvoke(() =>
                    {
                        var child = stlStack.Children[i];
                        fileName = (child as CheckBox).Content.ToString() + ".stl";
                    });

                    while (fileName == null)
                        Thread.Sleep(5);

                    if (isf.FileExists(fileName))
                        isf.DeleteFile(fileName);
                    /*foreach (FileInfo fileInfo in stlDirectoryInfo.GetFiles("*.stl"))
                    {
                        if (fileInfo.Name.Equals(fileName))
                        {
                            try
                            {
                                fileInfo.Delete();
                            }
                            catch (Exception) { }
                            continue;
                        }
                    }*/
                }
            }

            //Then we delete the GCode files
            childrenCount = -1;
            dispatcher.BeginInvoke(() =>
            {
                childrenCount = GCodeStack.Children.Count;
            });

            //We need to wait for the above dispatcher code to finish
            while (childrenCount == -1)
                Thread.Sleep(5);

            for (int i = 0; i < childrenCount; i++)
            {
                //1 = true, 0 = false, -1 = null
                int childCheckBoxChecked = -1;

                dispatcher.BeginInvoke(() =>
                {
                    var child = GCodeStack.Children[i];
                    childCheckBoxChecked = (child.GetType() == typeof(CheckBox) && (child as CheckBox).IsChecked.Value) ? 1 : 0;
                });

                while (childCheckBoxChecked == -1)
                    Thread.Sleep(5);

                if (childCheckBoxChecked == 1)
                {
                    string fileName = null;

                    dispatcher.BeginInvoke(() =>
                    {
                        var child = GCodeStack.Children[i];
                        fileName = (child as CheckBox).Content.ToString() + ".gcode";
                    });

                    while (fileName == null)
                        Thread.Sleep(5);

                    if (isf.FileExists(fileName))
                        isf.DeleteFile(fileName);
                    /*foreach (FileInfo fileInfo in GCodeDirectoryInfo.GetFiles("*.gcode"))
                    {
                        if (fileInfo.Name.Equals(fileName))
                        {
                            try
                            {
                                fileInfo.Delete();
                            }
                            catch (Exception) { }
                            continue;
                        }
                    }*/
                }
            }
        }

        public static void selectAll(StackPanel stlStack, StackPanel GCodeStack)
        {
            for (int i = 0; i < stlStack.Children.Count; i++)
            {
                if (stlStack.Children[i].GetType() == typeof(CheckBox))
                {
                    var child = stlStack.Children[i] as CheckBox;
                    child.IsChecked = true;
                }
            }

            for (int i = 0; i < GCodeStack.Children.Count; i++)
            {
                if (GCodeStack.Children[i].GetType() == typeof(CheckBox))
                {
                    var child = GCodeStack.Children[i] as CheckBox;
                    child.IsChecked = true;
                }
            }
        }

        public static void unSelectAll(StackPanel stlStack, StackPanel GCodeStack)
        {
            for (int i = 0; i < stlStack.Children.Count; i++)
            {
                if (stlStack.Children[i].GetType() == typeof(CheckBox))
                {
                    var child = stlStack.Children[i] as CheckBox;
                    child.IsChecked = false;
                }
            }

            for (int i = 0; i < GCodeStack.Children.Count; i++)
            {
                if (GCodeStack.Children[i].GetType() == typeof(CheckBox))
                {
                    var child = GCodeStack.Children[i] as CheckBox;
                    child.IsChecked = false;
                }
            }
        }
    }
}
