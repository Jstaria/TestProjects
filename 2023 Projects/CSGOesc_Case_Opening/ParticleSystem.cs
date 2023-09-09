using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    internal class ParticleSystem
    {
        private List<Particle> particles;
        private Rectangle position;
        private Random random;

        private int particleAmount;
        private float decaySpeed;
        private Color startColor;
        private Color endColor;
        private Texture2D asset;
        private int speed;
        private float rotationSpeed;
        private bool doesRotate;
        private float spreadAngle;
        private float spawnAmount;

        public ParticleSystem(int particleAmount, Color startColor, Color endColor, float decaySpeed, Texture2D asset, Rectangle position, int speed, bool doesRotate, float spreadAngle)
        {
            this.particleAmount = particleAmount;
            this.decaySpeed = decaySpeed;
            this.asset = asset;
            this.startColor = startColor;
            this.endColor = endColor;
            this.speed = speed;

            this.position = position;
            this.particles = new List<Particle>();
            this.random = new Random();
            this.doesRotate = doesRotate;
            this.rotationSpeed = 0;
            this.spreadAngle = spreadAngle;
            this.spawnAmount = particleAmount / 500;
        }

        public void Update()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();

                if (particles[i].LifeSpan == 0)
                {
                    particles.RemoveAt(i);

                    i--;
                }
            }

            if (particles.Count < particleAmount)
            {
                if (doesRotate)
                {
                    rotationSpeed = random.Next(-5, 5) * .01f;

                    for (int i = 0; i < spawnAmount; i++)
                    {
                        particles.Add(new Particle(new Vector2(random.Next(position.X, position.X + position.Width + 1), random.Next(position.Y, position.Y + position.Height + 1)), decaySpeed, asset, startColor, endColor, speed, rotationSpeed, random.Next((int)spreadAngle), doesRotate));
                    }
                }

                else
                {
                    particles.Add(new Particle(new Vector2(random.Next(position.X, position.X + position.Width + 1), random.Next(position.Y, position.Y + position.Height + 1)), decaySpeed, asset, startColor, endColor, speed, random.Next((int)spreadAngle)));
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle particle in particles)
            {
                particle.DrawAsset(sb);
            }
        }
    }
}
