using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Transactions;

namespace PuzzleGolf_TeamB
{
    internal class GameManager
    {
        // Some objects
        private GolfBall ball;
        private PowerMeter powerMeter;
        private ParticleSystem afterImage;

        // Will become a list later for each different texture
        /// <summary>
        ///  Place holder variable for our textures
        /// </summary>
        private Texture2D background;

        /// <summary>
        /// Whether or not god mode is currently active
        /// </summary>
        private bool godModeActive;

        private Vector2 mousePosition;
        //private Vector2 prevMousePos;

        // Previous keyboard state
        private MouseState prevMouseState;

        /// <summary>
        /// File loader for the levels
        /// </summary>
        private FileIO levelManager;

        /// <summary>
        /// Current level loaded
        /// </summary>
        public Wall[,] currentLevel;

        /// <summary>
        /// Data to keep track of the current level
        /// </summary>
        private int parNum, puts, prevPuts, totalScore, currentLevelNum;

        /// <summary>
        /// Public time variable
        /// </summary>
        public static float time;

        private bool godMode;

        public int Score
        {
            get
            {
                int score = 1000;

                if (prevPuts > parNum)
                {
                    int totalSub = prevPuts - parNum;

                    score = (int)Vector2.Clamp(new Vector2(score - (totalSub * 75), 0), Vector2.Zero, new Vector2(int.MaxValue, 0)).X;
                }

                else if (prevPuts < parNum)
                {
                    for (int i = 0; i < parNum - prevPuts; i++)
                    {
                        score += (int)(i * i * i * 10 * parNum / prevPuts);
                    }
                }

                totalScore += score;

                return score;
            }

        }

        /// <summary>
        /// Public get, private set
        /// </summary>
        public int Par
        {
            get { return parNum; }
            private set { parNum = value; }
        }

        /// <summary>
        /// Public get and set for put number
        /// </summary>
        public int Puts
        {
            get { return puts; }
        }

        /// <summary>
        /// The current level which you are on
        /// </summary>
        public int CurrentLevelNum
        {
            get { return currentLevelNum; }
        }

        // Updating godmode
        public bool GodMode
        {
            get { return godModeActive; }
            set { godModeActive = value; }
        }

        /// <summary>
        /// A game manager for the FileIO, and player data
        /// </summary>
        /// <param name="levelNames">An array of file names to be passed into a file manager</param>

        public GameManager(string[] levelNames, Dictionary<String, Texture2D> textures, PowerMeter powerMeter, GolfBall ball, Texture2D square)
        {
            this.parNum = 3;
            this.puts = 0;
            this.currentLevelNum = 0;
            this.godMode = godMode;

            currentLevel = new Wall[0, 0];

            levelManager = new FileIO();
            levelManager.LoadAssets(levelNames, textures);

            this.powerMeter = powerMeter;
            this.ball = ball;

            this.background = textures["Back"];

            afterImage = new ParticleSystem(Game1.circle, ball.PositionV, 0, 100, 7, ball);
        }

        /// <summary>
        /// Updates the the game accordingly
        /// </summary>
        /// <param name="currentMouseState"></param>
        public void Update(MouseState currentMouseState, GameTime gameTime)
        {
            // Updates power meter FSM
            float estimatedPower = powerMeter.EstimatedPow(currentMouseState);

            // Retrieves all information the ball needs to move
            if (SingleLeftMousePress(currentMouseState) && ball.Power == 0)
            {
                mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

                ball.Direction(mousePosition);

                //needs tweaking!! changes the "feel" of the game a lot depending on the divisor
                ball.Power = estimatedPower;

                // bool for god mode with && statement
                // ----------------------------------------------------------------------------------------------------------------------------------------------------
                if (puts >= Par * 2 && !godModeActive)
                {
                    Game1.currentState = GameState.GameOver;
                    puts = 0;
                }

                else
                {
                    AddPut();
                }
            }

            //if (SingleRightMousePress(currentMouseState))
            //{
            //    ball.PositionV = new Vector2(currentMouseState.X, currentMouseState.Y);
            //}

            if (mousePosition != new Vector2(0, 0))
            {
                // Moves ball
                ball.Update();

                for (int Rows = 0; Rows < currentLevel.GetLength(0); Rows++)
                {
                    for (int Columns = 0; Columns < currentLevel.GetLength(1); Columns++)
                    {
                        Wall currentWall = currentLevel[Rows, Columns];

                        if (currentWall != null && currentWall.IsMovable)
                        {
                            currentLevel[Rows, Columns].WallMove(ball);
                        }

                        if (currentWall != null)
                        {
                            if (ball.CheckCollisionX(currentWall.Position, true) || 
                                ball.CheckCollisionY(currentWall.Position, true))
                            {
                                // Goal
                                if (Rows == levelManager.GoalPos.X && Columns == levelManager.GoalPos.Y)
                                {
                                    Game1.currentState = GameState.Win;
                                    puts = 0;
                                    // NextLevel();
                                }

                                // bool for god mode with && statement
                                // ------------------------------------
                                //Water Tile
                                if (currentWall is Tiles && !godModeActive)
                                {
                                    Game1.currentState = GameState.GameOver;
                                    puts = 0;
                                }

                                // Breakable wall
                                if (currentWall.IsBreakable)
                                {
                                    currentLevel[Rows, Columns] = null;
                                }
                            }
                        }
                    }
                }

                afterImage.ParticleUpdate(gameTime);
                
            }

            time = (float)gameTime.TotalGameTime.TotalSeconds;

            prevMouseState = currentMouseState;
        }

