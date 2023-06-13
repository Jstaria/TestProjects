using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Particles
{
    internal class ParticleSystem
    {
        private int particleCount;
        private int count;
        private Particle[] particles;
        private Random random;
        private Vector2 position;
        private float size;
        private float speed;
        private float topLimit;

        public ParticleSystem(int particleNum, Vector2 position, Texture2D asset, float assetScaler, Color particleColor, float size)
        {
            this.position = position;

            random = new Random();

            particles = new Particle[particleNum];

            for (int i = 0; i < particleNum; i++)
            {
                particles[i] = new Particle(
                    new Vector2(position.X + random.Next(-10,10), 
                    position.Y + random.Next(-5,5)), 
                    asset, 
                    assetScaler, 
                    particleColor, 
                    (float)random.NextDouble()*10, 
                    size, 
                    (float)random.NextDouble() / 10);
            }

            particleCount = 0;
            count = 0;

            this.size = size;

            speed = 1;

            topLimit = 100;


        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();

            //position = new Vector2(mouseState.X, mouseState.Y);

            if (count % (speed*8) == 0)
            {
                particles[particleCount].IsActive = true;
                if (particleCount < particles.Length - 1)
                {
                    particleCount++;
                }
                else
                {
                    particleCount = 0;
                }
            }

            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].IsActive)
                {
                    particles[i].Update();
                }
                if (particles[i].Position.Y < position.Y - topLimit)
                {
                    particles[i].Position = new Vector2(particles[i].Position.X, position.Y);
                    particles[i].Size = size;
                }
            }

            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].IsActive)
                {
                    particles[i].Position += new Vector2(0, -speed);
                }
            }
            count++;
        }

        public void Draw(GraphicsDevice GraphicsDevice)
        {
            foreach (Particle particle in particles)
            {
                if (particle.IsActive)
                {
                    particle.Draw(GraphicsDevice);
                }
            }
        }
    }
}
