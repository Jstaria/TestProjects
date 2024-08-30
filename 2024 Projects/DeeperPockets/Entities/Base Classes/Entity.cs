using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets.Entities
{
    internal abstract class Entity
    {
        protected Vector2 pos;
        protected Vector2 drawPos;

        protected Texture2D tex;

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch sb);

        public Entity(Vector2 pos, Texture2D tex)
        {
            this.pos = pos;
            this.tex = tex;
        }
    }
}
