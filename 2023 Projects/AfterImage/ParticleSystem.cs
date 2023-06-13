using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AfterImage
{
    internal class ParticleSystem
    {
        private Player player;

        private Particle[] particles;
        private float[] angles;

        private Texture2D asset;
        private Point position;
        private float angle;
        private float lifeSpan;
        private float decaySpeed;
        private float sizeDecay;
        private int count;

        public int speed;

        public ParticleSystem (Texture2D asset, Point position, float angle, float lifeSpan, float decaySpeed, float sizeDecay, Player player)
        { 
            this.player = player;

            particles = new Particle[20];

            this.asset = asset;
            this.position = position;
            this.angle = angle;
            this.lifeSpan = lifeSpan;
            this.decaySpeed = decaySpeed;
            this.sizeDecay = sizeDecay;

            count = 0;
        }

        /// <summary>
        /// Relays info for after image
        /// </summary>
        public void ParticleUpdate(GameTime gameTime)
        {
            if ((int)gameTime.TotalGameTime.TotalMilliseconds % speed == 0)
            {
                particles[count] = new Particle(asset, new Point((int)player.Position.X, (int)player.Position.Y), angle, lifeSpan, decaySpeed, sizeDecay);

                count++;
            }
            

            if (count > particles.Length - 1)
            {
                count = 0;
            }

            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null)
                {
                    if (Game1.state && player.speedX == 0 && player.speedY == 0)
                    {
                        particles[i].floatNum = 0;
                    }
                    particles[i].floatNum -= .025f;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle position in particles)
            {
                if (position != null)
                {
                    //position.Update();
                    position.Draw(sb);
                }
            }
        }
    }
}
