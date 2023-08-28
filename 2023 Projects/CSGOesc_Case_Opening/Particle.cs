using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    internal class Particle
    {
        private Vector2 position;
        private float decayRate;

        private string amount;

        public float LifeSpan { get; private set; }

        public Particle(Vector2 postiion, float decayRate, string amount)
        {
            this.position = postiion;
            this.LifeSpan = 1;
            this.decayRate = decayRate;
            this.amount = amount;
        }

        public void Update()
        {
            position.Y--;

            if (LifeSpan > .05f)
            {
                LifeSpan *= decayRate;
            }
            
            else
            {
                LifeSpan = 0;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(Game1.regular, amount, position - Game1.regular.MeasureString(amount) / 2, Color.Black * LifeSpan);
        }
    }
}
