using DeeperPockets.Interfaces;
using DeeperPockets.Libraries;
using DeeperPockets.Singletons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets
{
    internal class HitBox : IObject, IDebug
    {
        public Vector2 Position { get; set; }
        public Vector2 DrawPosition { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Size { get; set; }

        private Color color;
        private float LineThickness; // Not used right now

        public HitBox(Vector2 position, Vector2 size, Color color, float lineThickness)
        {
            this.Position = position;
            this.DrawPosition = position;
            this.Size = size;
            this.color = color;
            this.LineThickness = lineThickness;

            Debug.Instance.AddDebugObj(this);
        }

        public void Update()
        {
            DrawPosition = Position - Global.Instance.WorldOffset - Global.Instance.CameraOffset;
        }

        public bool Contains(Vector2 position, Vector2 size)
        {
            return
                DrawPosition.X < position.X + size.X && DrawPosition.X + Size.X > position.X &&
                DrawPosition.Y < position.Y + size.Y && DrawPosition.Y + Size.Y > position.Y;
        }
        public bool Intersects(HitBox hitBox)
        {
            Vector2 position = hitBox.Position;
            Vector2 size = hitBox.Size;

            return
                DrawPosition.X < position.X + size.X && DrawPosition.X + Size.X > position.X &&
                DrawPosition.Y < position.Y + size.Y && DrawPosition.Y + Size.Y > position.Y;
        }

        public List<Vector2> ExitingCheck(HitBox hitBox, Vector2 velocity)
        {
            Vector2 tempVel = velocity;
            Vector2 tempPos = new Vector2(1000000,1000000);
            Vector2 size = hitBox.Size;

            bool[] sideChecksH = new bool[] { false, false };
            bool[] sideChecksV = new bool[] { false, false };

            if (hitBox.DrawPosition.X + velocity.X <= DrawPosition.X)
            {
                tempPos.X = Position.X;
                //sideChecksH[0] = true;
            }
            if (hitBox.DrawPosition.X + hitBox.Size.X + velocity.X >= DrawPosition.X + Size.X)
            {
                if (tempPos.X != 1000000)
                {
                    tempPos.X = Position.X + Size.X / 2;
                    tempPos.X -= size.X / 2;
                }
                else
                {
                    tempPos.X = Position.X + Size.X;
                    tempPos.X -= size.X;
                }

                
                //sideChecksH[1] = true;
            }
            if (hitBox.DrawPosition.Y + velocity.Y <= DrawPosition.Y)
            {
                tempPos.Y = Position.Y;
                //sideChecksV[0] = true;
            }
            if (hitBox.DrawPosition.Y + hitBox.Size.Y + velocity.Y >= DrawPosition.Y + Size.Y)
            {
                if (tempPos.Y != 1000000)
                {
                    tempPos.Y = Position.Y + Size.Y / 2;
                    tempPos.Y -= size.Y / 2;
                }

                else
                {
                    tempPos.Y = Position.Y + Size.Y;
                    tempPos.Y -= size.Y;
                }
                
                //sideChecksV[1] = true;
            }

            if (tempPos.X != 1000000) velocity.X = 0;
            if (tempPos.Y != 1000000) velocity.Y = 0;

            return new List<Vector2> { velocity, tempPos };
        }

        public void SetWorldPos(Vector2 pos)
        {
            this.DrawPosition = pos;
        }

        /// <summary>
        /// Uses ShapeBatch
        /// </summary>
        public void Draw()
        {
            ShapeBatch.BoxOutline(DrawPosition.X, DrawPosition.Y, Size.X, Size.Y, color * .75f);
        }
    }
}
