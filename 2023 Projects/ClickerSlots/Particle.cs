using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Simple particle for particle systems or one off text

namespace ClickerSlots
{
    internal class Particle
    {
        private Vector2 position;
        private float decayRate;

        private Texture2D asset;
        private Color startColor;
        private Color endColor;
        private float rotationSpeed;
        private float rotationAngle;
        private float colorDecay;

        private const int def = 1;
        private int speed;
        private string amount;
        private float size;
        private bool isRadial;
        private float spreadAngle;

        private SpriteFont particleFont;

        public float LifeSpan { get; private set; }

        // Because it is only me using this particular particle system, I will not comment out each ad every one of these
        public Particle(Vector2 postiion, float decayRate, string amount, SpriteFont particleFont)
        {
            this.particleFont = particleFont;
            this.position = postiion;
            this.LifeSpan = 1;
            this.decayRate = decayRate;
            this.amount = amount;
            this.speed = def;
            this.rotationSpeed = 0;
            this.rotationAngle = 0;
            this.size = 1;
        }

        public Particle(Vector2 postiion, float decayRate, Texture2D asset, Color color)
        {
            this.position = postiion;
            this.LifeSpan = 1;
            this.decayRate = decayRate;
            this.asset = asset;
            this.startColor = color;
            this.endColor = color;
            this.speed = def;
            this.rotationSpeed = 0;
            this.rotationAngle = 0;
            this.size = 1;
        }

        public Particle(Vector2 postiion, float decayRate, Texture2D asset, Color startColor, Color endColor)
        {
            this.position = postiion;
            this.LifeSpan = 1;
            this.decayRate = decayRate;
            this.asset = asset;
            this.startColor = startColor;
            this.endColor = endColor;
            this.speed = def;
            this.rotationSpeed = 0;
            this.rotationAngle = 0;
            this.size = 1;
        }

        public Particle(Vector2 postiion, float decayRate, Texture2D asset, Color startColor, Color endColor, int speed)
        {
            this.position = postiion;
            this.LifeSpan = 1;
            this.decayRate = decayRate;
            this.asset = asset;
            this.startColor = startColor;
            this.endColor = endColor;
            this.speed = speed;
            this.rotationSpeed = 0;
            this.rotationAngle = 0;
            this.size = 1;
        }

        public Particle(Vector2 postiion, float decayRate, Texture2D asset, Color startColor, Color endColor, int speed, float rotationSpeed)
        {
            this.position = postiion;
            this.LifeSpan = 1;
            this.decayRate = decayRate;
            this.asset = asset;
            this.startColor = startColor;
            this.endColor = endColor;
            this.speed = speed;
            this.rotationSpeed = rotationSpeed;
            this.rotationAngle = 0;
            this.size = 1;
        }

        public Particle(Vector2 postiion, float decayRate, Texture2D asset, Color startColor, Color endColor, int speed, float rotationSpeed, float spreadAngle, bool isRadial)
        {
            this.position = postiion;
            this.LifeSpan = 1;
            this.decayRate = decayRate;
            this.asset = asset;
            this.startColor = startColor;
            this.endColor = endColor;
            this.speed = speed;
            this.rotationSpeed = rotationSpeed;
            this.rotationAngle = 0;
            this.size = 1;
            this.spreadAngle = spreadAngle - 135;
            this.isRadial = isRadial;
            this.colorDecay = 1f;
        }
         
        public void Update()
        {
            if (isRadial)
            {
                position += new Vector2(
                    (float)(Math.Cos(MathHelper.ToRadians(spreadAngle)) - Math.Sin(MathHelper.ToRadians(spreadAngle))) * speed,
                    (float)(Math.Sin(MathHelper.ToRadians(spreadAngle)) + Math.Cos(MathHelper.ToRadians(spreadAngle))) * speed);
            }
            else
            {
                position.Y -= speed;
            }

            if (LifeSpan > .005f)
            {
                LifeSpan *= decayRate;
            }
            
            else
            {
                LifeSpan = 0;
            }

            size *= .995f;
            colorDecay *= (new Random()).Next(970,985) * .001f;

            rotationAngle += rotationSpeed;
        }

        public void DrawString(SpriteBatch sb)
        {
            sb.DrawString(particleFont, amount, position - Game1.ReadOut.MeasureString(amount) / 2, Color.Black * LifeSpan);
        }

        public void DrawAsset(SpriteBatch sb)
        {
            sb.Draw(asset, position, null, Color.Lerp(endColor, startColor, colorDecay) * LifeSpan, rotationAngle, new Vector2(asset.Width / 2, asset.Height / 2), size, SpriteEffects.None, 0);
        }
    }
}
