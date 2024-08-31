﻿using DeeperPockets.Interfaces;
using DeeperPockets.Singletons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets.Entities
{
    internal class Player : Entity, IMove, IInput
    {
        private int speed;

        private Vector2 originalPos;

        public KeyboardState prevKBState { get; set; }
        public KeyboardState curKBState { get; set; }
        public MouseState prevMouseState { get; set; }
        public MouseState curMouseState { get; set; }
        public int Speed { get { return speed; } }

        public Player(int Speed, Vector2 position, Texture2D tex) : base(position, tex)
        {
            speed = Speed;
            originalPos = position;
        }

        public void Move()
        {
            curKBState = Keyboard.GetState();

            Vector2 velocity = Vector2.Zero;

            if (curKBState.IsKeyDown(Keys.W)) velocity.Y -= Speed;
            if (curKBState.IsKeyDown(Keys.A)) velocity.X -= Speed;
            if (curKBState.IsKeyDown(Keys.S)) velocity.Y += Speed;
            if (curKBState.IsKeyDown(Keys.D)) velocity.X += Speed;

            if (velocity != Vector2.Zero) velocity.Normalize();

            velocity *= Speed;

            Vector2 worldVelocity = Global.Instance.currentScreenBox.ExitingCheck(Global.Instance.camera.HitBox, velocity)[0];

            float marginOfError = 5;

            if (pos.X < originalPos.X - marginOfError || pos.X > originalPos.X + marginOfError) worldVelocity.X = 0;
            if (pos.Y < originalPos.Y - marginOfError || pos.Y > originalPos.Y + marginOfError) worldVelocity.Y = 0;

            Global.Instance.WorldOffset += worldVelocity;

            if (worldVelocity.X == 0) pos.X += velocity.X;
            if (worldVelocity.Y == 0) pos.Y += velocity.Y;

            prevKBState = curKBState;
        }

        public override void Update(GameTime gameTime)
        {
            drawPos = pos - Global.Instance.CameraOffset;

            Move();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, new Rectangle((int)drawPos.X - tex.Width / 2, (int)drawPos.Y - tex.Height / 2, tex.Width, tex.Height), Color.White);
        }
    }
}
