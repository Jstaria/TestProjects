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
            Vector2 tempPos = Vector2.Zero;

            if (hitBox.DrawPosition.X + velocity.X <= DrawPosition.X) tempPos.X = Position.X;
            if (hitBox.DrawPosition.X + hitBox.Size.X + velocity.X >= DrawPosition.X + Size.X) tempPos.X = Position.X + Size.X;
            if (hitBox.DrawPosition.Y + velocity.Y <= DrawPosition.Y) tempPos.Y = Position.Y;
            if (hitBox.DrawPosition.Y + hitBox.Size.Y + velocity.Y >= DrawPosition.Y + Size.Y) tempPos.Y = Position.Y + Size.Y;

            if (tempPos.X != 0) velocity.X = 0;
            if (tempPos.Y != 0) velocity.Y = 0;

            return new List<Vector2> { velocity, tempPos };
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
