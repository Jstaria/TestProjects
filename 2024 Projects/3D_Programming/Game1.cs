using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace _3D_Programming
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;

        // Camera
        private Camera mainCamera;

        private BasicEffect basicEffect; // Tells XNA how to draw object

        // Geometric Info
        private List<VertexPositionColor> triangleVertices; // For color, VertexPositionTexture for texture mapping
        private VertexBuffer vertexBuffer; // Graphics buffer

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
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
            basicEffect.LightingEnabled = false;

            // Create triangle
            triangleVertices = new List<VertexPositionColor>();


            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    int length = 10;
                    int width = 10;

                    Color color = Color.Wheat;

                    switch(j * i % 18)
                    {
                        case 0:
                            color = Color.Red;
                            break;

                        case 1:
                            color = Color.Orange;
                            break;

                        case 2:
                            color = Color.Yellow;
                            break;

                        case 3:
                            color = Color.Green;
                            break;

                        case 4:
                            color = Color.Blue;
                            break;

                        case 5: 
                            color = Color.Purple;
                            break;
                    }

                    // Create 2 triangles to make a square
                    // Triangle 1

                    Random ng = new Random();
                    int num = ng.Next(20, 20);

                    triangleVertices.Add(
                        new VertexPositionColor(
                            new Vector3(-10 * width + i * width + 0 * width, num, -10 * length + j * length + 0 * length), 
                            color));
                    triangleVertices.Add(
                        new VertexPositionColor(
                            new Vector3(-10 * width + i * width + 0 * width, num, -10 * length + j * length + 1 * length),
                            color));
                    triangleVertices.Add(
                        new VertexPositionColor(
                            new Vector3(-10 * width + i * width + 1 * width, num, -10 * length + j * length + 0 * length),
                            color));

                    // Triangle 2
                    triangleVertices.Add(
                        new VertexPositionColor(
                            new Vector3(-10 * width + i * width + 1 * width, num, -10 * length + j * length + 0 * length),
                            color));
                    triangleVertices.Add(
                        new VertexPositionColor(
                            new Vector3(-10 * width + i * width + 0 * width, num, -10 * length + j * length + 1 * length),
                            color));
                    triangleVertices.Add(
                        new VertexPositionColor(
                            new Vector3(-10 * width + i * width + 1 * width, num, -10 * length + j * length + 1 * length),
                            color));
                }
            }

            VertexPositionColor[] vertices = new VertexPositionColor[triangleVertices.Count];

            for (int i = 0; i < triangleVertices.Count; i++)
            {
                vertices[i] = triangleVertices[i];
            }

            // An array of drawables we will give to the GPU
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), triangleVertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //_spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mainCamera.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
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
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            // Loop through all effects in shader, apply them, and draw our shader
            foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, triangleVertices.Count);
            }

            base.Draw(gameTime);
        }
    }
}