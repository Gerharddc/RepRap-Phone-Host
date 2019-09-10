using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RepRap_Phone_Host.RenderUtils
{
    /// <summary>
    /// This class contains the function needed to create a renderable grid box
    /// </summary>
    static class BoxCreator
    {
        /// <summary>
        /// This fnction creates a list of vertices that render a grid bx which represents the
        /// printing bed area.
        /// </summary>
        /// <param name="length">The length of the printing bed in mm</param>
        /// <param name="width">The width of the printing bed in mm</param>
        /// <param name="heigth">The heigth of the printing bed in mm</param>
        /// <param name="boxColor">The colour that the grid should be</param>
        /// <returns>The list of vertices that render that grid</returns>
        static public List<VertexPositionColoredNormal> createBox(int length, int width, int heigth, Color boxColor)
        {
            List<Vector3> vectorList = new List<Vector3>();
            List<VertexPositionColoredNormal> vertexList = new List<VertexPositionColoredNormal>();

            //Bottom lines
            for (int i = 0; i < length + 5; i += 5)
            {
                //vertical
                vectorList.Add(new Vector3(i, 0, 0));//start
                vectorList.Add(new Vector3(i, width, 0));//end                

            }
            for (int i = 0; i < width + 5; i += 5)
            {
                //horizontal
                vectorList.Add(new Vector3(0, i, 0));//start
                vectorList.Add(new Vector3(length, i, 0));//end
            }


            //Top lines
            //vertical
            vectorList.Add(new Vector3(0, 0, heigth));//start
            vectorList.Add(new Vector3(0, width, heigth));//end
            vectorList.Add(new Vector3(length, 0, heigth));//start
            vectorList.Add(new Vector3(length, width, heigth));//end
            //horizontal
            vectorList.Add(new Vector3(0, 0, heigth));//start
            vectorList.Add(new Vector3(length, 0, heigth));//end
            vectorList.Add(new Vector3(0, width, heigth));//start
            vectorList.Add(new Vector3(length, width, heigth));//end

            foreach (Vector3 vector in vectorList)
            {
                //For now we use Vector3(0.5f, 0.5f, 0.5f) as the normal for our grid because it is visible from all angles albeit quite dark from some
                //TODO: find a better normal or other solution that will make the grid mor visible from other angles
                //Perhaps drawing 2 line(1 on to and 1 on the bottom) is the answer?
                vertexList.Add(new VertexPositionColoredNormal(vector, boxColor, new Vector3(0.5f, 0.5f, 0.5f)));
            }

            return vertexList;
        }
    }
}
