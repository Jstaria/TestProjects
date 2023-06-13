using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadow
{
    internal class Wall : Object
    {
        private bool hasShadow;

        /// <summary>
        /// Returns walls posiiton
        /// </summary>
        public Rectangle Position
        {
            get { return this.position; }
        }

        /// <summary>
        /// Get or set whether the wall has a shadow
        /// </summary>
        public bool HasShadow
        {
            get { return hasShadow; }
            set { hasShadow = value; }
        }

        public Wall(Rectangle position, Texture2D asset) : base(position, asset)
        {
            this.asset = asset;
            this.position = position;
            this.hasShadow = true;
        }

    }
}
