using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadow
{
    /// <summary>
    /// Manages a list of walls
    /// </summary>
    internal class WallManager
    {
        // A list of walls being kept track of
        public static List<Wall> walls;

        // Light position which shadows move away from
        private Vector2 lightPos;

        /// <summary>
        /// Number of walls the manager has
        /// </summary>
        public int Count
        {
            get { return walls.Count; }
        }

        // Not needed if the player is alwasy center due to the camera
        //public Vector2 LightPos
        //{
        //    get { return lightPos; }
        //    set { lightPos = value; }
        //}

        /// <summary>
        /// Manages walls
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        public WallManager()
        {
            WallManager.walls = new List<Wall>();
            lightPos = new Vector2(Game1.width/2,Game1.height/2);
        }

        /// <summary>
        /// Adds a wall to the end of the list
        /// </summary>
        /// <param name="wall"></param>
        public void AddWall(Wall wall)
        {
            WallManager.walls.Add(wall);
        }

        /// <summary>
        /// Removes wall at index
        /// </summary>
        /// <param name="index">Index of the wall in the list</param>
        public void RemoveWall(int index)
        {
            walls.RemoveAt(index);
        }

        /// <summary>
        /// Uses wall's draw method to draw each wall
        /// </summary>
        /// <param name="sb"></param>
        public void DrawWalls(SpriteBatch sb)
        {
            foreach (Wall wall in walls)
            {
                wall.Draw(sb);
            }
        }

        /// <summary>
        /// Update if specified wall has shadow
        /// </summary>
        /// <param name="hasShadow"></param>
        /// <param name="index">Index in wall list to update</param>
        public void UpdateHasShadow(bool hasShadow, int index)
        {
            walls[index].HasShadow = hasShadow;
        }

        public void DrawShadows(SpriteBatch sb, GraphicsDevice GraphicsDevice, Matrix transform)
        {
            lightPos = Player.lightPos;

            sb.End();
            ShapeBatch.Begin(GraphicsDevice);

            foreach (Wall wall in walls)
            {
                if(wall.HasShadow)
                {
                    List<Vector2> corners = new List<Vector2>(4);

                    corners.Add(Vector2.Transform(new Vector2(wall.Position.X, wall.Position.Y), transform));
                    corners.Add(Vector2.Transform(new Vector2(wall.Position.X + wall.Position.Width, wall.Position.Y), transform));
                    corners.Add(Vector2.Transform(new Vector2(wall.Position.X + wall.Position.Width, wall.Position.Y + wall.Position.Height), transform));
                    corners.Add(Vector2.Transform(new Vector2(wall.Position.X, wall.Position.Y + wall.Position.Height), transform));

                    Vector2 nextCorner = new Vector2(0, 0);
                    Vector2 nextDirection = new Vector2(0, 0);

                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 direction = new Vector2(lightPos.X - corners[i].X, lightPos.Y - corners[i].Y);

                        try
                        {
                            nextCorner = corners[i + 1];
                        }

                        catch
                        {
                            nextCorner = corners[0];
                        }

                        finally
                        {
                            nextDirection = new Vector2(lightPos.X - nextCorner.X, lightPos.Y - nextCorner.Y);

                            ShapeBatch.Triangle(corners[i], nextCorner, ((nextCorner - nextDirection * 1000) + (corners[i] - direction * 1000)) / 2, Color.Black);
                            ShapeBatch.Triangle(corners[i], nextCorner - nextDirection * 1000, corners[i] - direction * 1000, Color.Black);
                            ShapeBatch.Triangle(nextCorner, nextCorner - nextDirection * 1000, corners[i] - direction * 1000, Color.Black);
                        }
                    }

                    
                }
            }

            ShapeBatch.End();
            sb.Begin(transformMatrix: transform);
        }
    }
}
