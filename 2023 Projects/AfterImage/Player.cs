using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Base game object class, standard texture and position

namespace AfterImage
{
    internal class Player
    {
        public float speedY;
        public float speedX;

        private bool[] dashFlag;
        private bool[] justDashed;
        private float[] lastDash;
        private float[] lastPress;
        private int count;

        private KeyboardState prevKB;
        private KeyboardState check;

        // variables for the texture and position
        protected Texture2D asset;
        protected Vector2 position;

        /// <summary>
        /// origin of asset
        /// </summary>
        protected Vector2 origin;

        public Vector2 Origin
        {
            get { return origin; }
        }

        /// <summary>
        /// Position including vector + size
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D Asset
        {
            get { return asset; }
        }
        public Player(Texture2D asset, Vector2 position)
        {
            this.asset = asset;
            this.position = position;
            this.speedY = 0;
            this.speedX = 0;

            this.origin = new Vector2(position.X + asset.Width / 4, position.Y + asset.Height / 4);

            count = 0;
            lastPress = new float[] {0, 0, 0, 0};
            lastDash = new float[] {0, 0, 0, 0};
            dashFlag = new bool[] { false, false, false, false };
            justDashed = new bool[] { false, false, false, false };
        }

        public void Update(GameTime gameTime)
        {
            this.origin = new Vector2(position.X + asset.Width / 4, position.Y + asset.Height / 4);
            int speed = 5;
            float friction = .4f;
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W) && !dashFlag[0])
            {
                if (speedY > -speed)
                {
                    speedY--;
                }
                if (speedY < -speed)
                {
                    speedY++;
                }

            }
            if (kb.IsKeyDown(Keys.S) && !dashFlag[2])
            {
                if (speedY < speed)
                {
                    speedY++;
                }
                if (speedY > speed)
                {
                    speedY--;
                }
            }

            if (kb.IsKeyDown(Keys.A) && !dashFlag[1])
            {
                if (speedX > -speed)
                {
                    speedX--;
                }
                if (speedX < -speed)
                {
                    speedX++;
                }
            }

            if (kb.IsKeyDown(Keys.D) && !dashFlag[3])
            {
                if (speedX < speed)
                {
                    speedX++;
                }
                if (speedX > speed)
                {
                    speedX--;
                }
            }

            if (speedX > 0 && !kb.IsKeyDown(Keys.D) && !dashFlag[3])
            {
                speedX *= (speed - friction) / speed;
                if (speedX < .01)
                {
                    speedX = 0;
                }
            }

            else if (speedX < 0 && !kb.IsKeyDown(Keys.A) && !dashFlag[1])
            {
                speedX *= (speed - friction) / speed;
                if (speedX > -.01)
                {
                    speedX = 0;
                }
            }

            if (speedY > 0 && !kb.IsKeyDown(Keys.S) && !dashFlag[2])
            {
                speedY *= (speed - friction) / speed;
                if (speedY < .01)
                {
                    speedY = 0;
                }
            }

            else if (speedY < 0 && !kb.IsKeyDown(Keys.W) && !dashFlag[0])
            {
                speedY *= (speed - friction) / speed;
                if (speedY > -.01)
                {
                    speedY = 0;
                }
            }

            if (DoubleKey(kb, Keys.D, gameTime, 3) && !dashFlag[3])
            {
                float timeSinceLastDash = (float)gameTime.TotalGameTime.TotalSeconds - lastDash[3];

                if (timeSinceLastDash > 2f)
                {
                    justDashed[3] = false;
                }

                if (!justDashed[3])
                {
                    dashFlag[3] = true;
                    justDashed[3] = true;
                    lastDash[3] = (float)gameTime.TotalGameTime.TotalSeconds;
                }
                
                count = 0;
            }

            if (DoubleKey(kb, Keys.A, gameTime, 1) && !dashFlag[1])
            {
                float timeSinceLastDash = (float)gameTime.TotalGameTime.TotalSeconds - lastDash[1];

                if (timeSinceLastDash > 2f)
                {
                    justDashed[1] = false;
                }

                if (!justDashed[1])
                {
                    dashFlag[1] = true;
                    justDashed[1] = true;
                    lastDash[1] = (float)gameTime.TotalGameTime.TotalSeconds;
                }

                count = 0;
            }

            if (DoubleKey(kb, Keys.W, gameTime, 0) && !dashFlag[0])
            {
                float timeSinceLastDash = (float)gameTime.TotalGameTime.TotalSeconds - lastDash[0];

                if (timeSinceLastDash > 2f)
                {
                    justDashed[0] = false;
                }

                if (!justDashed[0])
                {
                    dashFlag[0] = true;
                    justDashed[0] = true;
                    lastDash[0] = (float)gameTime.TotalGameTime.TotalSeconds;
                }

                count = 0;
            }

            if (DoubleKey(kb, Keys.S, gameTime, 2) && !dashFlag[2])
            {
                float timeSinceLastDash = (float)gameTime.TotalGameTime.TotalSeconds - lastDash[2];

                if (timeSinceLastDash > 2f)
                {
                    justDashed[2] = false;
                }

                if (!justDashed[2])
                {
                    dashFlag[2] = true;
                    justDashed[2] = true;
                    lastDash[2] = (float)gameTime.TotalGameTime.TotalSeconds;
                }

                count = 0;
            }

            if (dashFlag[1] && speedX > -20)
            {
                speedX -= 5;
            }

            if (dashFlag[3] && speedX < 20)
            {
                speedX += 5;
            }

            if (dashFlag[0] && speedY > -20)
            {
                speedY -= 5;
            }

            if (dashFlag[2] && speedY < 20)
            {
                speedY += 5;
            }
            
            if (count > 2)
            {
                for (int i = 0; i < dashFlag.Length; i++)
                {
                    dashFlag[i] = false;
                }
                count = 0;
            }
            position += new Vector2(speedX, speedY);
            prevKB = kb;

            count++;

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, new Vector2(position.X, position.Y), null, Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
        }

        public bool DoubleKey(KeyboardState kb, Keys key, GameTime gameTime, int lastPressIndex)
        {
            bool flag = false;

            if (kb.IsKeyDown(key) && prevKB.IsKeyUp(key))
            {
                float timeSinceLastPress = (float)gameTime.TotalGameTime.TotalSeconds - lastPress[lastPressIndex];

                lastPress[lastPressIndex] = (float)gameTime.TotalGameTime.TotalSeconds;

                if (timeSinceLastPress < .3f)
                {
                    flag = true;
                }
            }

            return flag;
        }
    }
}
