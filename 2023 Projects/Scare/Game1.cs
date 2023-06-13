using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scare
{
    public enum GameState
    {
        Menu,
        Game,
        Pause,
        Instructions,
        Lose,
        Win
    }

    public class Game1 : Game
    {
        // Credit to https://community.monogame.net/t/why-doesnt-my-simple-subtractive-2d-lighting-work/16521/2
        // Custom Blend state for makeshift flashlight
        public readonly static BlendState blendStateSubtract = new BlendState
        {
            ColorSourceBlend = Blend.One,
            AlphaSourceBlend = Blend.One,

            ColorDestinationBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,

            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaBlendFunction = BlendFunction.ReverseSubtract
        };

        private Dictionary<string, Texture2D> assets;
        private Dictionary<string, List<Button>> buttons;

        private Texture2D[] buttonAssets;

        private Player player;
        private List<Enemy> enemies;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Camera camera;
        private GameManager GameManager;

        public const int width = 1920;
        public const int height = 1080;

        private SpriteFont font;

        private GameState currentState;

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

            camera = new Camera();

            currentState = GameState.Menu;

            assets = new Dictionary<string, Texture2D>();
            buttons = new Dictionary<string, List<Button>>();

            enemies = new List<Enemy>();

            buttons.Add("Menu", new List<Button>());
            buttons.Add("Pause", new List<Button>());
            buttons.Add("Instructions", new List<Button>());
            buttons.Add("Win", new List<Button>());
            buttons.Add("Lose", new List<Button>());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            assets.Add("floor", Content.Load<Texture2D>("floor"));
            assets.Add("entity", Content.Load<Texture2D>("square"));
            assets.Add("flashlight", Content.Load<Texture2D>("lightorb"));
            assets.Add("bar", Content.Load<Texture2D>("bar"));
            assets.Add("underbar", Content.Load<Texture2D>("underbar"));

            buttonAssets = new Texture2D[]
            {
                Content.Load<Texture2D>("button_inactive"),
                Content.Load<Texture2D>("button_active")
            };

            font = Content.Load<SpriteFont>("font");

            // Menu Buttons
            buttons["Menu"].Add(new Button(buttonAssets, new Rectangle(100, 400, 200, 125), "Play", font, Color.Black));
            buttons["Menu"][0].OnLeftClick += ChangeToGame;

            buttons["Menu"].Add(new Button(buttonAssets, new Rectangle(100, 600, 200, 125), "Instructions", font, Color.Black));
            buttons["Menu"][1].OnLeftClick += ShowInstructions;

            buttons["Menu"].Add(new Button(buttonAssets, new Rectangle(100, 800, 200, 125), "Quit", font, Color.Black));
            buttons["Menu"][2].OnLeftClick += Quit;

            // Instructions Buttons
            buttons["Instructions"].Add(new Button(buttonAssets, new Rectangle(100, 400, 200, 125), "Play", font, Color.Black));
            buttons["Instructions"][0].OnLeftClick += ChangeToGame;

            buttons["Instructions"].Add(new Button(buttonAssets, new Rectangle(100, 600, 200, 125), "Menu", font, Color.Black));
            buttons["Instructions"][1].OnLeftClick += ChangeToMenu;

            // Pause Buttons
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(100, 400, 200, 125), "Unpause", font, Color.Black));
            buttons["Pause"][0].OnLeftClick += ChangeToGame;

            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(100, 600, 200, 125), "Menu", font, Color.Black));
            buttons["Pause"][1].OnLeftClick += ChangeToMenu;

            // Win Buttons
            buttons["Win"].Add(new Button(buttonAssets, new Rectangle(100, 600, 200, 125), "Menu", font, Color.Black));
            buttons["Win"][0].OnLeftClick += ChangeToMenu;

            // Lose Buttons
            buttons["Lose"].Add(new Button(buttonAssets, new Rectangle(100, 400, 200, 125), "Retry?", font, Color.Black));
            buttons["Lose"][0].OnLeftClick += ChangeToGame;

            buttons["Lose"].Add(new Button(buttonAssets, new Rectangle(100, 600, 200, 125), "Menu", font, Color.Black));
            buttons["Lose"][1].OnLeftClick += ChangeToMenu;

            player = new Player(assets, Vector2.Zero);

            GameManager = new GameManager(enemies, player, camera, assets);
        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case GameState.Menu:

                    foreach (Button button in buttons["Menu"])
                    {
                        button.Update(gameTime);
                    }

                    break;

                case GameState.Game:

                    GameManager.Update(gameTime);

                    break;

                case GameState.Pause:

                    foreach (Button button in buttons["Pause"])
                    {
                        button.Update(gameTime);
                    }

                    break;

                case GameState.Instructions:

                    foreach (Button button in buttons["Instructions"])
                    {
                        button.Update(gameTime);
                    }

                    break;

                case GameState.Lose:

                    foreach (Button button in buttons["Lose"])
                    {
                        button.Update(gameTime);
                    }

                    break;

                case GameState.Win:

                    foreach (Button button in buttons["Win"])
                    {
                        button.Update(gameTime);
                    }

                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (currentState == GameState.Game)
            {
                _spriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: camera.Transform);
            }
            else
            {
                _spriteBatch.Begin();
            }
            
            switch (currentState)
            {
                case GameState.Menu:

                    foreach (Button button in buttons["Menu"])
                    {
                        button.Draw(_spriteBatch);
                    }

                    break;

                case GameState.Game:

                    _spriteBatch.Draw(assets["floor"], new Vector2(-assets["floor"].Width / 2, -assets["floor"].Height / 2), Color.White);

                    GameManager.Draw(_spriteBatch);

                    break;

                case GameState.Pause:

                    foreach (Button button in buttons["Pause"])
                    {
                        button.Draw(_spriteBatch);
                    }

                    break;

                case GameState.Instructions:

                    foreach (Button button in buttons["Instructions"])
                    {
                        button.Draw(_spriteBatch);
                    }

                    break;

                case GameState.Lose:

                    foreach (Button button in buttons["Lose"])
                    {
                        button.Draw(_spriteBatch);
                    }

                    break;

                case GameState.Win:

                    foreach (Button button in buttons["Win"])
                    {
                        button.Draw(_spriteBatch);
                    }

                    break;
            }

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void ChangeToGame()
        {
            currentState = GameState.Game;
        }

        public void ChangeToMenu()
        {
            currentState = GameState.Menu;
        }

        public void ShowInstructions()
        {
            currentState = GameState.Instructions;
        }

        public void Quit()
        {
            Exit();
        }
    }
}