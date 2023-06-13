using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

// joseph Staria
// tests collisions between lines, lines, and technically rectangles
// Borrowed classes from HW2, credit: me

namespace TestCollision
{
    public enum State
    {
        Test,
        Wave,
        Jiggle,
        ShadersTest
    }
    public enum Colors
    {
        Red,
        RedOrange,
        Orange,
        OrangeYellow,
        Yellow,
        YellowGreen,
        Green,
        GreenBlue,
        Blue,
        BluePurple,
        Purple,
        PurpleRed
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Arrays of positions, square objects, and a player class
        private Rectangle[] pos;
        private Rectangle[,] posWave;
        private Square[] square;
        private Square[,] squareWave;
        private Player player;
        private Enemy enemy1;
        private Enemy enemy2;
        private Enemy enemy3;
        private Enemy enemy4;
        private Enemy[] enemies;

        // Assets, numbers, and positions
        private Random rng;
        private State state;
        private Colors colors;
        private Color color;
        private int i;
        private const int mult = 5;

        // For wave
        int squareWidth;
        int squareHeight;

        int numSquaresWidth;
        int numSquaresHeight;

        private Texture2D asset;
        public static Texture2D player2d;
        private SpriteFont text;

        private MouseState mouseState;
        private MouseState currentMouseState;
        private MouseState prevMouseState;
        private KeyboardState currentKBState;
        private KeyboardState prevKbState;

        private Vector2 startPos;
        private Vector2 endPos;

        // For Jiggle
        private Rectangle jigglePos;
        private Texture2D heart;

        public static int width;
        public static int height;

        bool[] jiggleFlags;

        // For Shaders test
        private GameManager gameManager;

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
            startPos = new Vector2(0, 0);
            endPos = new Vector2(0, 0);

            rng = new Random();
            state = State.Test;
            colors = Colors.Red;
            color = Color.Red;
            i = 0;

            // Number of squares on screen
            numSquaresWidth = 25;
            numSquaresHeight = 25;

            // Square parameters determined by how many squares we want
            squareWidth = _graphics.PreferredBackBufferWidth / numSquaresWidth;
            squareHeight = _graphics.PreferredBackBufferHeight / numSquaresHeight;

            // Setting up the arrays and 2d arrays for both test and wave
            pos = new Rectangle[10];
            posWave = new Rectangle[numSquaresWidth,numSquaresHeight];
            square = new Square[10];
            squareWave = new Square[numSquaresWidth,numSquaresHeight];

            // Test 1d array
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = new Rectangle(rng.Next(100, 901), rng.Next(100, 901), rng.Next(50, 251), rng.Next(50, 251));
            }

            // Wave 2d array
            for (int i = 0; i < posWave.GetLength(0); i++)
            {
                for (int j = 0; j < posWave.GetLength(1); j++)
                {
                    posWave[i,j] = new Rectangle(i * squareWidth, j * squareHeight, squareWidth, squareHeight);
                }
            }

            // Jiggle Pos Rectangle
            width = _graphics.PreferredBackBufferWidth;
            height = _graphics.PreferredBackBufferHeight;

            jigglePos = new Rectangle(width / 2, height / 2, 100, 150);
            jiggleFlags = new bool[] { false, false, false, false };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            text = Content.Load<SpriteFont>("text");
            asset = Content.Load<Texture2D>("square");
            player2d = Content.Load<Texture2D>("happyben");
            heart = Content.Load<Texture2D>("heart");

            // Enemies
            enemy1 = new Enemy(asset, new Rectangle(700, 700, 50, 50));
            enemy2 = new Enemy(asset, new Rectangle(700, 400, 50, 50));
            enemy3 = new Enemy(asset, new Rectangle(1200, 700, 50, 50));
            enemy4 = new Enemy(asset, new Rectangle(1200, 400, 50, 50));

            // Player object for test, allows movement of the start position
            player = new Player(player2d, 
                new Rectangle(
                    _graphics.PreferredBackBufferWidth / 2, 
                    _graphics.PreferredBackBufferHeight / 2, 
                    50, 50), 
                _graphics.PreferredBackBufferWidth, 
                _graphics.PreferredBackBufferHeight);

            // Adding squares to test array
            for (int i = 0; i < square.Length; i++)
            {
                square[i] = new Square(pos[i], asset);
            }

            // Adding squares to wave 2d array
            for (int i = 0; i < squareWave.GetLength(0); i++)
            {
                for (int j = 0; j < squareWave.GetLength(1); j++)
                {
                    squareWave[i, j] = new Square(posWave[i, j], asset);
                }
            }

            // Shaders game manager
            gameManager = new GameManager();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            currentKBState = Keyboard.GetState();

