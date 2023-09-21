using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGolf_TeamB
{
    /// <summary>
    /// Produces a simple after image particle system
    /// </summary>
    internal class ParticleSystem
    {
        private GolfBall ball;

        private Particle[] particles;
        //private float[] angles;

        private Texture2D asset;
        private Vector2 position;
        private float angle;
        private float lifeSpan;
        private float decaySpeed;

        private int count;

        public float speed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="lifeSpan"></param>
        /// <param name="decaySpeed"></param>
        /// <param name="ball"></param>
        public ParticleSystem (Texture2D asset, Vector2 position, float angle, float lifeSpan, float decaySpeed, GolfBall ball)
        { 
            this.ball = ball;

            particles = new Particle[30];

            this.asset = asset;
            this.position = position;
            this.angle = angle;
            this.lifeSpan = lifeSpan;
            this.decaySpeed = decaySpeed;

            count = 0;
            speed = 1;
        }

        /// <summary>
        /// Relays info for after image
        /// </summary>
        public void ParticleUpdate(GameTime gameTime)
        {
            if ((int)gameTime.TotalGameTime.TotalMilliseconds % speed == 0)
            {
                particles[count] = new Particle(asset, ball.PositionV, angle, lifeSpan, decaySpeed);

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
                    particles[i].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle position in particles)
            {
                if (position != null)
                {
                    position.Draw(sb);
                }
            }
        }
    }
}
