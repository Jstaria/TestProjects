using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics.CodeAnalysis;

namespace TriangleRayCollision
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;

        private Camera mainCamera;
        private BasicEffect basicEffect; // Tells XNA how to draw object

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            mainCamera = new Camera(Vector3.Zero, new Vector3(0, 0, -100), 60, _graphics.GraphicsDevice.DisplayMode.AspectRatio, 0.1f, 1000f, true);
            mainCamera.MoveSpeed = 5f;

            // BasicEffect, very powerful shader with open source code: https://learn.microsoft.com/en-us/previous-versions/windows/silverlight/dotnet-windows-silverlight/bb195083(v=xnagamestudio.35)
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1.0f;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.EnableDefaultLighting();

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Pass in camera matrices
            basicEffect.Projection = mainCamera.ProjectionMatrix;
            basicEffect.View = mainCamera.ViewMatrix;
            basicEffect.World = mainCamera.WorldMatrix;

            GraphicsDevice.Clear(Color.MediumPurple);

            base.Draw(gameTime);
        }
    }
}