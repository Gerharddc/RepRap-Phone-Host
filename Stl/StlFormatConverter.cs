using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RepRap_Phone_Host.GlobalValues;
using Microsoft.Xna.Framework;
using System.Threading;
using RepRap_Phone_Host.RenderUtils;
using System.IO.IsolatedStorage;

using System.Diagnostics;
using System.Globalization;

namespace RepRap_Phone_Host.Stl
{
    //We need to convert ascii stl files to binary because binary renders and slices faster

    /// <summary>
    /// This class can checks if a stl file is ascii and can convert it to binary one
    /// </summary>
    public static class StlFormatConverter
    {
        /// <summary>
        /// This method checks if a given stl is ascii or binary and then converts it binary if needed
        /// </summary>
        /// <param name="filePath">The Stl file to check</param>
        public static void checkIfAsciiAndConvert(string filePath)
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(filePath, FileMode.OpenOrCreate, isf);

            //FileStream fileStream = new FileStream(filePath, FileMode.Open);//the file stream used to read the stl file
            BinaryReader binaryReader = new BinaryReader(fileStream);//the binary reader used to check the stl is binary and read it if it is

            binaryReader.ReadBytes(80);//read the binary hearder
            int faceCount = binaryReader.ReadInt32();//read the amount of faces

            if (fileStream.Length != 84 + faceCount * 50)//check if the file is binary by comparing the expected length for one to the actual length
            {
                //we have an ascii file on our hands and need to convert it
                /*new Thread(new ThreadStart(() => { */
                convertAsciiToBinary(filePath);/* })).Start();*/
            }
        }

        /// <summary>
        /// This method converts a given ascii stl file to a binary one
        /// </summary>
        /// <param name="filePath">The path of the ascii file to convert</param>
        public static void convertAsciiToBinary(string filePath)
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filePath, FileMode.OpenOrCreate, isf);
            StreamReader streamReader = new StreamReader(stream);//filePath);

            string readLine = "";

            List<Vector3> p1List = new List<Vector3>();
            List<Vector3> p2List = new List<Vector3>();
            List<Vector3> p3List = new List<Vector3>();
            List<Vector3> nList = new List<Vector3>();

            List<VertexPositionColoredNormal> vertexList = new List<VertexPositionColoredNormal>();

            Vector3 p1, p2, p3, n;

            string[] lineElements;

            float v1, v2, v3;

            while (!streamReader.EndOfStream && Values.stl_IsBusy)
            {
                readLine = streamReader.ReadLine();

                if (readLine.Contains("facet normal"))
                {
                    try
                    {
                        readLine = readLine.Replace(@"facet normal ", " ");//remove the word "facet normal" from the line
                        lineElements = readLine.Split(' ');//split the line at each space
                        //the first 7 segments are just empty characters
                        v1 = float.Parse(lineElements[lineElements.Length - 3], CultureInfo.InvariantCulture);//We need to use US formatting because the strings
                        v2 = float.Parse(lineElements[lineElements.Length - 2], CultureInfo.InvariantCulture);//contain "."s which represent the decimal points
                        v3 = float.Parse(lineElements[lineElements.Length - 1], CultureInfo.InvariantCulture);
                        n = new Vector3(v1, v2, v3);//store the vertex value

                        streamReader.ReadLine();//Skip the "outer loop" line

                        readLine = streamReader.ReadLine();
                        readLine = readLine.Replace(@"vertex", " ");//remove the word "vertex" from the line
                        lineElements = readLine.Split(' ');//split the line at each space
                        //the first 7 segments are just empty characters
                        v1 = float.Parse(lineElements[lineElements.Length - 3], CultureInfo.InvariantCulture);//We need to use US formatting because the strings
                        v2 = float.Parse(lineElements[lineElements.Length - 2], CultureInfo.InvariantCulture);//contain "."s which represent the decimal points
                        v3 = float.Parse(lineElements[lineElements.Length - 1], CultureInfo.InvariantCulture);
                        p1 = new Vector3(v1, v2, v3);//store the vertex value
                        vertexList.Add(new VertexPositionColoredNormal(p1, new Color(), n));

                        readLine = streamReader.ReadLine();
                        readLine = readLine.Replace(@"vertex", " ");//remove the word "vertex" from the line
                        lineElements = readLine.Split(' ');//split the line at each space
                        //the first 7 segments are just empty characters
                        v1 = float.Parse(lineElements[lineElements.Length - 3], CultureInfo.InvariantCulture);//We need to use US formatting because the strings
                        v2 = float.Parse(lineElements[lineElements.Length - 2], CultureInfo.InvariantCulture);//contain "."s which represent the decimal points
                        v3 = float.Parse(lineElements[lineElements.Length - 1], CultureInfo.InvariantCulture);
                        p2 = new Vector3(v1, v2, v3);//store the vertex value
                        vertexList.Add(new VertexPositionColoredNormal(p2, new Color(), n));

                        readLine = streamReader.ReadLine();
                        readLine = readLine.Replace(@"vertex", " ");//remove the word "vertex" from the line
                        lineElements = readLine.Split(' ');//split the line at each space
                        //the first 7 segments are just empty characters
                        v1 = float.Parse(lineElements[lineElements.Length - 3], CultureInfo.InvariantCulture);//We need to use US formatting because the strings
                        v2 = float.Parse(lineElements[lineElements.Length - 2], CultureInfo.InvariantCulture);//contain "."s which represent the decimal points
                        v3 = float.Parse(lineElements[lineElements.Length - 1], CultureInfo.InvariantCulture);
                        p3 = new Vector3(v1, v2, v3);//store the vertex value
                        vertexList.Add(new VertexPositionColoredNormal(p3, new Color(), n));
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }

            streamReader.Dispose();
            stream.Dispose();

            //Stop here if we need to stop loading
            if (!Values.stl_IsBusy)
                return;

            //Now that we have the list of vertices we can export them to the file in binary format.
            StlExporter.exportStlToFile(vertexList, filePath);
        }
    }
}
