using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AfterImage
{
    internal class Particle
    {
        public Texture2D asset;
        public Point position;
        public float angle;
        public float lifeSpan;
        public float decaySpeed;
        public float sizeDecay;
        public float floatNum;
        private float scale;
        private Vector2 originOffset;

        public Particle(Texture2D asset, Point position, float angle, float lifeSpan, float decaySpeed, float sizeDecay)
        {
            this.asset = asset;
            this.position = position;
            this.angle = angle;
            this.lifeSpan = lifeSpan;
            this.decaySpeed = decaySpeed;
            this.sizeDecay = sizeDecay;
            this.floatNum = .5f;
            this.originOffset = Vector2.Zero;

            scale = .5f;
        }

        public void Update()
        {
            scale *= 1.05f;

            originOffset += new Vector2(.9f, .85f);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, new Vector2(position.X, position.Y), null, Color.Pink * floatNum, 0, originOffset, scale, SpriteEffects.None, 0);
        }
    }
}
