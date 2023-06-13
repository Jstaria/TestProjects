using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfterImage
{
    public struct Polygon
    {
        public List<Vector2> points; // Transformed points
        public Vector2 origin;   // Position of shape
        public float angle;      // Direction of shape
        public List<Vector2> model;  // "Model" of shape
        public bool overlap;     // Flag for overlap
    }

    internal class CollisionCheck
    {
        private List<Polygon> polygons = new List<Polygon>();

        public List<Polygon> Polygons
        {
            get { return polygons; }
        }

        // Adds a polygon to polygons
        public Polygon AddPolygon
        {
            set { polygons.Add(value); }
        }

        public void UpdateShapes()
        {
            // for each polygon
            for (int i = 0; i < polygons.Count; i++)
            {
                Polygon polygon = polygons[i];
                polygon.overlap = false;
                polygons[i] = polygon;

                // For each point
                for (int j = 0; j < polygons[i].points.Count; j++)
                {
                    Vector2 point = polygons[i].model[j];
                    float angle = polygons[i].angle;
                    Vector2 origin = polygons[i].origin;

                    // Rotates points via 'angle'
                    polygons[i].points[j] =
                        new Vector2(
                            ((point.X * (float)Math.Cos(angle)) - (point.Y * (float)Math.Sin(angle)) + origin.X),
                            ((point.X * (float)Math.Sin(angle)) + (point.Y * (float)Math.Cos(angle)) + origin.Y));
                }
            }
        }

        public bool Collide(Polygon one, Polygon two)
        {
            Polygon polygon1 = one;
            Polygon polygon2 = two;

            for (int shape = 0; shape < 2; shape++)
            {
                // Swap to check each shape on each other
                if (shape == 1)
                {
                    polygon1 = two;
                    polygon2 = one;
                }

                // For each edge
                for (int i = 0; i < polygon1.points.Count; i++)
                {
                    int j = (i + 1) % polygon1.points.Count;

                    // Creates Normal
                    Vector2 axisProj = new Vector2(-(polygon1.points[j].Y - polygon1.points[i].Y), polygon1.points[j].X - polygon1.points[i].X);

                    // Sets min and max for new 1D axis
                    float min_r1 = float.PositiveInfinity;
                    float max_r1 = float.NegativeInfinity;

                    for (int points = 0; points < polygon1.points.Count; points++)
                    {
                        float dot = (polygon1.points[points].X * axisProj.X + polygon1.points[points].Y * axisProj.Y);

                        min_r1 = Math.Min(min_r1, dot);
                        max_r1 = Math.Max(max_r1, dot);
                    }

                    // Sets min and max for new 1D axis along shape two
                    float min_r2 = float.PositiveInfinity;
                    float max_r2 = float.NegativeInfinity;

                    for (int points = 0; points < polygon2.points.Count; points++)
                    {
                        float dot = (polygon2.points[points].X * axisProj.X + polygon2.points[points].Y * axisProj.Y);

                        min_r2 = Math.Min(min_r2, dot);
                        max_r2 = Math.Max(max_r2, dot);
                    }

                    if (!(max_r2 >= min_r1 && max_r1 >= min_r2))
                    {
                        //System.Diagnostics.Debug.WriteLine(false);
                        return false;
                    }
                }
            }
            //System.Diagnostics.Debug.WriteLine(true);
            return true;
        }

        public void Update(GameTime gt)
        {
            UpdateShapes();

            // Checks for overlap 

            for (int m = 0; m < polygons.Count; m++)
            {
                Polygon shapeCollide = polygons[m];

                for (int n = (m + 1); n < polygons.Count; n++)
                {
                    shapeCollide.overlap |= Collide(polygons[m], polygons[n]);
                    System.Diagnostics.Debug.WriteLine(polygons[m].overlap);
                    polygons[m] = shapeCollide;
                }
            }
        }
    }
}