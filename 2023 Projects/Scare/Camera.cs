using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scare
{
    internal class Camera
    {
        private Matrix transform;

        public int Clamp
        {
            get { return 4500; }
        }

        public Matrix Transform 
        { 
            get { return transform; }
            private set { transform = value; }
        }

        public void Follow(Player target)
        {
            Matrix position = Matrix.CreateTranslation(
                MathHelper.Clamp(-target.Position.X - (target.Asset.Width / 2), -Clamp, Clamp),
                MathHelper.Clamp(-target.Position.Y - (target.Asset.Height / 2), -Clamp, Clamp),
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
