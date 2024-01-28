using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FallingSand
{
    internal class SandParticle
    {
        private Point position;
        private Color color;
        private int scale;
        private bool isActive = false;

        public bool IsActive { get { return isActive; } set { isActive = value; } }

        public Point Position { get { return position; } set { position = value; } }

        public SandParticle()
        {
            this.position = new Point(-1000,-1000);
            this.color = Color.Wheat;
            this.scale = 0;
            this.isActive = false;
        }

        public SandParticle(Point position, Color color, int scale)
        {
            this.position = position;
            this.color = color;
            this.scale = scale;
            this.isActive = true;
        }

        public void Draw()
        {
            if (!IsActive) return;
            ShapeBatch.Box(new Rectangle(position, new Point(scale, scale)), color);
        }
    }
}
