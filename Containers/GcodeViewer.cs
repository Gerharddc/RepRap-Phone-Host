using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.Containers
{
    /// <summary>
    /// This class contains the functions needed to
    /// view a GCode file with a stackpanel and a scrollviewer. 
    /// </summary>
    class GCodeViewer
    {
        private StackPanel stackPanel; //The stackpanel the will contain the cached lines
        private ScrollViewer scrollViewer; //The scrollviewer that will contain the stackpanel
        private string filePath = ""; //The path of the GCode file

        private string[] longLineList; //The list of lines in the file
        private int curListPos = 0; //The current position in the list of line

        private double centrePos; //The centre position of the scrollviewer
        private int cacheLength; //The amount of lines to be cached
        private int lineHeigth; //The heigth of the lines
        private int fontSize; //The fontSize that will fit into the line
        private int maxDistBetweenUpdates; //The maximum distance that the scrollviewer should move between layoutupdate events

        /// <summary>
        /// This function dynamically fills a stackpanel with a certian
        /// amount of line from a GCode file and allows you to systematically
        /// scroll through it
        /// </summary>
        /// <param name="_stackPanel">The stackpanel that will contain the lines</param>
        /// <param name="_scrollViewer">The scrollviewer that contains the stackpanel</param>
        /// <param name="_filePath">The path to the GCode file</param>
        /// <param name="_cacheLength">The amount of lines to cache in the stackpanel</param>
        /// <param name="_lineHeigth">The heigth that each line should be</param>
        /// <param name="_maxDistanceBetweenUpdates">The maximum distance that the scrollviewer can go before the layout update event occurs. This should be less than 250.</param>
        public GCodeViewer(StackPanel _stackPanel, ScrollViewer _scrollViewer,
            int _cacheLength, int _lineHeigth, int _maxDistanceBetweenUpdates = 200)
        {
            //set the local variables to those recieved from the caller
            stackPanel = _stackPanel;
            scrollViewer = _scrollViewer;
            cacheLength = _cacheLength;
            lineHeigth = _lineHeigth;
            maxDistBetweenUpdates = _maxDistanceBetweenUpdates;

            fontSize = (int)(lineHeigth * 0.8);//We calculate the fontsize once sothat we do not waste time every time a new line is added

            scrollViewer.LayoutUpdated += scrollViewer_LayoutUpdated;//subscribe to the layoutupdate event sothat we know when the scrollviewer has moved
        }

        public void loadGCodeFromFile(string _filePath)
        {
            try
            {
                filePath = _filePath;
                loadFile();
                loadInitialLines();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// This function loads the GCode file into memory
        /// </summary>
        private void loadFile()
        {
            try
            {
                var isf = IsolatedStorageFile.GetUserStoreForApplication();

                IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(filePath, FileMode.OpenOrCreate, isf);

                StreamReader sr = new StreamReader(isoStream);//filePath);//the streamreader that will read the GCode file

                if (sr == null)//check if the stream is empty
                    throw new Exception("Filename not valid");//throw an exception

                string longString = sr.ReadToEnd();//load the the entire file into a string
                sr.Dispose();//dispose of the streamreader as it is not needed anymore

                isoStream.Dispose();

                longLineList = longString.Split('\n');//split the file into lines
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Load the intial lines into the stackpanel
        /// </summary>
        private void loadInitialLines()
        {
            stackPanel.Dispatcher.BeginInvoke(() =>
            {
                stackPanel.Children.Clear();
                scrollViewer.ScrollToVerticalOffset(0);

                if (longLineList == null)//check if the list of line is empty
                    return;
                //throw new Exception("Line list is null");//throw an exception

                try
                {
                    for (int i = 0; i < cacheLength && i < longLineList.Length; i++)//add a cachelength amount of lines
                    {
                        var tempBlock = new TextBlock();//create a temporary textblock
                        tempBlock.FontSize = fontSize;//adjust the font to fit into the line heigth
                        tempBlock.Text = (i + 1) + ": " + longLineList[i];//add the line number in front of the text and get the line from the list
                        tempBlock.Height = lineHeigth;//set the block to the specified heigth
                        stackPanel.Children.Add(tempBlock);//add the line to the stackpanel
                        curListPos = i;//update the current list position
                    }
                }
                catch (Exception) { }

                //scrollViewer.ScrollToVerticalOffset(0);
            });
        }

        /// <summary>
        /// This gets called when the scrollviewers position gets updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void scrollViewer_LayoutUpdated(object sender, EventArgs e)
        {
            centrePos = (scrollViewer.ScrollableHeight - scrollViewer.ActualHeight) / 2;//calculate the vertical centre position
            checkScrollPosition();//check in which direction we should scroll
        }

        /// <summary>
        /// Check if we should scroll the list and in which direction.
        /// </summary>
        private void checkScrollPosition()
        {
            if (scrollViewer.VerticalOffset > scrollViewer.ScrollableHeight - maxDistBetweenUpdates)//check the scroller is in the lower limit
                updateScroller(true);//scroll down

            else if (scrollViewer.VerticalOffset < maxDistBetweenUpdates && curListPos > cacheLength)//check if the scroller is in the upper limit
                updateScroller(false);//scroll up
        }

        /// <summary>
        /// Update the stackpanel with the next lines and
        /// move the scrollviewer to its centre position.
        /// </summary>
        /// <param name="down">Should we scroll up or down?</param>
        private void updateScroller(bool down)
        {
            //do not update the scroller if a file has not been set yet
            if (filePath == "")
                return;

            //NB there is a limeit of 250 elements, we therefore always have to remove old elements before adding new ones

            try
            {
                if (down)//we should scroll down
                {
                    int oldCurListPos = curListPos;

                    //remove the old lines
                    int amountToRemove = cacheLength / 2;
                    for (int i = 0; i < amountToRemove; i++)
                    {
                        if (stackPanel.Children.Count < 1)
                            return;

                        stackPanel.Children.RemoveAt(0);
                    }
                    scrollViewer.ScrollToVerticalOffset(centrePos);//scroll to the middle of the scrollviewer

                    //add the new lines
                    int posToAddTo = curListPos + cacheLength / 2;
                    for (int i = oldCurListPos + 1; i <= posToAddTo && i < longLineList.Length; i++)
                    {
                        var tempBlock = new TextBlock();//create a temporary block to represent the next line
                        tempBlock.FontSize = fontSize;//make the text fit into the lineheigth
                        tempBlock.Text = (i + 1) + ": " + longLineList[i];//add the line number to the line and get the text from the list
                        tempBlock.Height = lineHeigth;//set the line to lineheigth
                        stackPanel.Children.Add(tempBlock);//add the line to the stackpanel

                        curListPos = i;
                    }
                }

                else//we should scroll up
                {
                    int oldCurListPos = curListPos;

                    //remove the old lines
                    int amountToRemove = cacheLength / 2;
                    for (int i = 0; i < amountToRemove; i++)
                    {
                        if (stackPanel.Children.Count < 1)
                            return;

                        stackPanel.Children.RemoveAt(stackPanel.Children.Count - 1);
                    }
                    scrollViewer.ScrollToVerticalOffset(centrePos + scrollViewer.ActualHeight);//scroll to the middle of the scrollviewer

                    //add the new lines
                    int posToAddTo = curListPos - cacheLength - cacheLength / 2 + 1;
                    for (int i = oldCurListPos - cacheLength; i >= posToAddTo && i >= 0 && i < longLineList.Length; i--)
                    {
                        var tempBlock = new TextBlock();//create a temporary block to represent the next line
                        tempBlock.FontSize = fontSize;//make the text fit into the lineheigth
                        tempBlock.Text = (i + 1) + ": " + longLineList[i];//add the line number to the line and get the text from the list
                        tempBlock.Height = lineHeigth;//set the line to lineheigth
                        stackPanel.Children.Insert(0, tempBlock);//add the line to the stackpanel

                        curListPos = i + cacheLength - 1;
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
