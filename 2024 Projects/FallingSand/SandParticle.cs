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
        private Color color;
        private int state;

        public Color Color { get { return color; } set { color = value; } }
        public int State { get { return state; } set { state = value; } }

        public SandParticle(Color color, int state)
        {
            this.color = color;
            this.state = state;
        }
    }
}
