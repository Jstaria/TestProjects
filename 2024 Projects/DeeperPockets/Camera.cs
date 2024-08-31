using DeeperPockets.Interfaces;
using DeeperPockets.Singletons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets
{
    internal class Camera : IMove, IInput, ICollision
    {
        private int speed;
        private HitBox hitBox;

        public event ICollision.OnCollision OnCollisionEnter;

        public int Speed { get { return speed; } }
        public KeyboardState prevKBState { get; set; }
        public KeyboardState curKBState { get; set; }
        public MouseState prevMouseState { get; set; }
        public MouseState curMouseState { get; set; }

        public HitBox HitBox { get { return hitBox; } }

        public bool canMoveUp;
        public bool canMoveDown;
        public bool canMoveLeft;
        public bool canMoveRight;

        public Camera(int speed, int screenWidth, int screenHeight)
        {
            this.hitBox = new HitBox(Vector2.Zero, new Vector2(screenWidth, screenHeight), Color.DarkRed, 5);
            this.speed = speed;

            if (Global.Instance.camera == null) Global.Instance.camera = this;
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

            System.Diagnostics.Debug.WriteLine(Global.Instance.currentScreenBox.Intersects(hitBox));

            Global.Instance.CameraOffset += velocity;

            prevKBState = curKBState;
        }

        public void Update(GameTime gameTime)
        {
            Move();

            if (curKBState.IsKeyDown(Keys.Space)) Global.Instance.CameraOffset = Vector2.Zero;
        }

        public void OnCollisionDetection()
        {
            if (OnCollisionEnter != null) OnCollisionEnter.Invoke();
        }
    }
}
