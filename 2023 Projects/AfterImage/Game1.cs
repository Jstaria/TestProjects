using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AfterImage
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private EntityManager entityManager;

        private Player player;
        private Enemy enemy;

        private Texture2D asset;

        public static bool state;

        private KeyboardState prevKBState;

        private Vector2 lastMousePos;

        private Effect pixelShader;
        private Effect shock;

        private Texture2D camera;

        RenderTarget2D renderTarget2D;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            renderTarget2D = new RenderTarget2D(GraphicsDevice, 1920, 1080, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            base.Initialize();

            state = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            asset = Content.Load<Texture2D>("square");

            player = new Player(asset, new Vector2(300,300));
            enemy = new Enemy(new Vector2(300, 300), asset, asset.Width*2, asset.Height*2);

            pixelShader = Content.Load<Effect>("Pixel");
            shock = Content.Load<Effect>("ShockWave");

            entityManager = new EntityManager(player, new List<Enemy>() { enemy }, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            KeyboardState kbState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            entityManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget2D);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            
            entityManager.Draw(_spriteBatch);

            // TODO: Add your drawing code here

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(SpriteSortMode.Immediate, effect: pixelShader);

            pixelShader.Parameters["pixelsX"].SetValue((float)1920);
            pixelShader.Parameters["pixelsY"].SetValue((float)1080);
            pixelShader.Parameters["pixelation"].SetValue((float)8);
            pixelShader.CurrentTechnique.Passes[0].Apply();

            //shock.Parameters["UpperFeather"].SetValue((float)0.1);
            //shock.Parameters["BottomFeather"].SetValue((float)0.1);
            //shock.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            //shock.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(renderTarget2D, new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}