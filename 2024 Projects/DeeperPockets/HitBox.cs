using MarchingSquares;
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
        private float LineThickness;

        public HitBox(Vector2 position, Vector2 size, Color color, float lineThickness)
        {
            this.Position = position;
            this.Size = size;
            this.color = color;
            this.LineThickness = lineThickness;

            Debug.Instance.AddDebugObj(this);
        }

        public void Update()
        {
            DrawPosition = Position - Global.Instance.WorldOffset - Global.Instance.CameraOffset;
        }

        public bool Interects(Vector2 position, Vector2 size)
        {
            return
                DrawPosition.X < position.X + size.X && DrawPosition.X + Size.X > position.X &&
                DrawPosition.Y < position.Y + size.Y && DrawPosition.Y + Size.Y > position.Y;
        }

        /// <summary>
        /// Uses ShapeBatch
        /// </summary>
        public void Draw()
        {
            ShapeBatch.BoxOutline(Position.X, Position.Y, Size.X, Size.Y, color * .5f);
        }
    }
}
