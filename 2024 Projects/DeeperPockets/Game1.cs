using DeeperPockets.Entities;
using DeeperPockets.Singletons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DeeperPockets
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private int screenWidth = 1920;
        private int screenHeight = 1080;

        private Texture2D square;
        private Player player;
        private Camera camera;
        private Map map;

        private List<TestEntity> entities;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            camera = new Camera(5, screenWidth, screenHeight);

            map = new Map();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            square = Content.Load<Texture2D>("square");

            Vector2 spawnPos = new Vector2(screenWidth / 2, screenHeight / 2);

            player = new Player(10, spawnPos, square);
            entities = new List<TestEntity>();

            // 9 test entities for testing player and camera movement
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    entities.Add(new TestEntity(spawnPos + new Vector2(i * 100, j * 100), square));
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            camera.Update(gameTime);
            
            map.Update();

            player.Update(gameTime);

            foreach (TestEntity entity in entities)
            {
                entity.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (TestEntity entity in entities)
            {
                entity.Draw(_spriteBatch);
            }

            player.Draw(_spriteBatch);

            _spriteBatch.End();

            // Draws debug information to the screen
            Debug.Instance.Draw(GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
