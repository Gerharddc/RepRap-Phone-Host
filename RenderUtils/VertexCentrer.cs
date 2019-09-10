using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RepRap_Phone_Host.RenderUtils
{
    static class VertexCentrer
    {
        /// <summary>
        /// Centres a VertexPositionColoredNormal list around a given x and y point in a Vector3.
        /// The z will be moved to have a minimum of the specified point.
        /// </summary>
        /// <param name="listToCentre">The list of vertices to centre. Ref is to prevent memory leaks</param>
        /// <param name="centrePosition">The position they should be centred anround.</param>
        static public void centreVertexList(ref List<VertexPositionColoredNormal> listToCentre, Vector3 centrePosition)
        {
            if (listToCentre == null)
                return;

            float xMax = float.NaN;//the maximum x coordinate of the model
            float xMin = float.NaN;//the minimum x coordinate of the model
            float xCentre = 0.0f;//the centre x coordinate of the model
            float xTarget = centrePosition.X;//the target centre x coordinate of the model
            float xMove = 0.0f;//the amount that the models needs to move to reach the target position

            float yMax = float.NaN;//the maximum y coordinate of the model
            float yMin = float.NaN;//the minimum y coordinate of the model
            float yCentre = 0.0f;//the centre y coordinate of the model
            float yTarget = centrePosition.Y;//the target centre y coordinate of the model
            float yMove = 0.0f;

            float zMin = float.NaN;//the minimum y coordinate of the model            
            float zTarget = centrePosition.Z;//the target minimum y coordinate of the model
            float zMove = 0.0f;//the amount that the models needs to move to reach the target position

            foreach (VertexPositionColoredNormal vertex in listToCentre)//cycle through all the vertices in the list
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
                if (float.IsNaN(zMin))
                    zMin = vector.Z;

                xMax = Math.Max(xMax, vector.X);//set this position to the maximum if it is larger than the previous one
                xMin = Math.Min(xMin, vector.X);//set this position to the minimum if it is smaller than the previous one

                yMax = Math.Max(yMax, vector.Y);//set this position to the maximum if it is larger than the previous one
                yMin = Math.Min(yMin, vector.Y);//set this position to the minimum if it is smaller than the previous one

                zMin = Math.Min(zMin, vector.Z);//set this position to the minimum if it is smaller than the previous one
            }

            xCentre = (xMax + xMin) / 2;//calculate the centre x position 
            xMove = xTarget - xCentre;//calculate the amount that has to be moved in the x axis

            yCentre = (yMax + yMin) / 2;//calculate the centre y position 
            yMove = yTarget - yCentre;//calculate the amount that has to be moved in the y axis

            zMove = zTarget - zMin;//calculate the amount that has to be moved in the z axis
            /*
            //Give the caller the centreposition that can be used to move the model back to its previous position
            if (oldCentrePosition != null)
                oldCentrePosition = new Vector3(xCentre, yCentre, zMin);*/

            for (int i = 0; i < listToCentre.Count; i++)//got through all the vertices in the list
            {
                VertexPositionColoredNormal vertex = listToCentre[i];//Open the vertex for editing
                Vector3 vector = vertex.Position;//Oen the vector of thevertex for editing
                //move the model to the target positions
                vector.X += xMove;
                vector.Y += yMove;
                vector.Z += zMove;
                vertex.Position = vector;//Store the new vector in the vertex
                listToCentre[i] = vertex;//store the new vertex in the list
            }
        }

        /// <summary>
        /// Centres a VertexPositionColoredNormal list around a given x and y point in a Vector3.
        /// The z will be moved to have a minimum of the specified point.
        /// </summary>
        /// <param name="listToCentre">The list of vertices to centre. Ref is to prevent memory leaks</param>
        /// <param name="centrePosition">The position they should be centred anround.</param>
        /// <param name="objectSink">The distnace that the object should be sunken into z</param>
        static public void centreVertexList(ref List<VertexPositionColoredNormal> listToCentre, Vector3 centrePosition, float objectSink)
        {
            if (listToCentre == null)
                return;

            float xMax = float.NaN;//the maximum x coordinate of the model
            float xMin = float.NaN;//the minimum x coordinate of the model
            float xCentre = 0.0f;//the centre x coordinate of the model
            float xTarget = centrePosition.X;//the target centre x coordinate of the model
            float xMove = 0.0f;//the amount that the models needs to move to reach the target position

            float yMax = float.NaN;//the maximum y coordinate of the model
            float yMin = float.NaN;//the minimum y coordinate of the model
            float yCentre = 0.0f;//the centre y coordinate of the model
            float yTarget = centrePosition.Y;//the target centre y coordinate of the model
            float yMove = 0.0f;

            float zMin = float.NaN;//the minimum y coordinate of the model            
            float zTarget = centrePosition.Z;//the target minimum y coordinate of the model
            float zMove = 0.0f;//the amount that the models needs to move to reach the target position

            foreach (VertexPositionColoredNormal vertex in listToCentre)//cycle through all the vertices in the list
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
                if (float.IsNaN(zMin))
                    zMin = vector.Z;

                xMax = Math.Max(xMax, vector.X);//set this position to the maximum if it is larger than the previous one
                xMin = Math.Min(xMin, vector.X);//set this position to the minimum if it is smaller than the previous one

                yMax = Math.Max(yMax, vector.Y);//set this position to the maximum if it is larger than the previous one
                yMin = Math.Min(yMin, vector.Y);//set this position to the minimum if it is smaller than the previous one

                zMin = Math.Min(zMin, vector.Z);//set this position to the minimum if it is smaller than the previous one
            }

            xCentre = (xMax + xMin) / 2;//calculate the centre x position 
            xMove = xTarget - xCentre;//calculate the amount that has to be moved in the x axis

            yCentre = (yMax + yMin) / 2;//calculate the centre y position 
            yMove = yTarget - yCentre;//calculate the amount that has to be moved in the y axis

            zMove = zTarget - zMin - objectSink;//calculate the amount that has to be moved in the z axis
            /*
            //Give the caller the centreposition that can be used to move the model back to its previous position
            if (oldCentrePosition != null)
                oldCentrePosition = new Vector3(xCentre, yCentre, zMin);*/

            for (int i = 0; i < listToCentre.Count; i++)//got through all the vertices in the list
            {
                VertexPositionColoredNormal vertex = listToCentre[i];//Open the vertex for editing
                Vector3 vector = vertex.Position;//Oen the vector of thevertex for editing
                //move the model to the target positions
                vector.X += xMove;
                vector.Y += yMove;
                vector.Z += zMove;
                vertex.Position = vector;//Store the new vector in the vertex
                listToCentre[i] = vertex;//store the new vertex in the list
            }
        }
    }
}
