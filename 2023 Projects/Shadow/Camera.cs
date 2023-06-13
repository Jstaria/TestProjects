using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadow
{
    internal class Camera
    {
        Matrix transform;

        public Matrix Transform 
        { 
            get { return transform; }
            private set { transform = value; }
        }

        public void Follow(Object target)
        {
            Matrix position = Matrix.CreateTranslation(
                -target.PositionV.X - (target.Position.Width / 2),
                -target.PositionV.Y - (target.Position.Height / 2),
                0);

            Matrix offset = Matrix.CreateTranslation(
                Game1.width / 2,
                Game1.height / 2,
                0);

            transform = position * offset;
        }

        public void Follow(Vector2 target)
        {
            Matrix position = Matrix.CreateTranslation(
                -target.X,
                -target.Y,
                0);

            Matrix offset = Matrix.CreateTranslation(
                Game1.width / 2,
                Game1.height / 2,
                0);

            transform = position * offset;
        }

        public void FollowDirection(Vector2 direction, Matrix transfrom)
        {
            Matrix directionM = Matrix.CreateTranslation(
                -direction.X,
                -direction.Y,
                0);

            Matrix offset = Matrix.CreateTranslation(
                Game1.width / 2,
                Game1.height / 2,
                0);

            Transform = transfrom * directionM * offset;
        }

    }
}
