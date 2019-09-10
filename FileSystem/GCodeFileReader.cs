using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using RepRap_Phone_Host.Communication;
using RepRap_Phone_Host.GCode;
using RepRap_Phone_Host.GlobalValues;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.FileSystem
{
    /// <summary>
    /// This class contains all the functions needed to read and send
    /// GCode from a file. It also etimates printing time and progress.
    /// </summary>
    class GCodeFileReader
    {
        GCodeSender GCodeSender;//The GCodesender that will be used to send the lines of GCode
        Thread GCodeReadingThread;//The thread that reads and sends the GCode
        GCodeGenerator GCodeGenerator;//The generator that will home and cool

        /// <summary>
        /// Intialise a class that can read and send GCode line for line from
        /// a file.
        /// </summary>
        /// <param name="_GCodeSender">The fully intialised GCodeSender that will be used to send the GCode</param>
        public GCodeFileReader(GCodeSender _GCodeSender, GCodeGenerator _GCodeGenerator)
        {
            GCodeSender = _GCodeSender;
            GCodeGenerator = _GCodeGenerator;
        }

        /// <summary>
        /// Read and send GCode line for line from a file
        /// </summary>
        /// <param name="path">The path of the GCode file to use</param>
        public void readAndtrySendingGCodeFromFile(string path)
        {
            try
            {
                //Start reading loop in new thread
                GCodeReadingThread = new Thread(new ThreadStart(() => GCodeReadAndSender(path)));
                GCodeReadingThread.Start();

                if (FileReadingStarted != null)
                    FileReadingStarted();//Alert any listeners that we have started reading a file
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        bool stopGCode = false;

        /// <summary>
        /// Cancel reading a file if currently busy reading one.
        /// </summary>
        public void cancelReadingFromFile()
        {
            stopGCode = true;
            /*if (GCodeReadingThread.ThreadState == ThreadState.Running)
            {
                GCodeReadingThread.Abort();
                stopGCode = true;

                if (FileReadingStopped != null)
                    FileReadingStopped();//Alert any listeners that we have stopped reading a file

                GCodeGenerator.coolExtruder();
                GCodeGenerator.homeAll();
            }*/
        }

        //This event is called when the file reading process has started
        public event FileReadingStartedHandler FileReadingStarted;
        public delegate void FileReadingStartedHandler();

        //This event is called when the file reading process has stopped
        public event FileReadingStoppedHandler FileReadingStopped;
        public delegate void FileReadingStoppedHandler();

        /// <summary>
        /// Reads and send the GCode from a file.This should be run inside a thread.
        /// </summary>
        /// <param name="path"></param>
        private void GCodeReadAndSender(string path)
        {
            //To estimate how much printing time is left we substract the printing time of the lines that have been pinted from the
            //total printing time for the file.

            GCodeProgressEstimator progEst = new GCodeProgressEstimator();

            /*double totalTimeForFile = GCodeProgressEstimator.estimateTotalTimeForFile(path,
                new Vector3(ApplicationSettings.currentPos_X, ApplicationSettings.currentPos_Y, ApplicationSettings.currentPos_Z));*/
            double totalTimeForFile = progEst.estimateTotalTimeForFile(path);

            double printedTime = 0;//This value represents the amount of time that we have printed

            //This value stores the position of the printed after the previous line has completed and is used to calculate the time needed to complete
            //the current line. We should probably rather use the current printhead position in some way but that will not work directly because the line
            //that is currently being sent is sometimes a few lines ahead of the line that was previously completed
            Vector3 prevPos = new Vector3(Values.currentPos_X, Values.currentPos_Y, Values.currentPos_Z);

            var isf = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isf);

            StreamReader streamReader = new StreamReader(isoStream);//path); 

            bool problemSending = false;

            /*float prevE = 0;
            float prevF0 = 0;
            float prevF1 = 0;*/

            int linecount = 0;

            //Reset the progress estimator
            progEst = new GCodeProgressEstimator();

            while (!streamReader.EndOfStream && !problemSending && !stopGCode)//Wait for the file to be completely read or for an error to happen
            {
                linecount++;

                string line = streamReader.ReadLine();

                //remove comments
                if (line.Contains(";"))
                    line = line.Split(';')[0];

                //ignore empty lines
                if (line.Length < 2)
                    continue;

                //send the GCode line (it gets proccessed first though)
                problemSending = !GCodeSender.trySendingGCode(line);

                GCode.GCode GCodeLine = new GCode.GCode(line);

                /*double addTime = 0;

                if (GCodeLine.hasG)
                {
                    addTime = progEst.calculateTimeForLine(GCodeLine);
                }

                if (addTime > 0.0)
                    printedTime += addTime;*/
                double addTime = progEst.calculateTimeForLine(GCodeLine);
                printedTime += addTime;

                Values.progress_SecondsLeft = (int)(totalTimeForFile - printedTime);
                Values.progress_PercentageDone = (int)(printedTime / totalTimeForFile * 100);
            }

            Debug.WriteLine("finsihed");

            Values.progress_SecondsLeft = totalTimeForFile;
            Values.progress_PercentageDone = 0;

            if (FileReadingStopped != null)
                FileReadingStopped();//Alert any listeners that we have stopped reading a file

            GCodeGenerator.coolExtruder();
            GCodeGenerator.homeAll();

            stopGCode = false;

            isoStream.Dispose();
            streamReader.Dispose();
        }
    }
}
