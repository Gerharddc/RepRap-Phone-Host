using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Media;

namespace RepRap_Phone_Host.Containers
{
    /// <summary>
    /// A stream based wrapper for a stackpanel which contains text
    /// that wil automatically scroll when the maximum number of lines are reached.
    /// NOTE: Empty lines are currently not supported!
    /// </summary>
    class LogBox : Stream
    {
        private StackPanel stackPanel;//The stackpanel that will contain all teh lines
        private PhoneApplicationPage outputPage;//The page whose dispatcher will be used

        private int maxLines;//The maximum number of line that can fir into our stackpanel
        private int lineCount = 0;//The amount of lines we currently have in our stackpanel
        private int lineHeigth;//The heigth our lines should be
        private int fontSize; //The fontSize that will fit into the line

        /// <summary>
        /// Initializes a new stream based wrapper for a stcakpanel that
        /// will automatically write only the last lines that fit into it.
        /// </summary>
        /// <param name="_stackPanel">The stackpanel that should be used.</param>
        /// <param name="_outputPage">The page that the stackpanel is on.</param>
        /// <param name="_lineHeigth">The heigth that the lines should be.</param>
        public LogBox(StackPanel _stackPanel, PhoneApplicationPage _outputPage, int _lineHeigth)
        {
            //Transfer the recieved parameters to local variables
            stackPanel = _stackPanel;
            outputPage = _outputPage;
            lineHeigth = _lineHeigth;

            //Calculate the maximum amount of lines that will fit into our stackpanel
            maxLines = (int)Math.Floor(stackPanel.Height / _lineHeigth);//We need to round down sothat the last line is not clipped

            fontSize = (int)(lineHeigth * 0.8);//We calculate the fontsize once sothat we do not waste time every time a new line is added
        }

        /// <summary>
        /// Writes bytes from a string encoded in UTF8 to the StackPanel
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            //Generate a string from the recieved bytes that should have been encoded with UTF8
            string newText = Encoding.UTF8.GetString(buffer, offset, count).ToString();

            try//The following has a high risk of generating exceptions
            {
                //We write the text to a temporary file as to simply the process of reading line for line
                StreamWriter tempWriter = new StreamWriter("outPut.temp");
                tempWriter.Write(newText);
                tempWriter.Close();

                //We now read line for line from the file
                StreamReader tempReader = new StreamReader("outPut.temp");
                while (!tempReader.EndOfStream)//keep reading until the end
                {
                    string line = tempReader.ReadLine();//read a line
                    if (line.Length > 1)//Skeinforge has some strange output character which causes random new lines so we ignore them for now
                    {
                        addLineToStack(line);//add the line to the stack
                    }
                }
                tempReader.Dispose();//dispose of the no longer used streamreader
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);//debug the exception
            }
        }

        /// <summary>
        /// This functions writes a line to the log with
        /// the specified colour
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineColor"></param>
        public void writeLineWithColor(string line, Color lineColor)
        {
            if (line.Length > 1)//Skeinforge has some strange output character which causes random new lines so we ignore them for now
            {
                addLineToStack(line, lineColor);//add the line to the stack
            }
        }

        /// <summary>
        /// This function writes a line to the log
        /// </summary>
        /// <param name="line"></param>
        public void writeLine(string line)
        {
            addLineToStack(line);//add the line to the stack
        }

        /// <summary>
        /// Add a line of text to the stackpanel taking into account
        /// the maximum amount of lines that it can support.
        /// </summary>
        /// <param name="line"></param>
        private void addLineToStack(string line)
        {
            outputPage.Dispatcher.BeginInvoke(() =>//We need to perform UI related  tasks through the pages dispatcher
            {
                if (!(lineCount < maxLines))//check if we have reached our maximum amount of lines
                {
                    stackPanel.Children.RemoveAt(0);//remove the top line if the stack is full                    
                }
                else
                    lineCount++;//indicate that another line has been added

                var lineBlock = new TextBlock();//create a temporary textblock that will be added to the stackpanel
                lineBlock.Text = line;//set the lines text to the recieved parameter
                lineBlock.Height = lineHeigth;//set the line heigth to the indicated heigth
                lineBlock.FontSize = fontSize;//set the fontsize to the calculated fontsize that will fit into the above lineheigth
                //lineBlock.Foreground = new SolidColorBrush(Colors.Orange);
                stackPanel.Children.Add(lineBlock);//add the line to our stackpanel
            });
        }

        /// <summary>
        /// Add a line of text to the stackpanel taking into account
        /// the maximum amount of lines that it can support.
        /// </summary>
        /// <param name="line">The text of the line to add</param>
        /// <param name="lineColor">The color of the line to add</param>
        private void addLineToStack(string line, Color lineColor)
        {
            outputPage.Dispatcher.BeginInvoke(() =>//We need to perform UI related  tasks through the pages dispatcher
            {
                if (!(lineCount < maxLines) && stackPanel.Children.Count > 0)//check if we have reached our maximum amount of lines
                {
                    stackPanel.Children.RemoveAt(0);//remove the top line if the stack is full                    
                }
                else
                    lineCount++;//indicate that another line has been added

                var lineBlock = new TextBlock();//create a temporary textblock that will be added to the stackpanel
                lineBlock.Text = line;//set the lines text to the recieved parameter
                lineBlock.Height = lineHeigth;//set the line heigth to the indicated heigth
                lineBlock.FontSize = fontSize;//set the fontsize to the calculated fontsize that will fit into the above lineheigth
                lineBlock.Foreground = new SolidColorBrush(lineColor);//set the colour of the line
                stackPanel.Children.Add(lineBlock);//add the line to our stackpanel
            });
        }

        //All the rest are just unmodified but required overrides
        public override void WriteByte(byte value)
        {
            base.WriteByte(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            return;
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanRead
        {
            get { return false; }
        }
    }
}
