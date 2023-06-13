using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Particles
{
    internal class Particle
    {
        private Vector2 position;
        private Color particleColor;
        private Texture2D asset;
        private float assetScaler;
        private bool isActive;
        private float rotation;
        private float size;
        private Random random;
        private float rotationSpeed;

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Particle (Vector2 position, Texture2D asset, float assetScaler, Color particleColor, float rotation, float size, float rotationSpeed)
        {
            this.position = position;
            this.asset = asset;
            this.assetScaler = assetScaler;
            this.particleColor = particleColor;
            this.isActive = false;
            this.rotation = rotation;
            this.size = size;
            this.random = new Random();
            this.rotationSpeed = rotationSpeed;
        }

        public void Update()
        {
            particleColor.A -= (byte).1f;

            size *= .96f;
            
            rotation += rotationSpeed;
            
            
        }

        public void Draw(GraphicsDevice GraphicsDevice)
        {
            ShapeBatch.Begin(GraphicsDevice);

            ShapeBatch.Triangle(position, size, rotation, particleColor);

            ShapeBatch.End();
        }
    }
}
