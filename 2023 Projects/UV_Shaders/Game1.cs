using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace UV_Shaders
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private RenderTarget2D renderTarget2D;

        private Vector2 mousePos;

        private MouseState prevMState;

        private float time = .9999f;

        private Effect shock;

        private bool flag = false;

        private int width = 1000;
        private int height = 1000;

        private Texture2D hands;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            renderTarget2D = new RenderTarget2D(GraphicsDevice, width, height, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            shock = Content.Load<Effect>("ShockWave");

            hands = Content.Load<Texture2D>("hands");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released)
            {
                mousePos = new Vector2(mouseState.X, mouseState.Y);

                time = .9999f;
                flag = true;
            }

            if (flag)
            {
                time += .01f;
            }

            if (time > 1.99)
            {
                flag = false;
                time = 0.99999f;
            }

            // TODO: Add your update logic here

            prevMState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            GraphicsDevice.SetRenderTarget(renderTarget2D);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(hands, new Rectangle(0, 0, width, height), Color.White);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(SpriteSortMode.Deferred, effect: shock);

            //pixelShader.Parameters["pixelsY"].SetValue((float)1080);
            //pixelShader.Parameters["pixelation"].SetValue((float)8);
            //pixelShader.CurrentTechnique.Passes[0].Apply();
            //pixelShader.Parameters["pixelsX"].SetValue((float)1920);

            shock.Parameters["UpperFeather"].SetValue((float).1);
            shock.Parameters["BottomFeather"].SetValue((float).1);
            shock.Parameters["rippleIntensity"].SetValue((float)1);
            shock.Parameters["x"].SetValue((float)mousePos.X / _graphics.PreferredBackBufferWidth);
            //shock.Parameters["y"].SetValue((float)mousePos.Y / _graphics.PreferredBackBufferHeight);
            shock.Parameters["time"].SetValue((float)time);
            
            shock.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(renderTarget2D, new Rectangle(0, 0, width, height), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}