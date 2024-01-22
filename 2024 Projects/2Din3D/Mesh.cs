using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _2Din3D
{
    internal class Mesh
    {
        // Geometric Info
        private VertexBuffer vertexBuffer; // Graphics buffers
        private List<Triangle> triangles = new List<Triangle>();
        private List<VertexPositionColorNormal> vertices = new List<VertexPositionColorNormal>();

        public VertexBuffer VertexBuffer { get { return vertexBuffer; } }
        public List<Triangle> Triangles { get { return triangles; } }

        public void AddMeshColor(List<Triangle> triangles, Color color)
        {
            this.triangles = triangles;

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
            vertexBuffer = new VertexBuffer(Game1._graphics.GraphicsDevice, typeof(VertexPositionColorNormal), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorNormal>(vertices.ToArray());
        }

        public void MoveMesh(Vector3 direction)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                VertexPositionColorNormal v = vertices[i];
                v.Position += direction;
                vertices[i] = v;
            }

            vertexBuffer = new VertexBuffer(Game1._graphics.GraphicsDevice, typeof(VertexPositionColorNormal), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorNormal>(vertices.ToArray());
        }

        public void CreateSquare(Vector3 position, float scale)
        {
            Triangle tri1 = new Triangle();
            tri1.vertices = new Vector3[3];
            tri1.vertices[0] = new Vector3(-1 * scale, 1 * scale, 0) + position;
            tri1.vertices[1] = new Vector3(1 * scale, 1 * scale, 0) + position;
            tri1.vertices[2] = new Vector3(-1 * scale, -1 * scale, 0) + position;

            Vector3 edge1 = tri1.vertices[1] - tri1.vertices[0];
            Vector3 edge2 = tri1.vertices[2] - tri1.vertices[0];

            Vector3 Normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

            tri1.normal = Normal;

            tri1.faceCount = 1;

            foreach (Vector3 v in tri1.vertices)
            {
                vertices.Add(new VertexPositionColorNormal(v, Color.White, tri1.normal));
            }

            Triangle tri2 = new Triangle();
            tri2.vertices = new Vector3[3];
            tri2.vertices[0] = new Vector3(1 * scale, 1 * scale, 0) + position;
            tri2.vertices[1] = new Vector3(1 * scale, -1 * scale, 0) + position;
            tri2.vertices[2] = new Vector3(-1 * scale, -1 * scale, 0) + position;

            edge1 = tri2.vertices[1] - tri2.vertices[0];
            edge2 = tri2.vertices[2] - tri2.vertices[0];

            Normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

            tri2.normal = Normal;

            tri2.faceCount = 1;

            foreach (Vector3 v in tri2.vertices)
            {
                vertices.Add(new VertexPositionColorNormal(v, Color.White, tri2.normal));
            }

            triangles.Add(tri1);
            triangles.Add(tri2);

            vertexBuffer = new VertexBuffer(Game1._graphics.GraphicsDevice, typeof(VertexPositionColorNormal), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorNormal>(vertices.ToArray());
        }
    }
}
