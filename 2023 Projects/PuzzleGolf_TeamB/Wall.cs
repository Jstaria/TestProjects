using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGolf_TeamB
{
    internal class Wall : GameObject
    {
        private bool isBreakable;
        private bool isBroken;
        private bool isMoveable;
        private int movement;
        private string direction;
        private Vector2 speed;
        int traveled;
        private Color tint;

        /// <summary>
        /// Public direction for checking collision
        /// </summary>
        public string Direction
        {
            get { return direction; }
        }

        public Color Tint 
        { 
            get { return tint; } 
        }

        public Rectangle FuturePosition
        {
            get 
            {
                return new Rectangle(position.X + (int)speed.X, position.Y + (int)speed.Y, position.Width, position.Height);
            }
        }

        /// <summary>
        /// Returns if the wall is broken
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set
            {
                if (isBreakable)
                {
                    isBroken = value;
                }
            }
        }

        /// <summary>
        /// Returns if the wall is breakable
        /// </summary>
        public bool IsBreakable
        {
            get { return isBreakable; }
        }

        /// <summary>
        /// Returns if the wall is movable
        /// </summary>
        public bool IsMovable
        {
            get { return isMoveable; }
        }

        public Wall(bool isBreakable,bool isMoveable,string direction, int x, int y, int screenH, Texture2D texture, Color tint)
            : base(texture, new Rectangle(x, y, screenH / 25, screenH / 25))
        {
            this.isBreakable = isBreakable;
            this.isMoveable = isMoveable;
            this.direction = direction;
            this.isBroken = false;
            this.tint = tint;

            speed = Vector2.Zero;
            movement = 1;
            traveled = 0;
        }
        
        /// <summary>
        /// makes the wall size based on the size of the screen
        /// </summary>
        public void WallSize(int windowHeig)
        {
            position.Width = windowHeig / 25;
            position.Height = windowHeig / 25;

        }


        /// <summary>
        /// Movement for the wall
        /// </summary>
        public void WallMove(GolfBall ball)
        {
            switch(direction)
            {
                case "up":

                    position.Y -= movement;
                    traveled += 1;

                    speed.Y = 1;

                    if (ball.CheckCollisionY(position, true))
                    {
                        ball.Y += movement * 1.5f;
                    }

                    break;

                case "down":

                    position.Y += movement;
                    traveled += 1;

                    speed.Y = -1;

                    if (ball.CheckCollisionY(position, true))
                    {
                        ball.Y -= movement * 1.5f;
                    }

                    break;

                case "right":

                    position.X += movement;
                    traveled += 1;

                    speed.X = 1;

                    if (ball.CheckCollisionX(position, true))
                    {
                        ball.X -= movement * 1.5f;
                    }

                    break;

                case "left":

                    position.X -= movement;
                    traveled += 1;

                    speed.X = -1;

                    if (ball.CheckCollisionX(position, true))
                    {
                        ball.X += movement * 1.5f;
                    }

                    break;
            }

            if (traveled == 125)
            {
                movement *= -1;
                traveled = 0;

                speed = -speed;
            }
        }

        /// <summary>
        /// Draws the wall based of the spritebach put in.
        /// </summary>
        /// <param name="Sb"></param>
        /// <param name="tint"></param>
        public void DrawWall(SpriteBatch sb)
        {
            sb.Draw(asset, position, tint);
        }

    }
}
