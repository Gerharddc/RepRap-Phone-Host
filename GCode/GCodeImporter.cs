using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using RepRap_Phone_Host.RenderUtils;
using RepRap_Phone_Host.GlobalValues;
using System.Globalization;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.GCode
{
    class GCodeImporter
    {
        static public List<VertexPositionColoredNormal> loadGCodeFromFile(string filePath, Color filamentColor)
        {
            var isf = IsolatedStorageFile.GetUserStoreForApplication();

            if (!isf.FileExists(filePath))//!File.Exists(filePath))
                return new List<VertexPositionColoredNormal>();

            IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(filePath, FileMode.OpenOrCreate, isf);

            StreamReader streamReader = new StreamReader(isoStream);//filePath);

            //List<Vector3> GCodePointList = new List<Vector3>();
            List<VertexPositionColoredNormal> vertexList = new List<VertexPositionColoredNormal>();

            Vector3 lastPos = new Vector3(0, 0, 0); //the previous position
            Vector3 lastNormal = new Vector3(0, 0, 0);
            string line; //used to store the current line information
            string[] lineParts; //array that conatins the different parts of the line
            bool relativeGCode = false; //used to determine if values are absolute or relative
            bool extrudedMove = true; //used to indicate if the move is extruded or not.
            float xPosition = 0.0f; //used to store the current x position for multiple lines
            float yPosition = 0.0f; //used to store the current y position for multiple lines
            float zPosition = -1.0f; //used to store the current z position for multiple lines
            int currentLayer = 0; //the current layer

            while (!streamReader.EndOfStream && Values.GCode_IsBusy) //read all the lines in the GCode file
            {
                //Quit if the file is not busy anymore
                /*if (!Values.GCode_IsBusy)*/
                   // return null;

                line = streamReader.ReadLine(); //read GCode line to string
                line = line.Split(';')[0]; //remove comments from GCode

                if (line.Contains("G90")) //check if GCode is absolte
                    relativeGCode = false; //store this data

                if (line.Contains("G91")) //check if GCode is relative
                    relativeGCode = true; //store this data

                if (!line.Contains("G1") && !line.Contains("G0"))
                    continue;

                if (!(line.Contains("X") || line.Contains("Y") || line.Contains("Z"))) //check if line conatins coordinates
                    continue; //we should not read lines that do do not contain lineSegments

                if (line.Contains("E")) //check if the move is extruded
                    extrudedMove = true; //store this data
                else
                    extrudedMove = false; //store this data

                lineParts = line.Split(' '); //split the line into segments that were sperated by spaces

                foreach (string linePart in lineParts)//itterate through all parts of the line
                {
                    if (linePart.Contains("X")) //check if linePart contains a x coordinate
                    {
                        if (!relativeGCode) //check if moves are not relative
                            xPosition = float.Parse(linePart.Split('X')[1], CultureInfo.InvariantCulture); //set coordinate to absolte parsed float
                        else //moves are relative
                            xPosition += float.Parse(linePart.Split('X')[1], CultureInfo.InvariantCulture); //set coordinate to relative parsed float
                    }
                    else if (linePart.Contains("Y")) //check if linePart contains a x coordinate
                    {
                        if (!relativeGCode) //check if moves are not relative
                            yPosition = float.Parse(linePart.Split('Y')[1], CultureInfo.InvariantCulture); //set coordinate to absolte parsed float
                        else //moves are relative
                            yPosition += float.Parse(linePart.Split('Y')[1], CultureInfo.InvariantCulture); //set coordinate to relative parsed float
                    }
                    else if (linePart.Contains("Z")) //check if linePart contains a x coordinate
                    {
                        var prev = zPosition;

                        if (!relativeGCode) //check if moves are not relative
                            zPosition = float.Parse(linePart.Split('Z')[1], CultureInfo.InvariantCulture); //set coordinate to absolte parsed float
                        else //moves are relative
                            zPosition += float.Parse(linePart.Split('Z')[1], CultureInfo.InvariantCulture); //set coordinate to relative parsed float

                        //If the z position has changed then we have entered a new layer
                        if (zPosition != prev)
                        {
                            Values.layerStartIndices[currentLayer] = vertexList.Count;
                            currentLayer++;
                        }
                    }
                }

                Vector3 pos = new Vector3(xPosition, yPosition, zPosition);
                Vector3 normal = new Vector3(-yPosition, xPosition, 0);
                normal = Vector3.Normalize(normal);

                if (extrudedMove)
                {
                    vertexList.Add(new VertexPositionColoredNormal(lastPos, filamentColor, lastNormal));
                    vertexList.Add(new VertexPositionColoredNormal(pos, filamentColor, normal));
                }

                lastPos = pos;
                lastNormal = normal;
            }

            Debug.WriteLine("Loaded");

            Values.layerCount = currentLayer;

            streamReader.Dispose();
            isoStream.Dispose();

            return vertexList;
        }
    }
}
