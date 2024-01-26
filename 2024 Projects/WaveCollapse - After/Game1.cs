using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace WaveCollapse___After
{
    public enum Structure
    {
        none,
        list,
        priorityQ
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private CellGrid cellGrid;

        private Dictionary<int, Texture2D> assets;

        public static SpriteFont basicFont;

        public static Structure currentStructure;
        private KeyboardState prevKB;

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

            currentStructure = Structure.none;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            basicFont = Content.Load<SpriteFont>("basicFont");

            FileInfo[] files = FileIO.GetFiles(".png");

            assets = new Dictionary<int, Texture2D>();
            string[] names = new string[files.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = files[i].Name.Remove(files[i].Name.Length - 4, 4);
                assets.Add(i+1, Content.Load<Texture2D>("../../../Content/tiles/" + names[i]));
            }

            cellGrid = new CellGrid(25, 25, assets, Point.Zero, 40);
            cellGrid.CreateCellOptionList();
            cellGrid.CreateGrid();
            //cellGrid.Collapse();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kb = Keyboard.GetState();

            switch (currentStructure)
            {
                case Structure.none:

                    cellGrid.Collapse1By1(gameTime);

                    SwitchState(kb, prevKB);

                    break;

                case Structure.list:

                    cellGrid.Collapse1By1Listed(gameTime);

                    SwitchState(kb, prevKB);

                    break;

                case Structure.priorityQ:

                    cellGrid.Collapse1By1Queued(gameTime);

                    SwitchState(kb, prevKB);

                    break;
            }

            prevKB = kb;

            base.Update(gameTime);
        }

        private void SwitchState(KeyboardState kb, KeyboardState prevKB)
        {
            if (kb.IsKeyDown(Keys.Space) && prevKB.IsKeyUp(Keys.Space))
            {
                currentStructure = (Structure)((int)(currentStructure + 1) % ((int)Structure.priorityQ + 1));
                cellGrid.CreateGrid();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            cellGrid.Draw(_spriteBatch);

            _spriteBatch.DrawString(basicFont, "Current Sorting Structure: " + currentStructure.ToString(), new Vector2(50, 80), Color.White);
            _spriteBatch.DrawString(basicFont, "Press SPACE to change sorting structure", new Vector2(50, 110), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}