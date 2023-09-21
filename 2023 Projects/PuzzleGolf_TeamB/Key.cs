using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Unused in final build

namespace PuzzleGolf_TeamB
{
    internal class Key
    {
        private Rectangle position;
        private Texture2D asset;
        private GolfBall ball;
        private bool isCollected;

        public bool IsCollected
        {
            get { return isCollected; }
        }

        public Key(Rectangle position, Texture2D asset, GolfBall ball)
        {
            this.position = position;
            this.asset = asset;
            this.ball = ball;
        }

        public void Collect()
        {
            if (ball.CheckCollisionX(position, false) ||
                ball.CheckCollisionY(position, false))
            {
                isCollected = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.Beige);
        }
    }
}
