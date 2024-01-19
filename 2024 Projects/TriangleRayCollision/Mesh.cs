using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriangleRayCollision
{
    internal class Mesh
    {
        // Geometric Info
        private List<VertexBuffer> vertexBuffers = new List<VertexBuffer>(); // Graphics buffers

        //private List<List<Triangle>> triangles = new List<List<Triangle>>();

        public List<VertexBuffer> VertexBuffers { get { return vertexBuffers; } }

        public void AddMeshColor(List<Triangle> triangles, Color color)
        {
            List<VertexPositionColorNormal> vertices = new List<VertexPositionColorNormal>();

            // Loop through each triangle, if a triangle just so happens to not have a face, it will be deleted
            for (int index = 0; index < triangles.Count; index++)
            {
                if (triangles[index].faceCount == 0)
                {
                    triangles.Remove(triangles[index]);
                    index--;
                    continue;
                }

                // If the triangle does have a face, it will appropriately be set into a list of vertices with that triangles normals
                for (int i = 0; i < 3; i++)
                {
                    VertexPositionColorNormal vertex = new VertexPositionColorNormal(triangles[index].vertices[i], color, triangles[index].normal);
                    vertices.Add(vertex);
                }
            }

            // An array of drawables we will give to the GPU
            vertexBuffers.Add(new VertexBuffer(Game1._graphics.GraphicsDevice, typeof(VertexPositionColorNormal), vertices.Count, BufferUsage.WriteOnly));
            vertexBuffers[vertexBuffers.Count - 1].SetData<VertexPositionColorNormal>(vertices.ToArray());
        }
    }
}
