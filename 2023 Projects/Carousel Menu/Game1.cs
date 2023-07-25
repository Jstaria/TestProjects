using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Carousel_Menu
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Carousel carousel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the square textures into a list
            List<Texture2D> textures = new List<Texture2D>
        {
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png
            Content.Load<Texture2D>("square"),   // Load square.png

        };

            // Create the Carousel object passing the list of textures and the ContentManager
            carousel = new Carousel(Content, textures);
        }

        protected override void UnloadContent()
        {
            // Unload any non-ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Get the current mouse state
            MouseState mouseState = Mouse.GetState();

            // Update the Carousel passing the game time and the current mouse state
            carousel.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Set up a view matrix to position the camera (looking at the center of the carousel)
            Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 600), Vector3.Zero, Vector3.Up);

            // Set up a projection matrix for perspective rendering
            Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);

            // Start drawing with the SpriteBatch
            spriteBatch.Begin();

            // Draw the Carousel passing the GraphicsDevice, view matrix, projection matrix, and camera position
            carousel.Draw(GraphicsDevice, viewMatrix, projectionMatrix, new Vector3(0, 0, 1000000));

            // End drawing with the SpriteBatch
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}