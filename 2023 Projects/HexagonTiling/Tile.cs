using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexagonTiling
{
    internal class Tile
    {
        private Vector2 origin;
        private Rectangle position;
        private Texture2D texture;

        private bool isInCollision;

        public bool IsInCollision
        {
            get { return isInCollision; }
            set { isInCollision = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
        }

        public Rectangle Position
        {
            get { return position; }
        }

        public Tile(Vector2 position, Texture2D texture, int size)
        {
            this.position = new Rectangle((int)position.X, (int)position.Y, size, size);
            this.texture = texture;
            this.origin = new Vector2(
                this.position.X + size / 2,
                this.position.Y + size / 2);

            isInCollision = false;
        }

        public void Draw(SpriteBatch sb, Effect effect, double time)
        {
            if(isInCollision)
            {
                sb.End();
                sb.Begin(SpriteSortMode.Immediate, effect: effect);

                effect.Parameters["time"].SetValue((float)time);
                effect.CurrentTechnique.Passes[0].Apply();

                sb.Draw(texture, position, null, Color.Gray, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 1);

                sb.End();
                sb.Begin();
            }
            else
            {
                sb.Draw(texture, position, null, Color.DarkGoldenrod, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 1);
            }
            
        }
    }
}
