using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Microsoft.VisualBasic;

namespace PuzzleGolf_TeamB
{
    /// <summary>
    /// Four basic game states
    /// </summary>
    public enum GameState
    {
        Title, // Instructions and Game Title
        Game, // Main Game 
        Rules, // Contains instructions for how to play
        Pause, // Pause Screen, includes option for Title/Main Menu
        GameOver, // Game Over
        Win     // Game Win
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static GameState currentState;

        // Width and Height of Window
        public static int width;
        public static int height;

        // Keyboard and Mouse logic
        private KeyboardState currentKBState;
        private KeyboardState previousKBState;
        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private Vector2 mousePosition;

        // Fonts
        private SpriteFont title;

        // Textures
        public static Texture2D circle;
        public static Texture2D square;

        private Dictionary<String, Texture2D> textures;

        private Texture2D titleScreen;
        private Texture2D pauseScreen;
        private Texture2D gameOverScreen;
        private Texture2D levelCompleteScreen;
        private Texture2D backScreen;
        private Texture2D cursor;


        private Texture2D startButton;
        private Texture2D rulesButton;
        private Texture2D godButton;
        private Texture2D titleButton;
        private Texture2D resumeButton;
        private Texture2D nextLevelButton;
        private Texture2D mainMenuButton;
        private Texture2D retryButton;
        private Texture2D greyedGodButton;

        //private Texture2D powerMeterBase;
        private Texture2D powerMeterSprites;

        public static Effect rainbow;

        private Color bgColor;

        // Game Objects (If declared here)
        private GolfBall ball;
        private PowerMeter powerMeter;

        // Game and Level
        private string[] levelNames;
        private GameManager GameManager;

        // Positions/Rectangles
        private Rectangle ballPos;
        private Random rng;

        // Buttons
        public bool godMode;
        private List<Button> titleButtons = new List<Button>();
        private List<Button> rulesButtons = new List<Button>();
        private List<Button> pauseButtons = new List<Button>();
        private List<Button> winButtons = new List<Button>();
        private List<Button> gameOverButtons = new List<Button>();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            width = _graphics.PreferredBackBufferWidth;
            height = _graphics.PreferredBackBufferHeight;

            currentState = GameState.Title;
            godMode = false;

            textures = new Dictionary<string, Texture2D>();

            // Game and Level manager
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            levelNames = new string[]
            {
                "PlayTestLevel",
                "Billy",
                "Nic",
                "Ross",
                "JoeLevel",
                "DuncanLevel",
                "JoeLevel2",
                "bonus"
            };
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // Default positions
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            ballPos = new Rectangle(width / 2, height / 2, 10, 10);

            mousePosition = new Vector2(0, 0);
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            bgColor = Color.Black;//new Color(10, 15, 62);

            rng = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            title = Content.Load<SpriteFont>("title");

            circle = Content.Load<Texture2D>("circle");
            square = Content.Load<Texture2D>("square");
            cursor = Content.Load<Texture2D>("cursor");

            textures.Add("Wall", Content.Load<Texture2D>("Wall"));
            textures.Add("Breakable", Content.Load<Texture2D>("Broken Wall"));
            textures.Add("UpDown", Content.Load<Texture2D>("Up_Down Wall"));
            textures.Add("LeftRight", Content.Load<Texture2D>("Left_Right Wall"));
            textures.Add("Square", Content.Load<Texture2D>("square"));
            textures.Add("Water", Content.Load<Texture2D>("water"));
            textures.Add("Goal", Content.Load<Texture2D>("goal"));
            textures.Add("Back", Content.Load<Texture2D>("background"));
            textures.Add("UI", Content.Load<Texture2D>("ui"));

            titleScreen = Content.Load<Texture2D>("TitleII");
            pauseScreen = Content.Load<Texture2D>("pauseScreen");
            gameOverScreen = Content.Load<Texture2D>("Game over");
            levelCompleteScreen = Content.Load<Texture2D>("Level complete");
            backScreen = Content.Load<Texture2D>("Back");

            startButton = Content.Load<Texture2D>("StartButton");
            rulesButton = Content.Load<Texture2D>("RulesButton");
            godButton = Content.Load<Texture2D>("GodButton");
            titleButton = Content.Load<Texture2D>("TitleButton");
            resumeButton = Content.Load<Texture2D>("ResumeButton");
            nextLevelButton = Content.Load<Texture2D>("Next-level");
            mainMenuButton = Content.Load<Texture2D>("Main-menu");
            retryButton = Content.Load<Texture2D>("retryButton");

            rainbow = Content.Load<Effect>("Rainbow");


            //powerMeterBase = Content.Load<Texture2D>("power meter");
            powerMeterSprites = Content.Load<Texture2D>("pm_spritesheet");

            // TODO: use this.Content to load your game content here

            ball = new GolfBall(circle, ballPos);
            powerMeter = new PowerMeter(powerMeterSprites, ballPos, ball);
            GameManager = new GameManager(levelNames, textures, powerMeter, ball, square);
            //GodGameManager = new GameManager(levelNames, powerMeter, ball, square);

