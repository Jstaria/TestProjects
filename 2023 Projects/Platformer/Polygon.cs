using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Collision is dealt with inside of a collision manager,
// Using polygons as a base hitbox

namespace Platformer
{
    internal class Polygon
    {
        private List<Vector2> points; // Transformed points
        private Vector2 origin;   // Position of shape
        private float angle;      // Direction of shape
        private List<Vector2> model;  // "Model" of shape
        private bool inCollision;  // Is in collision

        public Polygon(List<Vector2> points, Vector2 origin)
        {
            this.points = points;
            this.origin = origin;
            this.angle = 0;
            
            // Model is the original shape of the polygon
            this.model = new List<Vector2>(points.Count);

            // We will grab each point without creating a reference to it
            // This is because model is a reference for points when rotating
            foreach (Vector2 point in points)
            {
                model.Add(new Vector2(point.X, point.Y));   
            }

            // Defaults to false
            this.inCollision = false;
        }
    }
}