            switch (state)
            {
                // Simple movement, intersection, and line showing
                case State.Test:

                    enemies = new Enemy[] { enemy1, enemy2, enemy3, enemy4};

                    EnemySingleton.Count = enemies.Length;

                    // Line start and end positions
                    startPos = new Vector2(player.X + player.Position.Width / 2, player.Y + player.Position.Height / 2);
                    endPos = new Vector2(mouseState.X, mouseState.Y);

                    player.Update(gameTime);
                    
                    //enemy1.Update(gameTime, new Vector2(player.X, player.Y), enemy2);
                    //enemy2.Update(gameTime, new Vector2(player.X, player.Y), enemy1);

                    foreach (Enemy enemy in enemies)
                    {
                        foreach (Enemy enemy1 in enemies)
                        {
                            enemy.Update(gameTime, new Vector2(player.X, player.Y), enemy1);
                        }
                    }

                    if (currentKBState.IsKeyDown(Keys.Tab) && prevKbState.IsKeyUp(Keys.Tab))
                    {
                        state = State.Wave;
                    }

                    if (currentKBState.IsKeyDown(Keys.Q) && prevKbState.IsKeyUp(Keys.Q))
                    {
                        state = State.Jiggle;
                    }

                    if (currentKBState.IsKeyDown(Keys.PageUp) && prevKbState.IsKeyUp(Keys.PageUp))
                    {
                        state = State.ShadersTest;
                    }

                    break;

                case State.Wave:

                    currentMouseState = Mouse.GetState();

                    // Couldn't figure out a way to use a switch without it going a bajillion miles per hour,
                    // So I slowed it down by using a count and a const multiplier variable to determine however many ticks it should change
                    i++;
                    i %= 12 * mult;

                    // Line start and end
                    startPos = new Vector2(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight/2);
                    endPos = new Vector2(mouseState.X, mouseState.Y);

                    if (currentKBState.IsKeyDown(Keys.Tab) && prevKbState.IsKeyUp(Keys.Tab))
                    {
                        state = State.Test;
                    }
                    if (currentKBState.IsKeyDown(Keys.Q) && prevKbState.IsKeyUp(Keys.Q))
                    {
                        state = State.Jiggle;
                    }

                    if (ButtonState.Pressed == currentMouseState.LeftButton && ButtonState.Released == prevMouseState.LeftButton)
                    {
                        numSquaresWidth += 10;  
                        numSquaresHeight += 10;
                        UpdateWave();
                    }

                    else if (ButtonState.Pressed == currentMouseState.RightButton && ButtonState.Released == prevMouseState.RightButton && numSquaresHeight > 9)
                    {
                        numSquaresWidth -= 10;
                        numSquaresHeight -= 10;
                        UpdateWave();
                    }

                    break;

                case State.Jiggle:
                    

                    if (currentKBState.IsKeyDown(Keys.Tab) && prevKbState.IsKeyUp(Keys.Tab))
                    {
                        state = State.Test;
                    }
                    if (currentKBState.IsKeyDown(Keys.Q) && prevKbState.IsKeyUp(Keys.Q))
                    {
                        state = State.Wave;
                    }

                    if (jigglePos.Width == 100)
                    {
                        jiggleFlags[0] = true;
                        jiggleFlags[1] = false;
                    }
                    if (jigglePos.Width == 70) 
                    { 
                        jiggleFlags[1] = true;
                        jiggleFlags[0] = false;
                    }
                    if (jigglePos.Height == 150) 
                    { 
                        jiggleFlags[2] = true;
                        jiggleFlags[3] = false;
                    }
                    if (jigglePos.Height == 120)
                    {
                        jiggleFlags[3] = true;
                        jiggleFlags[2] = false;
                    }

                    if (jigglePos.Width > 70 && jiggleFlags[0])
                    {
                        jigglePos.Width--;
                    }
                    if (jigglePos.Height > 120 && jiggleFlags[2])
                    {
                        jigglePos.Height--;
                    }

                    if (jigglePos.Width < 100 && jiggleFlags[1])
                    {
                        jigglePos.Width++;
                    }
                    if (jigglePos.Height < 150 && jiggleFlags[3])
                    {
                        jigglePos.Height++;
                    }

                    break;

                case State.ShadersTest:



                    if (currentKBState.IsKeyDown(Keys.Tab) && prevKbState.IsKeyUp(Keys.Tab))
                    {
                        state = State.Test;
                    }

                    break;
            }