        /// <summary>
        /// Draws all instances on board
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb, SpriteFont title)
        {
            sb.Draw(background, new Rectangle(
                currentLevel[0,0].Position.X, 
                currentLevel[0,0].Position.Y, 
                (int)(25 * currentLevel[0,0].Position.Height), 
                (int)(25 * currentLevel[0, 0].Position.Height)), Color.White);

            afterImage.Draw(sb);
            

            ball.Draw(sb);

            // original draw
            //powerMeter.Draw(_spriteBatch);

            // Only draws movable walls so they are under the regular walls
            foreach (Wall Bwall in currentLevel)
            {
                if (Bwall != null)
                {
                    if (!Bwall.IsBroken && Bwall.IsMovable)
                    {
                        Bwall.DrawWall(sb);
                    }
                }
            }

            // Draws every other wall
            foreach (Wall Bwall in currentLevel)
            {
                if (Bwall != null)
                {
                    if (!Bwall.IsBroken && !Bwall.IsMovable)
                    {
                        Bwall.DrawWall(sb);
                    }
                }
            }

            if (ball.Power == 0)
            {
                // Updated draw rotation
                powerMeter.Draw(sb);
            }



            sb.DrawString(title, ball.PositionV.ToString(),
                new Vector2(50, 50), Color.White);

            sb.DrawString(title, "Puts: " + puts.ToString(),
                new Vector2(50, 100), Color.White);

            sb.DrawString(title, "Par: " + Par.ToString(),
                new Vector2(50, 150), Color.White);

            if (!godModeActive)
            {
                sb.DrawString(title, String.Format("Fail at 2x Par: {0}", Par * 2),
                new Vector2(50, 200), Color.White);
            }
        }

        /// <summary>
        /// Loads in next level from fileIO and resets corresponding data
        /// </summary>
        public void NextLevel()
        {
            ball.Power = 0;

            currentLevel = levelManager.LoadNext();
            currentLevelNum++;

            this.parNum = levelManager.ParNum;
            this.puts = 0;

            ball.PositionV = levelManager.BallPos;

            afterImage = new ParticleSystem(Game1.circle, ball.PositionV, 0, 100, 7, ball);
        }

        /// <summary>
        /// Just adds a put
        /// </summary>
        public void AddPut()
        {
            if (puts == 0)
            {
                prevPuts = 0;
            }

            puts++;
            prevPuts++;
        }

        /// <summary>
        /// Synced up to a button to restart the level
        /// </summary>
        public void RestartLevel()
        {
            ball.Power = 0;

            currentLevel = levelManager.ThisLevel();
            currentLevelNum = levelManager.FilePos;

            this.parNum = levelManager.ParNum;
            this.puts = 0;

            ball.PositionV = levelManager.BallPos;

            afterImage = new ParticleSystem(Game1.circle, ball.PositionV, 0, 100, 7, ball);
        }

        public void StartOver()
        {
            totalScore = 0;
            ball.Power = 0;

            currentLevel = levelManager.LoadSpecificLevel(0);
            this.parNum = levelManager.ParNum;
            this.puts = 0;

            ball.PositionV = levelManager.BallPos;

            afterImage = new ParticleSystem(Game1.circle, ball.PositionV, 0, 100, 7, ball);
        }

        private bool SingleLeftMousePress(MouseState currentMouseState)
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed)
                && (prevMouseState.LeftButton == ButtonState.Released);
        }

        private bool SingleRightMousePress(MouseState currentMouseState)
        {
            return (currentMouseState.RightButton == ButtonState.Pressed)
                && (prevMouseState.RightButton == ButtonState.Released);
        }
    }
}
