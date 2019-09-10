using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepRap_Phone_Host.RenderUtils;
using System.IO;
using Microsoft.Xna.Framework;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.Stl
{
    /// <summary>
    /// This class contains the functions needed to export a binary
    /// stl file from a list of vertices.
    /// </summary>
    static class StlExporter
    {
        //TODO: this class needs a lot of error checking, exception handling and filesize checkking

        /// <summary>
        /// This function exports a list of vertices to a binary stl file
        /// </summary>
        /// <param name="model">The list of vertices that represent the model to be exported</param>
        /// <param name="filePath">The path of the file that should be exported</param>
        /// <returns>Returns if the process was sucessful</returns>
        static public bool exportStlToFile(List<VertexPositionColoredNormal> model, string filePath)
        {
            //we do not want to save empty models
            if (model == null)
                return false;//There was a problem with exporting

            var isf = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(filePath, FileMode.Create, isf);
            //FileStream fileStream = new FileStream(filePath, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);

            writeHeader(binaryWriter);
            int amountOfFaces = writeFaceCount(binaryWriter, model);
            writeFaces(binaryWriter, amountOfFaces, model);

            //Dispose of stream and writer after we have finished using them
            binaryWriter.Dispose();
            fileStream.Dispose();

            return true;//Exported completed without problems
        }

        /// <summary>
        /// This function converts and writes all the faces in the model to an stl file
        /// </summary>
        /// <param name="binaryWriter">The writer that will be used to write the faces to the file</param>
        /// <param name="faceCount">The amount of faces in the model</param>
        /// <param name="model">The list of vertices that represent the model</param>
        /// <returns>Returns if the process was sucessful</returns>
        static private bool writeFaces(BinaryWriter binaryWriter, int faceCount, List<VertexPositionColoredNormal> model)
        {
            for (int i = 0; i < faceCount; i++)//write the data for all the faces
            {
                //Get the normal from the first vertex in the pair and write it to the file 
                Vector3 normal = model[i * 3].Normal;
                binaryWriter.Write(normal.X);
                binaryWriter.Write(normal.Y);
                binaryWriter.Write(normal.Z);

                //Cycle through the 3 vertices per face and write them to the file
                for (int j = i * 3; j < (i * 3) + 3; j++)//cycle through the three vertices of the facet
                {
                    //Get the position vector and write it's values to the file
                    Vector3 vertice = model[j].Position;
                    binaryWriter.Write(vertice.X);
                    binaryWriter.Write(vertice.Y);
                    binaryWriter.Write(vertice.Z);
                }
                //Write empry 16 bit vertex terminating interger
                short emptyInt = 0;
                binaryWriter.Write(emptyInt);
            }

            return true;
        }

        /// <summary>
        /// This functions writes the facecount to the stl file
        /// </summary>
        /// <param name="binaryWriter">The writer that will be used to write the facecount to the file</param>
        /// <param name="model">The model that the facecount will be calculated for</param>
        /// <returns>The amount of faces in the model</returns>
        static private int writeFaceCount(BinaryWriter binaryWriter, List<VertexPositionColoredNormal> model)
        {
            //every 3 vertices represent 1 face
            int amountOfFaces = model.Count / 3;//The amount of faces need to be written as a 32 bit interger

            //write the amount of faces to the file
            binaryWriter.Write(amountOfFaces);

            //return the calculated amount of faces
            return amountOfFaces;
        }

        /// <summary>
        /// This function writes the 80 bit header to the binary stl file
        /// </summary>
        /// <param name="binaryWriter">The writer that will be used to write the header to the file</param>
        /// <returns>Returns if the process was sucessful</returns>
        static private bool writeHeader(BinaryWriter binaryWriter)
        {
            //The header needs to be a total size of 80 bytes that's why we have empty spaces in te text
            char[] headerText = "Exported with PolyMaker-Host".ToCharArray();
            int sizeOfText = sizeof(char) * headerText.Length;

            //Write the header text to the file
            binaryWriter.Write(headerText);

            //Fill the header with empty bytes until it is exactely 80 bytes in size because this is expected for the stl format
            while (binaryWriter.BaseStream.Length < 80)
                binaryWriter.Write(0);

            return true;
        }
    }
}
