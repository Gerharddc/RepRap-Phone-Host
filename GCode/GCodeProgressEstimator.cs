using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.GCode
{
    class GCodeProgressEstimator
    {
        /// <summary>
        /// This method estimates the amount of seconds needed for printing a specified file
        /// </summary>
        /// <param name="path">The GCode file to estimate the printing time of</param>
        public double estimateTotalTimeForFile(string path)
        {
            double totalTime = 0;

            try
            {
                IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream stream = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isf);
                StreamReader streamReader = new StreamReader(stream);//path);

                int linecount = 0;

                while (!streamReader.EndOfStream)
                {
                    linecount++;

                    GCode currentLine = new GCode(streamReader.ReadLine());

                    double addTime = calculateTimeForLine(currentLine);

                    //if (addTime > 0.0)
                    totalTime += addTime;
                }

                streamReader.Dispose();
                stream.Dispose();
            }
            catch (Exception e) 
            {
                Debug.WriteLine(e);
            }

            return totalTime;
        }

        bool relative = false;

        float lastX = 0;
        float lastY = 0;
        float lastZ = 0;

        float lastF0 = 0;
        float lastF1 = 0;

        /// <summary>
        /// This method calculates the amount of seconds it will take to print a specified line of GCode
        /// </summary>
        /// <param name="line">The line of GCode to estimate printing time on.</param>
        public float calculateTimeForLine(GCode line)
        {
            if (line.hasG && line.G == 91)
            {
                relative = true;
                lastX = 0;
                lastY = 0;
                lastZ = 0;
            }

            if (!(line.hasG && (line.G == 0 || line.G == 1)))
                return 0;

            float timeNeeded = 0;

            float totalDistance = 0;

            float xDistance = 0;
            float yDistance = 0;
            float zDistance = 0;

            if (line.hasX)
            {
                xDistance = line.X - lastX;
                if (!relative)
                    lastX = line.X;
            }

            if (line.hasY)
            {
                yDistance = line.Y - lastY;
                if (!relative)
                    lastY = line.Y;
            }

            if (line.hasZ)
            {
                zDistance = line.Z - lastZ;
                if (!relative)
                    lastZ = line.Z;
            }

            //find the total distance
            totalDistance = (float)Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance) + (zDistance * zDistance));
            //totalDistance = (float)Math.Pow(xDistance * xDistance + yDistance * yDistance + zDistance * zDistance, 1 / 2);

            float feedRt = 0;

            if (line.hasF)
            {
                feedRt = line.F;
                if (line.G == 0)
                    lastF0 = line.F;
                else if (line.G == 1)
                    lastF1 = line.F;
            }
            else
            {
                if (line.G == 0)
                    feedRt = lastF0;
                else if (line.G == 1)
                    feedRt = lastF1;
            }

            float mmPerSecond = feedRt / 60.0f;//The speed of the movement in terms of mm covered per secdeond

            timeNeeded = (float)(totalDistance / mmPerSecond);

            return timeNeeded;
        }
    }
}
