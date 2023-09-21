using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGolf_TeamB
{
    internal class GolfBall : GameObject
    {
        //protected bool[] flags;

        /// <summary>
        /// The speed/power of the ball
        /// </summary>
        protected Vector2 power;

        /// <summary>
        /// Direction of the ball
        /// </summary>
        protected Vector2 direction;
        protected Vector2 movement;

        // Intersection points
        //Vector2 intersection1;
        //Vector2 intersection2;
        //Vector2 intersection3;
        //Vector2 intersection4;

        // For shapebatch
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Tracks position of mouse, updated in Update()
        /// </summary>
        protected Vector2 mousePosition;

        /// <summary>
        /// Direction of the current mouse position
        /// </summary>
        private Vector2 currentMousePosDir;

        public Vector2 CurrentMousePosDir
        {
            get { return currentMousePosDir; }
            set
            {
                currentMousePosDir = positionV - value;
                currentMousePosDir.Normalize();
            }
        }

        /// <summary>
        /// Get and Set for power vector
        /// </summary>
        public float Power
        {
            get { return power.X; }
            set { power = new Vector2(-value, -value); }
        }

        /// <summary>
        /// Golf ball object which only takes in the original position of the asset, and the asset itself
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="position"></param>
        public GolfBall(Texture2D asset, Rectangle position) : base(asset, position)
        {
            this.position = position;
            this.asset = asset;

            // power starts off as 0, ball is not moving
            this.power = new Vector2(0, 0);

            graphicsDevice = this.asset.GraphicsDevice;
        }

        public void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        /// <summary>
        /// Moves ball
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update()
        {
            positionV = new Vector2(X, Y);
            moveBall();
        }

        /// <summary>
        /// Moves ball based on mouse direction
        /// </summary>
        public void moveBall()
        {
            float distance = Vector2.Distance(
                mousePosition,
                positionV);

            angle = MathF.Atan2(direction.Y, direction.X);

            direction.Normalize();
            movement = direction;

            Collision();

            X += movement.X * power.X;
            Y += movement.Y * power.Y;

            power -= power * .02f;
        }

        public void Direction(Vector2 mousePosition)
        {
            this.mousePosition = mousePosition;
            direction = mousePosition - positionV;
        }

        // Checks collision with the bounds of the window
        public void Collision()
        {
            // Switches X power
            if (X + position.Width > Game1.width || X + position.Width + movement.X * power.X > Game1.width)
            {
                power.X = -power.X;
            }
            if (X < 0 || X + movement.X * power.X < 0)
            {
                power.X = -power.X;
            }

            // Switches Y power
            if (Y + position.Height > Game1.height || Y + position.Height + movement.Y * power.Y > Game1.height)
            {
                power.Y = -power.Y;
            }
            if (Y < 0 || Y + movement.Y * power.Y < 0)
            {
                power.Y = -power.Y;
            }

            if (power.X < .1f && power.X > -.1f || power.Y < .1f && power.Y > -.1f)
            {
                power = new Vector2(0, 0);
            }
        }

        /// <summary>
        /// Passes in a rectangle which then only checks the X coords of the ball
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        public bool CheckCollisionX(Rectangle wall, bool bounce)
        {
            bool flag = false;
            int num = 2;

            if (new Rectangle((int)(X + movement.X * power.X), (int)Y, position.Width, position.Height).Intersects(wall) ||
                new Rectangle((int)X, (int)Y, position.Width, position.Height).Intersects(wall))
            {
                // Right wall collision check
                if (X + movement.X * power.X <= wall.X + wall.Width || X <= wall.X + wall.Width)
                {
                    if (bounce)
                    {
                        while (X > wall.X + wall.Width)
                        {
                            X--;
                        }
                        power.X = -power.X;

                        X += movement.X * (power.X / num);
                    }

                    flag = true;
                }

                // Left wall Collision Check
                else if (X + (movement.X * power.X) + position.Width >= wall.X || X + position.Width >= wall.X)
                {
                    if (bounce)
                    {
                        while (X + position.Width < wall.X)
                        {
                            X++;
                        }
                        power.X = -power.X;

                        X += movement.X * (power.X / num);
                    }

                    flag = true;
                }
            }

            return flag;
        }

        /// <summary>
        /// Passes in a rectangle which then only checks the Y coords of the ball
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        public bool CheckCollisionY(Rectangle wall, bool bounce)
        {
            bool flag = false;
            int num = 2;

            if (new Rectangle((int)X, (int)(Y + movement.Y * power.Y), position.Width, position.Height).Intersects(wall) ||
                new Rectangle((int)X, (int)Y, position.Width, position.Height).Intersects(wall))
            {
                // Top Collision check
                if (Y + (movement.Y * power.Y) <= wall.Y + wall.Height || Y <= wall.Y + wall.Height)
                {
                    if (bounce)
                    {
                        while (Y > wall.Y + wall.Height)
                        {
                            Y--;
                        }
                        power.Y = -power.Y;

                        Y += movement.Y * (power.Y / num);
                    }

                    flag = true;
                }

                // Bottom Collision Check
                else if (Y + (movement.Y * power.Y) + position.Height >= wall.Y || Y + position.Height >= wall.Y)
                {
                    if (bounce)
                    {
                        while (Y + position.Height < wall.Y)
                        {
                            Y++;
                        }
                        power.Y = -power.Y;

                        Y -= movement.Y * (power.Y / num);
                    }

                    flag = true;
                }
            }

            return flag;
        }

        /*
        // Origin of ball
        Vector2 startPos = new Vector2((X + position.Width / 2), (Y + position.Height / 2));

        // Magnified direction
        Vector2 endPos = startPos + movement * power;

        // Top
        if (CheckLineCollision(startPos, endPos,
            new Vector2(wall.Position.X, wall.Position.Y),
            new Vector2(wall.Position.X + wall.Position.Width, wall.Position.Y)))                            // x,y => x+w,y
        {
            power.Y = -power.Y;
        }

        // Right side
        if (CheckLineCollision(startPos, endPos,
            new Vector2(wall.Position.X + wall.Position.Width, wall.Position.Y),
            new Vector2(wall.Position.X + wall.Position.Width, wall.Position.Y + wall.Position.Height)))     // x+w,y => x+w,y+h
        {
            power.X = -power.X;
        }

        // Bottom
        if (CheckLineCollision(startPos, endPos,
            new Vector2(wall.Position.X + wall.Position.Width, wall.Position.Y + wall.Position.Height),
            new Vector2(wall.Position.X, wall.Position.Y + wall.Position.Height)))                           // x+w,y+h => x,y+h
        {
            power.Y = -power.Y;
        }

        // Left side
        if (CheckLineCollision(startPos, endPos,
            new Vector2(wall.Position.X, wall.Position.Y + wall.Position.Height),
            new Vector2(wall.Position.X, wall.Position.Y)))                                                  // x,y+h => x,y)
        {
            power.X = -power.X;
        }

    }

    /// <summary>
    /// Checks for a collision between two lines
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="startPosRect"></param>
    /// <param name="endPosRect"></param>
    /// <returns></returns>
    public bool CheckLineCollision(Vector2 startPos, Vector2 endPos, Vector2 startPosRect, Vector2 endPosRect)
    {
        bool flag = false;
        float
            l1_x, l1_y, l2_x, l2_y, // Line start and end point
            r1_x, r1_y, r2_x, r2_y, // Rectangle wall start and end point
            L_x, L_y,               // Line
            R_x, R_y,               // Rectangle Line
            s, t;                   // Scalers

        // Line             // Top of rectangle
        l1_x = startPos.X; r1_x = startPosRect.X;
        l1_y = startPos.Y; r1_y = startPosRect.Y;
        l2_x = endPos.X; r2_x = endPosRect.X;
        l2_y = endPos.Y; r2_y = endPosRect.Y;

        // Actual vector points for our two lines
        L_x = l2_x - l1_x; L_y = l2_y - l1_y;
        R_x = r2_x - r1_x; R_y = r2_y - r1_y;

        // Math for the scalers of our two lines
        t = ((L_x * (l1_y - r1_y)) - (L_y * (l1_x - r1_x))) / ((L_x * R_y) - (R_x * L_y));
        s = ((R_x * (l1_y - r1_y)) - (R_y * (l1_x - r1_x))) / ((L_x * R_y) - (R_x * L_y));

        if ((s >= 0 && s <= 1) && (t >= 0 && t <= 1))
        {
            // Top side intersection
            if (startPosRect.X == position.X && startPosRect.Y == position.Y)
            {
                intersection1.X = l1_x + (s * L_x);
                intersection1.Y = l1_y + (s * L_y);

                // Concept to make it not travel through blocks
                // Might have to place elsewhere, and not in checkline collision
                //if (Vector2.Distance(movement*power,intersection1) > Vector2.Distance(positionV, intersection1))
                //{
                //    positionV = intersection1;
                //    power.Y = -power.Y;
                //}
            }

            // Right side intersection
            if (startPosRect.X == position.X + position.Width && startPosRect.Y == position.Y)
            {
                intersection2.X = l1_x + (s * L_x);
                intersection2.Y = l1_y + (s * L_y);

                power = positionV - intersection2;
            }

            // Bottom side intersection
            if (startPosRect.X == position.X + position.Width && startPosRect.Y == position.Y + position.Height)
            {
                intersection3.X = l1_x + (s * L_x);
                intersection3.Y = l1_y + (s * L_y);

                power = positionV - intersection3;
            }

            // Left side interaction
            if (startPosRect.X == position.X && startPosRect.Y == position.Y + position.Height)
            {
                intersection4.X = l1_x + (s * L_x);
                intersection4.Y = l1_y + (s * L_y);

                power = positionV - intersection4;
            }
            flag = true;
        }

        return flag;
    }*/

    }
}
