using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FallingSand
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SandBox box;

        public static SpriteFont basicFont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 1000;
        }

        protected override void Initialize()
        {
            box = new SandBox(250, 250, _graphics.PreferredBackBufferHeight,15, 6);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            basicFont = Content.Load<SpriteFont>("basicFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            box.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ShapeBatch.Begin(_graphics.GraphicsDevice);

            box.Draw();

            ShapeBatch.Box(new Rectangle(10, 10, 400, 80), Color.DarkGray * .25f);

            ShapeBatch.End();

            _spriteBatch.Begin();

            _spriteBatch.DrawString(basicFont, "Brush: " + box.CurrentState.ToString(), new Vector2(20, 20), Color.White);
            _spriteBatch.DrawString(basicFont, "Press RMB to change brush", new Vector2(20, 50), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}