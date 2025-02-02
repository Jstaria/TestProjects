﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace _3D_Programming
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Camera
        private Camera mainCamera;

        // Marching Cubes
        private CubeMarching cube;

        private SpriteFont font;

        private BasicEffect basicEffect; // Tells XNA how to draw object

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize()
        {
            // Using default constructor
            mainCamera = new Camera();
            mainCamera.MoveSpeed = 5f;

            // BasicEffect, very powerful shader with open source code: https://learn.microsoft.com/en-us/previous-versions/windows/silverlight/dotnet-windows-silverlight/bb195083(v=xnagamestudio.35)
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1.0f;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.EnableDefaultLighting();

            cube = new CubeMarching(15,20,15,.01f, 100, .05f);
            cube.UpdateAll();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mainCamera.Update(gameTime);

            cube.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            VertexBuffer vertexBuffer = cube.VertexBuffer;

            // Pass in camera matrices
            basicEffect.Projection = mainCamera.ProjectionMatrix;
            basicEffect.View = mainCamera.ViewMatrix;
            basicEffect.World = mainCamera.WorldMatrix;

            GraphicsDevice.Clear(Color.MediumPurple);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            // Optional
            // Turn off back face culling so we can see both sides of our created geometry
            // Keeping this on is generally how everyone else would do it, it is quite literally just to be able to use our orbiting camera
            // and see both sides of the triangle

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;

            // Loop through all effects in shader, apply them, and draw our shader
            foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                if (vertexBuffer == null)
                    continue;

                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount);
            }

            //_spriteBatch.Begin();

            //_spriteBatch.DrawString(font, cube.triangleCount.ToString(), new Vector2(100, 100), Color.Black);
            //_spriteBatch.DrawString(font, cube.vertexCount.ToString(), new Vector2(100, 150), Color.Black);

            //_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}