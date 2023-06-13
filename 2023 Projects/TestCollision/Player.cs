using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

// Moveable player class used with collision checking

namespace TestCollision
{
    internal class Player : GameObject
    {
        private int levelScore;
        private int totalScore;
        private int width;
        private int height;
        private int speed;

        /// <summary>
        /// Returns the score for just the level you are on
        /// </summary>
        public int LevelScore
        {
            get { return levelScore; }
            set { levelScore = value; }
        }

        /// <summary>
        /// Returns the score since the beginning of the game
        /// </summary>
        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }

        // players speed, can be affected by the speed pickup
        public int Speed
        {
            set { speed = value; }
        }

        public Player(Texture2D asset, Rectangle position, int width, int height) : base(asset, position)
        {
            this.levelScore = 0;
            this.totalScore = 0;
            this.width = width;
            this.height = height;
            this.speed = 5;
            this.X = position.X;
            this.Y = position.Y;

        }

        // Player Input
        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W))
            {
                if (Y < -50)
                {
                    Y = height-50;
                }
                Y -= speed;

            }
            if (kb.IsKeyDown(Keys.S))
            {
                if (Y > height)
                {
                    Y = -50;
                }
                Y += speed;
            }

            if (kb.IsKeyDown(Keys.A))
            {
                if (X < -50)
                {
                    X = width-50;
                }
                X -= speed;
            }

            if (kb.IsKeyDown(Keys.D))
            {
                if (X > width)
                {
                    X = -50;
                }
                X += speed;
            }
        }
        /// <summary>
        /// Resets the location of the player to the center
        /// </summary>
        public void Center()
        {
            positionV.X = width / 2;
            positionV.Y = height / 2;
        }
    }
}
