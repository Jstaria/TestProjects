using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Our objects to be collided with

namespace TestCollision
{
    internal class Square
    {
        Rectangle position;
        Texture2D asset;
        Vector2 intersection1;
        Vector2 intersection2;
        Vector2 intersection3;
        Vector2 intersection4;
        bool[] flags;

        // A "List" of all intersections for the sides of a rectangle
        public Vector2 Intersection1
        {
            get { return intersection1; }
        }

        public Vector2 Intersection2
        {
            get { return intersection2; }
        }

        public Vector2 Intersection3
        {
            get { return intersection3; }
        }

        public Vector2 Intersection4
        {
            get { return intersection4; }
        }

        /// <summary>
        /// An array of bools for the flags indicating each intersection
        /// </summary>
        public bool[] Flags
        {
            get { return flags; }
        }

        public Square(Rectangle position, Texture2D asset)
        {
            this.position = position;
            this.asset = asset;
            intersection1 = new Vector2(0, 0);
        }

        /// <summary>
        /// Checks collision between a line and the appropriate square object (this)
        /// </summary>
        /// <param name="startPos">Lines start pos</param>
        /// <param name="endPos">Lines end pos</param>
        /// <returns></returns>
        public bool[] CheckCollision(Vector2 startPos, Vector2 endPos)
        {
            // The check for the rectangle is actually just checking for each side (line) of the rectangle 
            flags = new bool[] {false, false, false, false, false};

            // Top
            if (CheckLineCollision(startPos, endPos, new Vector2(position.X, position.Y), new Vector2(position.X + position.Width, position.Y)))                                        // x,y => x+w,y
            {
                flags[0] = true;
                flags[1] = true;
            }

            // Right side
            if (CheckLineCollision(startPos, endPos, new Vector2(position.X + position.Width, position.Y), new Vector2(position.X + position.Width, position.Y + position.Height)))     // x+w,y => x+w,y+h
            {
                flags[0] = true;
                flags[2] = true;
            }

            // Bottom
            if (CheckLineCollision(startPos, endPos, new Vector2(position.X + position.Width, position.Y + position.Height), new Vector2(position.X, position.Y + position.Height)))    // x+w,y+h => x,y+h
            {
                flags[0] = true;
                flags[3] = true;
            }

            // Left side
            if (CheckLineCollision(startPos, endPos, new Vector2(position.X, position.Y + position.Height), new Vector2(position.X, position.Y)))                                       // x,y+h => x,y)
            {
                flags[0] = true;
                flags[4] = true;
            }
           
            return flags;
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
                }
                
                // Right side intersection
                if (startPosRect.X == position.X + position.Width && startPosRect.Y == position.Y)
                {
                    intersection2.X = l1_x + (s * L_x);
                    intersection2.Y = l1_y + (s * L_y);
                }

                // Bottom side intersection
                if (startPosRect.X == position.X + position.Width && startPosRect.Y == position.Y + position.Height)
                {
                    intersection3.X = l1_x + (s * L_x);
                    intersection3.Y = l1_y + (s * L_y);
                }

                // Left side interaction
                if (startPosRect.X == position.X && startPosRect.Y == position.Y + position.Height)
                {
                    intersection4.X = l1_x + (s * L_x);
                    intersection4.Y = l1_y + (s * L_y);
                }
                flag = true;
            }

            return flag;
        }

        // Simple draw for Square
        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(this.asset, position, color);
        }
    }
}
