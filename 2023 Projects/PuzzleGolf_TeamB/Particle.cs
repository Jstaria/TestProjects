using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PuzzleGolf_TeamB
{
    internal class Particle
    {
        private Texture2D asset;
        private Vector2 position;
        //private float angle;
        //private float lifeSpan;
        private float decaySpeed;
        private float floatNum;
        private float sizeMod;
        //private float time;

        public float FloatNum
        {
            get { return floatNum; }
            set { floatNum = value; }
        }

        public Particle(Texture2D asset, Vector2 position, float angle, float lifeSpan, float decaySpeed)
        {
            this.asset = asset;
            this.position = position;
            //this.angle = angle;
            //this.lifeSpan = lifeSpan;
            this.decaySpeed = decaySpeed;
            this.floatNum = 1;
            this.sizeMod = 0;
        }

        public void Update(GameTime gameTime)
        {
            floatNum *= (decaySpeed - 1)/decaySpeed;

            if ((int)gameTime.TotalGameTime.TotalMilliseconds % 5 == 0)
            {
                sizeMod += .75f;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, effect: Game1.rainbow);

            Game1.rainbow.Parameters["time"].SetValue(GameManager.time);
            Game1.rainbow.CurrentTechnique.Passes[0].Apply();

            sb.Draw(
                asset, 
                new Rectangle(
                    (int)(position.X + sizeMod / 2), 
                    (int)(position.Y + sizeMod / 2), 
                    (int)(10 - sizeMod), 
                    (int)(10 - sizeMod)), 
                Color.DarkGray * floatNum);

            sb.End();
            sb.Begin();
        }
    }
}
