using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RepRap_Phone_Host.RenderUtils
{
    public struct VertexPositionColoredNormal : IVertexType
    {
        Vector3 vertexPosition;
        Color vertexColor;
        Vector3 vertexNormal;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );


        public VertexPositionColoredNormal(Vector3 pos, Color color, Vector3 normal)
        {
            vertexPosition = pos;
            vertexColor = color;
            vertexNormal = normal;
        }

        //Public methods for accessing the components of the custom vertex.
        public Vector3 Position
        {
            get { return vertexPosition; }
            set { vertexPosition = value; }
        }
        public Color Color
        {
            get { return vertexColor; }
            set { vertexColor = value; }
        }
        public Vector3 Normal
        {
            get { return vertexNormal; }
            set { vertexNormal = value; }
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }
}