            prevKbState = currentKBState;
            prevMouseState = currentMouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Coral);

            _spriteBatch.Begin();


            switch (state)
            {
                case State.Test:

                    foreach (Enemy enemy in enemies)
                    {
                        enemy.DrawRotation(_spriteBatch);
                    }

                    player.Draw(_spriteBatch);

                    // Drawing the 10 random squares for test
                    
                    for (int i = 0; i < square.Length; i++)
                    {
                        // Draws the intersection colors, the points, and the line
                        // (The last two being based upon whether or not a passed in bool is true)
                        DrawIntersections(_spriteBatch, square[i], true, startPos, endPos);
                    }
                    
                    break;

                case State.Wave:
                    
                    // My rgb switch, not perfect, but thats because I needed to slow it down, so now it is an int with a mult
                    switch (i)
                    {
                        case 0 * mult:
                            color = Color.Firebrick;
                            colors = Colors.RedOrange;
                            break;

                        case 1 * mult:
                            color = Color.OrangeRed;
                            colors = Colors.Orange;
                            break;

                        case 2 * mult:
                            color = Color.DarkOrange;
                            colors = Colors.OrangeYellow;
                            break;

                        case 3 * mult:
                            color = Color.Goldenrod;
                            colors = Colors.Yellow;
                            break;

                        case 4 * mult:
                            color = Color.LimeGreen;
                            colors = Colors.YellowGreen;
                            break;

                        case 5 * mult:
                            color = Color.ForestGreen;
                            colors = Colors.Green;
                            break;

                        case 6 * mult:
                            color = Color.Green;
                            colors = Colors.GreenBlue;
                            break;

                        case 7 * mult:
                            color = Color.Teal;
                            colors = Colors.Blue;
                            break;

                        case 8 * mult:
                            color = Color.RoyalBlue;
                            colors = Colors.BluePurple;
                            break;

                        case 9 * mult:
                            color = Color.DarkSlateBlue;
                            colors = Colors.Purple;
                            break;

                        case 10 * mult:
                            color = Color.Purple;
                            colors = Colors.PurpleRed;
                            break;

                        case 11 * mult:
                            color = Color.Maroon;
                            colors = Colors.Red;
                            break;
                    }

                    // Draws the intersection colors, the points, and the line
                    // (The last two being based upon whether or not a passed in bool is true)
                    for (int i = 0; i < squareWave.GetLength(0); i++)
                    {
                        for (int j = 0; j < squareWave.GetLength(1); j++)
                        {
                            DrawIntersections(_spriteBatch, squareWave[i, j], false, startPos, endPos);
                        }
                    }

                    break;

                case State.Jiggle:

                    _spriteBatch.Draw(player2d, new Rectangle(width/2-225,height/2-550,400,800), Color.White);
                    _spriteBatch.Draw(heart, jigglePos, Color.White);

                    break;

                case State.ShadersTest:

                    _spriteBatch.End();

                    gameManager.Draw();

                    _spriteBatch.Begin();
                    break;

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawIntersections(SpriteBatch sb, Square square, bool pointSwitch, Vector2 startPos, Vector2 endPos)
        {
            square.CheckCollision(startPos, endPos);

            if (square.Flags[0])
            {
                // Checks for each wall of the rectangle
                square.Draw(_spriteBatch, color);
                if (square.Flags[1] && pointSwitch)
                {
                    _spriteBatch.Draw(asset, new Rectangle((int)square.Intersection1.X - 5, (int)square.Intersection1.Y - 5, 10, 10), Color.Yellow);
                }

                if (square.Flags[2] && pointSwitch)
                {
                    _spriteBatch.Draw(asset, new Rectangle((int)square.Intersection2.X - 5, (int)square.Intersection2.Y - 5, 10, 10), Color.Yellow);
                }

                if (square.Flags[3] && pointSwitch)
                {
                    _spriteBatch.Draw(asset, new Rectangle((int)square.Intersection3.X - 5, (int)square.Intersection3.Y - 5, 10, 10), Color.Yellow);
                }

                if (square.Flags[4] && pointSwitch)
                {
                    _spriteBatch.Draw(asset, new Rectangle((int)square.Intersection4.X - 5, (int)square.Intersection4.Y - 5, 10, 10), Color.Yellow);
                }
            }
            else
            {
                // Draws it in beige if no intersection
                square.Draw(_spriteBatch, Color.Beige);
            }

            if (pointSwitch)
            {
                _spriteBatch.End();

                ShapeBatch.Begin(GraphicsDevice);
                ShapeBatch.Line(startPos, endPos, Color.White);
                ShapeBatch.End();

                _spriteBatch.Begin();
            }

        }

        private void UpdateWave()
        {
            //Square parameters determined by how many squares we want
            squareWidth = _graphics.PreferredBackBufferWidth / numSquaresWidth;
            squareHeight = _graphics.PreferredBackBufferHeight / numSquaresHeight;

            // Arrays
            posWave = new Rectangle[numSquaresWidth, numSquaresHeight];
            squareWave = new Square[numSquaresWidth, numSquaresHeight];

            // Wave 2d array
            for (int i = 0; i < posWave.GetLength(0); i++)
            {
                for (int j = 0; j < posWave.GetLength(1); j++)
                {
                    posWave[i, j] = new Rectangle(i * squareWidth, j * squareHeight, squareWidth, squareHeight);
                }
            }

            // Adding squares to wave 2d array
            for (int i = 0; i < squareWave.GetLength(0); i++)
            {
                for (int j = 0; j < squareWave.GetLength(1); j++)
                {
                    squareWave[i, j] = new Square(posWave[i, j], asset);
                }
            }
        }

    }
}