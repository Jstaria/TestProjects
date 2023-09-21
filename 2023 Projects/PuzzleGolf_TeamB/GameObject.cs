using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Base game object class, standard texture and position

namespace PuzzleGolf_TeamB
{
    internal class GameObject
    {
        // variables for the texture and position
        protected Texture2D asset;
        protected Rectangle position;
        protected Vector2 positionV;
        //protected Vector2 origin;
        protected float angle;

        /// <summary>
        /// Position including vector + size
        /// </summary>
        public Rectangle Position
        {
            get { return position; }
        }

        public Vector2 PositionV
        {
            get { return positionV; }
            set { positionV = value; }
        }

        /// <summary>
        /// Position vectorss X component
        /// </summary>
        public float X
        {
            get { return positionV.X; }
            set { positionV.X = value; }
        }

        /// <summary>
        /// Position vector's Y component
        /// </summary>
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

        /// <summary>
        /// Draws without rotation in mind, only needs positionV updated
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, new Rectangle((int)X,(int)Y,position.Width,position.Height), Color.White);
        }

        /// <summary>
        /// Draws with rotation in mind, needs:
        /// PositionV updated,
        /// Source rectangle (for animation)
        /// Origin: where it rotates around
        /// Angle at which to be drawn
        /// Spriteeffect, and the layer (IDK what this does)
        /// </summary>
        /// <param name="sb"></param>
        public virtual void DrawRotation(SpriteBatch sb, Vector2 mousePos, Rectangle source, GolfBall ball)
        {
            // Needs direction, fortunately in ball, we have a calculated direction already
            // Current direction facing the mouse is already done
            Vector2 direction = mousePos;

            // Angle can be calculated via using arc tan
            angle = (float)Math.Atan2(direction.Y, direction.X);

            // And origin is just the zero vector apparently

            sb.Draw(
                asset, 
                new Vector2(
                    ball.X + ball.position.Width / 2, 
                    ball.Y + ball.position.Height / 2), 
                source, 
                new Color(100,100,100,50), 
                angle - (float)Math.PI / 4, 
                Vector2.Zero /* origin */, 
                1, 
                SpriteEffects.None, 
                0);
        }

        public virtual void Update(GameTime gameTime) { }
    }
}
