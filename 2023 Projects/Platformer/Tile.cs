using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    internal class Tile
    {
        private Texture2D asset;
        private Rectangle position;

        // Used in hitbox creation
        private List<Vector2> points;
        private Polygon hitbox;
        
        /// <summary>
        /// Walkable tile
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="position"></param>
        public Tile(Texture2D asset, Rectangle position)
        {
            this.asset = asset;
            this.position = position;
            points = new List<Vector2>()
            {
                new Vector2(position.X,                  position.Y),
                new Vector2(position.X + position.Width, position.Y),
                new Vector2(position.X + position.Width, position.Y + position.Height),
                new Vector2(position.X                 , position.Y + position.Height)
            };

            hitbox = new Polygon(points, new Vector2(position.X + position.Width / 2, position.Y + position.Height / 2));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(asset, position, Color.White);
        }
    }
}
