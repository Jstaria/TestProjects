using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MarchingSquares
{
    public enum Smoothing
    {
        None,
        Smoothed
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D circleAsset;

        private MarchingSquares squares;

        private Smoothing smoothState;
        private KeyboardState prevKB;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1500;
            _graphics.PreferredBackBufferHeight = 1000;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            circleAsset = Content.Load<Texture2D>("circle");

            squares = new MarchingSquares(450, 300, _graphics.PreferredBackBufferHeight, circleAsset, .1f, .005f);
            //squares.SetValuesRandomly();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kb = Keyboard.GetState();

            squares.Update();

            if (kb.IsKeyDown(Keys.Space) && prevKB.IsKeyUp(Keys.Space))
            {
                smoothState = (Smoothing)(((int)smoothState + 1) % 2);
            }

            prevKB = kb;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ShapeBatch.Begin(_graphics.GraphicsDevice);

            switch(smoothState)
            {
                case Smoothing.None:
                    squares.Draw();
                    break;

                case Smoothing.Smoothed:
                    squares.DrawSmooth();
                    break;
            }

            
            // TODO: Add your drawing code here

            ShapeBatch.End();

            base.Draw(gameTime);
        }
    }
}