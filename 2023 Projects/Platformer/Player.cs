using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    internal class Player
    {
        private Texture2D asset;
        private Rectangle position;

        // Used in hitbox creation
        private List<Vector2> points;
        private Polygon hitbox;
        private Rectangle checkBox;

        // Movement
        private Vector2 currentVelocity = Vector2.Zero;
        private float vertAccel = 2;

        // Jump variables
        private bool isJumping;
        private float timeSinceJump;

        private KeyboardState prevKBState;

        /// <summary>
        /// Walkable tile
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="position"></param>
        public Player(Texture2D asset, Rectangle position)
        {
            this.asset = asset;
            this.position = position;
            points = new List<Vector2>()
            {
                new Vector2(position.X,                  position.Y),
                new Vector2(position.X + position.Width, position.Y),
                new Vector2(position.X + position.Width, position.Y + position.Height),
                new Vector2(position.X                 , position.Y + position.Height)
            };

            hitbox = new Polygon(points, new Vector2(position.X + position.Width / 2, position.Y + position.Height / 2));
            checkBox = new Rectangle(position.X - 20, position.Y - 20, position.Width + 20, position.Height + 20);

            isJumping = false;
        }

        public void Update(GameTime gameTime)
        {
            

            KeyboardState currentKBState = Keyboard.GetState();

            // Moving left
            if (currentKBState.IsKeyDown(Keys.Right))
            {
                if (currentVelocity.X == 0)
                {
                    currentVelocity.X += 2;
                }
                
                currentVelocity = Vector2.Clamp(currentVelocity * 1.5f, Vector2.Zero, new Vector2(5, currentVelocity.Y));
            }
            else
            {
                currentVelocity = Vector2.Clamp(new Vector2(currentVelocity.X - 1, currentVelocity.Y), Vector2.Zero, new Vector2(5, currentVelocity.Y));
            }

            // Moving Right
            if (currentKBState.IsKeyDown(Keys.Left))
            {
                if (currentVelocity.X == 0)
                {
                    currentVelocity.X -= 2;
                }

                currentVelocity = Vector2.Clamp(currentVelocity * 1.5f, new Vector2(-5, currentVelocity.Y), Vector2.Zero);
            }
            else
            {
                currentVelocity = Vector2.Clamp(new Vector2(currentVelocity.X + 1, currentVelocity.Y), new Vector2(-5, currentVelocity.Y), Vector2.Zero);
            }

            // Jumping
            if (GetSinglePress(currentKBState, Keys.Up) && !isJumping)
            {
                timeSinceJump = (float)gameTime.TotalGameTime.TotalSeconds;
                currentVelocity.Y = 10f;
                vertAccel = -5;
                isJumping = true;
            }

            if (isJumping)
            {
                

                float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;

                int YDisplacement = (int)(this.position.Y + currentVelocity.Y * (currentTime - timeSinceJump) + .5f * vertAccel * Math.Pow(currentTime - timeSinceJump, 2));

                YDisplacement = (int)Vector2.Clamp(new Vector2(0, YDisplacement), Vector2.Zero, new Vector2(0, 10)).Y;
                System.Diagnostics.Debug.WriteLine(YDisplacement);

                currentVelocity.Y = (currentVelocity.Y + vertAccel * (currentTime - timeSinceJump)) / 2;

                if (position.Y + position.Height + YDisplacement > 400)
                {
                    position.Y = 400 - position.Height;
                    currentVelocity.Y = 0;
                    vertAccel = 0;
                    isJumping = false;
                }
                else
                {
                    this.position.Y += YDisplacement;
                }
            }

            // Falling
            if (currentKBState.IsKeyDown(Keys.Down))
            {
                if (currentVelocity.Y > 0)
                {
                    currentVelocity.Y -= 1;
                }
                else
                {
                    currentVelocity.Y *= 1.1f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(asset, position, Color.White);
        }

        private bool GetSinglePress(KeyboardState currentKBState, Keys key)
        {
            return currentKBState.IsKeyDown(key) && prevKBState.IsKeyUp(key);
        }
    }
}
