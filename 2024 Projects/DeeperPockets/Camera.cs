using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets
{
    internal class Camera : IMove, IInput
    {
        public int Speed { get; set; }
        public KeyboardState prevKBState { get; set; }
        public KeyboardState curKBState { get; set; }
        public MouseState prevMouseState { get; set; }
        public MouseState curMouseState { get; set; }

        public Camera(int speed)
        {
            Speed = speed;
        }

        public void Move()
        {
            curKBState = Keyboard.GetState();

            Vector2 velocity = Vector2.Zero;

            if (curKBState.IsKeyDown(Keys.Up)) velocity.Y -= Speed;
            if (curKBState.IsKeyDown(Keys.Left)) velocity.X -= Speed;
            if (curKBState.IsKeyDown(Keys.Down)) velocity.Y += Speed;
            if (curKBState.IsKeyDown(Keys.Right)) velocity.X += Speed;

            if (velocity != Vector2.Zero) velocity.Normalize();

            velocity *= Speed;

            Global.Instance.CameraOffset += velocity;

            prevKBState = curKBState;
        }

        public void Update(GameTime gameTime)
        {
            Move();

            if (curKBState.IsKeyDown(Keys.Space)) Global.Instance.CameraOffset = Vector2.Zero;
        }
    }
}
