using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadow
{
    internal class Object
    {
        protected Rectangle position;
        protected Texture2D asset;

        public Vector2 PositionV
        {
            get { return new Vector2(position.X, position.Y); }
            set { position = new Rectangle((int)value.X, (int)value.Y, position.Width, position.Height); }
        }
        
        public Rectangle Position
        {
            get { return position; }
        }

        public Object (Rectangle position, Texture2D asset)
        {
            this.position = position;
            this.asset = asset;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.White);
        }

        public virtual void Update() { }
    }
}
