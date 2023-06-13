using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Simple sprite class that manages a sprite

namespace TestCollision
{
    internal class Sprite
    {
        private Texture2D asset;

        public Sprite(Texture2D asset)
        {
            this.asset = asset;
        }

        public virtual void Draw(Vector2 pos)
        {
            Globals.SpriteBatch.Draw(asset, pos, Color.White);
        }
    }
}
