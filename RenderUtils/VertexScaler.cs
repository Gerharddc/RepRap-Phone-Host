using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RepRap_Phone_Host.RenderUtils
{
    static class VertexScaler
    {
        /// <summary>
        /// This function is used to scale a list of vertices to the specified
        /// size
        /// </summary>
        /// <param name="listToScale">The list of vertices that should be scaled. Ref is to prevent memory leaks.</param>
        /// <param name="scale">The multiplier that should be used to scale the vertices.</param>
        static public void rotateVertexList(ref List<VertexPositionColoredNormal> listToScale, float scale)
        {
            if (listToScale == null)
                return;

            //Create the matrix needed to scale each vertex
            Matrix scaleMatrix = Matrix.CreateScale(scale);

            for (int i = 0; i < listToScale.Count; i++)//got through all the vertices in the list
            {
                VertexPositionColoredNormal vertex = listToScale[i];//open the vertex for editing
                //apply the final matrix to the podition of the vertex
                vertex.Position = Vector3.Transform(vertex.Position, scaleMatrix);
                listToScale[i] = vertex;//save the new vertex
            }
        }
    }
}