            // button creation 
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // Title Buttons
            titleButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 1 / 2, height * 2 / 9, width / 6, height / 6),
                    startButton));
            titleButtons[0].OnLeftButtonClick += this.ChangeToGameState;
            titleButtons[0].OnLeftButtonClick += GameManager.StartOver;

            titleButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 1 / 2, height * 4 / 9, width / 6, height / 6),
                    rulesButton));
            titleButtons[1].OnLeftButtonClick += this.ChangeToRulesState;

            titleButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 1 / 2, height * 6 / 9, width / 6, height / 6),
                    godButton));
            titleButtons[2].OnLeftButtonClick += this.ToggleGodMode;

            // Rules Buttons
            rulesButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 5 / 12, height * 3 / 5, width / 6, height / 6),
                    mainMenuButton));
            rulesButtons[0].OnLeftButtonClick += this.ChangeToTitleState;
            
            // Pause Buttons
            pauseButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 5 / 12, height * 3 / 5, width / 6, height / 6),
                    titleButton));
            pauseButtons[0].OnLeftButtonClick += this.ChangeToTitleState;

            pauseButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 8 / 12, height * 3 / 5, width / 6, height / 6),
                    resumeButton));
            pauseButtons[1].OnLeftButtonClick += this.ChangeToGameState;

            pauseButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 2 / 12, height * 3 / 5, width / 6, height / 6),
                    retryButton));
            pauseButtons[2].OnLeftButtonClick += GameManager.RestartLevel;
            pauseButtons[2].OnLeftButtonClick += this.ChangeToGameState;

            // Game Over Buttons
            winButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 3 / 12, height * 3 / 5, width / 6, height / 6),
                    nextLevelButton));
            winButtons[0].OnLeftButtonClick += GameManager.NextLevel;
            winButtons[0].OnLeftButtonClick += this.ChangeToGameState;

            winButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 6 / 12, height * 3 / 5, width / 6, height / 6),
                    mainMenuButton));
            winButtons[1].OnLeftButtonClick += this.ChangeToTitleState;

            // Win Buttons
            gameOverButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 3 / 12, height * 3 / 5, width / 6, height / 6),
                    retryButton));
            gameOverButtons[0].OnLeftButtonClick += GameManager.RestartLevel;
            gameOverButtons[0].OnLeftButtonClick += this.ChangeToGameState;

            gameOverButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(width * 6 / 12, height * 3 / 5, width / 6, height / 6),
                    mainMenuButton));
            gameOverButtons[1].OnLeftButtonClick += this.ChangeToTitleState;
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentKBState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();



            // Toggles Fullscreen
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            if (_graphics.IsFullScreen && 
                currentKBState.IsKeyDown(Keys.RightAlt) && 
                SingleKeyPress(currentKBState, Keys.Enter))
            {
                _graphics.PreferredBackBufferWidth = 1280;
                _graphics.PreferredBackBufferHeight = 720;

                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();

                width = _graphics.PreferredBackBufferWidth;
                height = _graphics.PreferredBackBufferHeight;

            }

            else if (!_graphics.IsFullScreen && 
                currentKBState.IsKeyDown(Keys.RightAlt) && 
                SingleKeyPress(currentKBState, Keys.Enter))
            {
                _graphics.PreferredBackBufferWidth = 1920/*GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width*/;
                _graphics.PreferredBackBufferHeight = 1080/*GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height*/;

                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();

                width = _graphics.PreferredBackBufferWidth;
                height = _graphics.PreferredBackBufferHeight;
            }
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // FSM for current GameState
            switch (currentState)
            {
                case GameState.Title:

                    //GameManager.StartOver();

                    //button update
                    foreach (Button b in titleButtons)
                    {
                        b.Update(gameTime);
                    }

                    break;

                case GameState.Rules:

                    //button update
                    foreach (Button b in rulesButtons)
                    {
                        b.Update(gameTime);
                    }

                    break;

                case GameState.Game:

                    ball.CurrentMousePosDir = new Vector2(currentMouseState.X, currentMouseState.Y);

                    if (SingleKeyPress(currentKBState, Keys.Tab))
                    {
                        currentState = GameState.Pause;
                    }

                    // temp insertion of wall list until file io is done
                    GameManager.Update(currentMouseState, gameTime);


                    if (SingleKeyPress(currentKBState, Keys.Delete))
                    {
                        currentState = GameState.GameOver;
                    }

                    break;

                case GameState.Pause:

                    foreach (Button b in pauseButtons)
                    {
                        b.Update(gameTime);
                    }
                    break;

                case GameState.GameOver:
                    //          GameManager.RestartLevel();
                    //
                    // IF the player is above a certain par number or some kind of lose condition 
                    // Like a wall that "Destroys" the ball or the player loses the ball
                    // I think Billy is working on that wall/tile later so that'll be our lose condition

                    foreach (Button b in gameOverButtons)
                    {
                        b.Update(gameTime);
                    }

                    /*if (SingleLeftMousePress(currentMouseState))
                    {
                        currentState = GameState.Title;
                    }*/
                    break;

                case GameState.Win:

                    // We need two options, a button for main menu, and a title for next level
                    // Next level would require a button to use 
                    //
                    //          GameManager.NextLevel();

                    foreach (Button b in winButtons)
                    {
                        if (b.Equals(winButtons[0]))
                        {
                            if (GameManager.CurrentLevelNum + 1 < levelNames.Length)
                            {
                                b.Update(gameTime);
                            }
                        }

                        else
                        {
                            b.Update(gameTime);
                        }
                    }

                    /*if (SingleLeftMousePress(currentMouseState))
                    {
                        currentState = GameState.Title;
                    }*/
                    break;
            }

            previousKBState = currentKBState;
            previousMouseState = currentMouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);
            _spriteBatch.Begin();

            // FSM for current GameState
            switch (currentState)
            {
                // --- title display ---
                case GameState.Title:

                    _spriteBatch.Draw(titleScreen, 
                        new Rectangle(0, 0, width, height), Color.White);
                    
                    //draw buttons
                    foreach (Button b in titleButtons)
                    {
                        if (b != titleButtons[2])
                        {
                            b.Draw(_spriteBatch, Color.White);
                        }
                        else
                        {
                            Color color = Color.White;

                            if (GameManager.GodMode)
                            {
                                color = Color.Green;
                            }

                            b.Draw(_spriteBatch, color);
                        }
                        
                    }

                    break;


                // --- rules display ---
                case GameState.Rules:

                    _spriteBatch.Draw(pauseScreen,
                       new Rectangle(0, 0, width, height), Color.White);

                    //display buttons
                    foreach (Button b in rulesButtons)
                    {
                        b.Draw(_spriteBatch, Color.White);
                    }

                    break;
                    

                // --- game state ---
                case GameState.Game:
                     
                    _spriteBatch.Draw(textures["UI"], Vector2.Zero, Color.White * .75f);

                    GameManager.Draw(_spriteBatch, title);

                    _spriteBatch.DrawString(title,
                        "Use TAB to pause",
                        new Vector2((int)(width * 4.2/ 5), height / 14), Color.White);

                    _spriteBatch.DrawString(title,
                        "Alt + Enter can be\n" +
                        "Used " +
                        "for fullscreen!",
                        new Vector2((int)(width * 4.2 / 5), height / 8), Color.White);

                    break;


                // --- pause state ----
                case GameState.Pause:

                    _spriteBatch.Draw(pauseScreen,
                       new Rectangle(0, 0, width, height), Color.White);

                    // Display buttons
                    foreach (Button b in pauseButtons)
                    {
                        b.Draw(_spriteBatch, Color.White);
                    }

                    break;
                    

                // --- game over state ---
                case GameState.GameOver:

                    _spriteBatch.Draw(gameOverScreen,
                        new Rectangle(0, 0, width, height), Color.White);

                    //display buttons
                    foreach (Button b in gameOverButtons)
                    {
                        b.Draw(_spriteBatch, Color.White);
                    }

                    break;


                // --- win state ---
                case GameState.Win:

                    _spriteBatch.Draw(levelCompleteScreen,
                        new Rectangle(0, 0, width, height), Color.White);
                    
                    // Display win information
                    _spriteBatch.DrawString(title,
                        "\n\n\n\n\nYOUR SCORE: " + GameManager.Score,
                        new Vector2((int)(width / 3), height / 3), Color.White);

                    //display buttons
                    foreach (Button b in winButtons)
                    {
                        if (b.Equals(winButtons[0]))
                        {
                            if (GameManager.CurrentLevelNum + 1 < levelNames.Length)
                            {
                                b.Draw(_spriteBatch, Color.White);
                            }
                        }

                        else
                        {
                            b.Draw(_spriteBatch, Color.White);
                        }
                        
                    }

                    break;
            }

            int num = 50;

            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, effect: rainbow);

            Game1.rainbow.Parameters["time"].SetValue(GameManager.time);
            Game1.rainbow.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(cursor, new Rectangle((int)currentMouseState.X - num / 2 + 5, (int)currentMouseState.Y - num / 2 + 5, num, num), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool SingleKeyPress(KeyboardState currentKBState, Keys key)
        {
            return currentKBState.IsKeyDown(key) && previousKBState.IsKeyUp(key);
        }

        private bool SingleLeftMousePress(MouseState currentMouseState)
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed) 
                && (previousMouseState.LeftButton == ButtonState.Released);
        }

        private bool SingleRightMousePress(MouseState currentMouseState)
        {
            return (currentMouseState.RightButton == ButtonState.Pressed)
                && (previousMouseState.RightButton == ButtonState.Released);
        }

        // State changing methods for buttons
        public void ChangeToGameState()
        {
            currentState = GameState.Game;
        }
        public void ChangeToRulesState()
        {
            currentState = GameState.Rules;
        }
        public void ChangeToTitleState()
        {
            currentState = GameState.Title;
        }
        public void ToggleGodMode()
        {
            GameManager.GodMode = !GameManager.GodMode;
        }
    }
}