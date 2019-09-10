using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RepRap_Phone_Host.RenderUtils
{
    static class VertexRotator
    {
        /// <summary>
        /// Rotates a VertexPositionColoredNormal list according to given x, y and z rotations.
        /// </summary>
        /// <param name="listToRotate">The list of vertices to rotate. Ref is to prevent memory leaks.</param>
        /// <param name="xRot">The amount of yaw to be applied in Radians</param>
        /// <param name="yRot">The amount of pitch to be applied in Radians</param>
        /// <param name="zRot">The amount of roll to be applied in Radians</param>
        static public void rotateVertexList(ref List<VertexPositionColoredNormal> listToRotate, float xRot, float yRot, float zRot)
        {
            if (listToRotate == null)
                return;

            float xMax = float.NaN;//the maximum x coordinate of the model
            float xMin = float.NaN;//the minimum x coordinate of the model
            float xCentre = 0.0f;//the centre x coordinate of the model

            float yMax = float.NaN;//the maximum y coordinate of the model
            float yMin = float.NaN;//the minimum y coordinate of the model
            float yCentre = 0.0f;//the centre y coordinate of the model

            float zMax = float.NaN;//the maximum y coordinate of the model
            float zMin = float.NaN;//the minimum y coordinate of the model
            float zCentre = 0.0f;//the centre y coordinate of the model

            foreach (VertexPositionColoredNormal vertex in listToRotate)//cycle through all the vertices in the list
            {
                Vector3 vector = vertex.Position;//the position of the current vertice

                //just intitialise the values if it has not been done yet
                //float.NaN is used to avoid a situation were either the min or max is actually the default value if were to use a real number
                if (float.IsNaN(xMax))
                    xMax = vector.X;
                if (float.IsNaN(xMin))
                    xMin = vector.X;
                if (float.IsNaN(yMax))
                    yMax = vector.Y;
                if (float.IsNaN(yMin))
                    yMin = vector.Y;
                if (float.IsNaN(zMax))
                    zMax = vector.Z;
                if (float.IsNaN(zMin))
                    zMin = vector.Z;

                xMax = Math.Max(xMax, vector.X);//set this position to the maximum if it is larger than the previous one
                xMin = Math.Min(xMin, vector.X);//set this position to the minimum if it is smaller than the previous one

                yMax = Math.Max(yMax, vector.Y);//set this position to the maximum if it is larger than the previous one
                yMin = Math.Min(yMin, vector.Y);//set this position to the minimum if it is smaller than the previous one

                zMax = Math.Max(zMax, vector.Z);//set this position to the maximum if it is larger than the previous one
                zMin = Math.Min(zMin, vector.Z);//set this position to the minimum if it is smaller than the previous one
            }

            xCentre = (xMax + xMin) / 2;//calculate the centre x position 
            yCentre = (yMax + yMin) / 2;//calculate the centre x position 
            zCentre = (zMax + zMin) / 2;//calculate the centre x position

            //We need to translate the model sothat its centre is on 0,0,0 before we can rotate it
            Matrix translateToCentreMatrix = Matrix.CreateTranslation(new Vector3(-xCentre, -yCentre, -zCentre));
            //We then create the matrix that we will use to apply the requestes rotation
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(xRot, yRot, zRot);
            //We then create the matrix needed to move the model back to its original position
            Matrix translateBackMatrix = Matrix.CreateTranslation(new Vector3(xCentre, yCentre, zCentre));
            //Finally we create the matrix thats first centres it, then rotates it and finally takes it back to its original position
            Matrix matrixToApply = translateToCentreMatrix * rotationMatrix * translateBackMatrix;

            for (int i = 0; i < listToRotate.Count; i++)//got through all the vertices in the list
            {
                VertexPositionColoredNormal vertex = listToRotate[i];//Open the vertex for editing
                //apply the final matrix to the vector
                vertex.Position = Vector3.Transform(vertex.Position, matrixToApply);
                listToRotate[i] = vertex;//store the new vertex in the list
            }
        }
    }
}
