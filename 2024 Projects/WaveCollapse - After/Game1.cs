using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace WaveCollapse___After
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private CellGrid cellGrid;

        private Dictionary<int, Texture2D> assets;

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
            // TODO: Add your initialization logic here
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            FileInfo[] files = FileIO.GetFiles(".png");

            assets = new Dictionary<int, Texture2D>();
            string[] names = new string[files.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = files[i].Name.Remove(files[i].Name.Length - 4, 4);
                assets.Add(i+1, Content.Load<Texture2D>("../../../Content/tiles/" + names[i]));
            }

            cellGrid = new CellGrid(20, 20, assets, Point.Zero, 50);
            cellGrid.CreateCellOptionList();
            cellGrid.CreateGrid();
            //cellGrid.Collapse();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            cellGrid.Collapse1By1();
            
            //cellGrid.Collapse1By1Listed();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            cellGrid.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}