using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepRap_Phone_Host.RenderUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Threading;
using RepRap_Phone_Host.GlobalValues;
using System.Globalization;

namespace RepRap_Phone_Host.Stl
{
    static class StlImporter
    {
        /// <summary>
        /// This function loads a stl mesh into a vertex list
        /// from a specified file with certain parameters.
        /// </summary>
        /// <param name="filePath">The path of the stl to be imported</param>
        /// <param name="meshColor">The color that the mesh should have</param>
        /// <param name="centrePosition">The position that the model should be centred on.</param>
        /// <returns></returns>
        static public List<VertexPositionColoredNormal> loadStlFromFile(string filePath, Color meshColor)
        {
            List<VertexPositionColoredNormal> vertexList = new List<VertexPositionColoredNormal>();//the list of vertices that will be returned

            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

            if (!isoFile.FileExists(filePath))//File.Exists(filePath))
                return new List<VertexPositionColoredNormal>();

            IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(filePath, FileMode.OpenOrCreate, isoFile);

            //FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);//the file stream used to read the stl file
            BinaryReader binaryReader = new BinaryReader(fileStream);//the binary reader used to check the stl is binary and read it if it is

            binaryReader.ReadBytes(80);//read the binary hearder
            int faceCount = binaryReader.ReadInt32();//read the amount of faces

            if (fileStream.Length != 84 + faceCount * 50)//check if the file is binary by comparing the expected length for one to the actual length
            {
                //we are now reading an ascii stl
                binaryReader.Dispose();//dispose of the binary reader because it is not needed anymore
                fileStream.Dispose();//dispode of the filestream because it is not needed anymore
                //Convert the model from ascii to binary
                StlFormatConverter.convertAsciiToBinary(filePath);

                //Stop here if we should stop loading
                if (!Values.stl_IsBusy)
                    return null;

                fileStream = new IsolatedStorageFileStream(filePath, FileMode.OpenOrCreate, isoFile);//new FileStream(filePath, FileMode.Open);//the file stream used to read the stl file
                binaryReader = new BinaryReader(fileStream);//the binary reader used to check the stl is binary and read it if it is

                binaryReader.ReadBytes(80);//read the binary hearder
                faceCount = binaryReader.ReadInt32();//read the amount of faces

                loadBinary(ref vertexList, binaryReader, faceCount, meshColor);//load the binary stl into the vertex list
                fileStream.Dispose();
            }
            else
            {
                //we are now reading a binary stl
                loadBinary(ref vertexList, binaryReader, faceCount, meshColor);//load the binary stl into the vertex list
                fileStream.Dispose();//we dispose the filestream after it has been used
            }

            //Stop here if we should stop loading
            if (!Values.stl_IsBusy)
                return null;

            if (vertexList.Count < 1)//check if the list is empty
                throw new Exception("Something went wrong trying to import the stl");//throw an exception if it is

            return vertexList;//return the generated vertex list
        }

        /// <summary>
        /// Read a stl mesh into a vertex list from a file with
        /// binary formating.
        /// </summary>
        /// <param name="loadToList">The list that the mesh should be loaded to.</param>
        /// <param name="binaryReader">The binary reader that should be used.</param>
        /// <param name="faceCount">The amount of faces in the model.</param>
        /// <param name="meshColor">The color that the mesh should be loaded in.</param>
        static private void loadBinary(ref List<VertexPositionColoredNormal> loadToList, BinaryReader binaryReader, int faceCount, Color meshColor)
        {
            //load a stl mesh according to the binary file format
            for (int i = 0; i < faceCount && Values.stl_IsBusy; i++)//read the data for all the faces
            {
                Vector3 normal = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());//read the normal from the first 3 bytes

                for (int j = i * 3; j < (i * 3) + 3; j++)//cycle through the three vertices of the facet
                {
                    Vector3 vertice = new Vector3(binaryReader.ReadSingle(),
                        binaryReader.ReadSingle(), binaryReader.ReadSingle());//read the position of the vertex
                    loadToList.Insert(j, new VertexPositionColoredNormal(vertice, meshColor, normal));//add the vertex to the list
                }
                binaryReader.ReadInt16();//read the end of the facet
            }

            binaryReader.Dispose();//dispose of the reader after we are finished
        }
    }
}
