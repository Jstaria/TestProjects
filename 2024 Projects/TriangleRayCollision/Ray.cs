using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://courses.cs.washington.edu/courses/cse457/09au/lectures/triangle_intersection.pdf

namespace TriangleRayCollision
{
    internal static class Ray
    {
        public static bool Cast(Mesh mesh, Vector3 rayStart, Vector3 rayEnd)
        {
            bool inCollision = false;

            foreach (Triangle tri in mesh.Triangles)
            {
                //Matrix n = Matrix.Transpose(Matrix.CreateFromAxisAngle(tri.normal, 0));
                
                //float t = (rayEnd - Vector3.Transform(rayStart,n)) / (Vector3.Transform(rayEnd, n));
            }

            return inCollision;
        }
    }
}
