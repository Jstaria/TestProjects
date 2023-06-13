using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AfterImage
{
    internal class Enemy
    {
        private Vector2 position;
        private Vector2 origin;
        private Texture2D asset;
        private List<Vector2> points;
        private float angle;

        private float width;

        public Vector2 Origin
        {
            get { return position; }
        }

        public List<Vector2> Points
        {
            get { return points; }
        }

        public float Angle
        {
            get { return angle; }
        }

        /// <summary>
        /// Creates an enemy with an origin, and then updates its position and its bounding box corners
        /// </summary>
        /// <param name="origin"> Center of shape</param>
        /// <param name="asset"> Asset </param>
        /// <param name="width"> hitbox width </param>
        /// <param name="height"> hitbox height </param>
        public Enemy(Vector2 origin, Texture2D asset, int width, int height)
        {
            this.width = width;
            this.angle = 0;
            this.origin = origin;
            this.position = new Vector2(origin.X - width / 2, origin.Y - height / 2);
            this.asset = asset;
            this.points = new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(+width, 0),
                new Vector2(+width, +height),
                new Vector2(0, +height),
            };
        }

        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(asset, position, null, color, angle, Vector2.Zero, width / asset.Width, SpriteEffects.None, 0);
        }

    }
}
