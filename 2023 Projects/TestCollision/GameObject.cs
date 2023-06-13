using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Base game object class, standard texture and position

namespace TestCollision
{
    abstract internal class GameObject
    {
        // variables for the texture and position
        protected Texture2D asset;
        protected Rectangle position;
        protected Vector2 positionV;
        protected Vector2 origin;
        protected float angle;

        /// <summary>
        /// Position including vector + size
        /// </summary>
        public Rectangle Position
        {
            get { return position; }
        }

        public float X
        {
            get { return positionV.X; }
            set { positionV.X = value; }
        }

        public float Y
        {
            get { return positionV.Y; }
            set { positionV.Y = value; }
        }

        public GameObject(Texture2D asset, Rectangle position)
        {
            this.asset = asset;
            this.positionV = new Vector2(position.X, position.Y);
            this.position = position;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, new Rectangle((int)X,(int)Y,position.Width,position.Height), Color.White);
        }

        public virtual void DrawRotation(SpriteBatch sb)
        {
            sb.Draw(asset, this.positionV, null, Color.White, angle, origin, 1, SpriteEffects.None, 0);
        }

        public abstract void Update(GameTime gameTime);
    }
}
