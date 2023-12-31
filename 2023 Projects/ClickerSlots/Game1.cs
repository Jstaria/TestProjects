﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClickerSlots
{
    public enum GameState
    {
        Game,
        Slots,
        Pause,
        Menu
    }

    public enum MenuState
    {
        Menu,
        Achievements,
        Credit
    }

    public class Game1 : Game
    {
        private List<Achievement> achievements;
        private RenderTarget2D renderTarget;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random rng;

        public static SpriteFont ReadOut;
        public static SpriteFont regular;

        private SlotMachine[] MenuGraphic;

        public static SoundEffect hitMarker;
        public static SoundEffect buttonSound;

        private float fade;

        private Dictionary<string, List<Button>> buttons;

        public static Dictionary<string, Texture2D> assets;

        private GameState prevState;
        private GameState currentState;
        private MenuState currentMenuState;

        private int prevScrollValue;
        private List<int> maxScroll;
        private List<int> minScroll;

        private float sceneSwitchTime;
        private float currentSceneTime;

        private SlotMachine slot;
        private PointManager pointManager;

        Rectangle[,] positionArray;

        private Soundtrack playlist;
        private Song[] songs;
        private string[] songNames;

        private float time;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1240;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            time = 0;

            rng = new Random();

            MenuGraphic = new SlotMachine[5];

            renderTarget = new RenderTarget2D(GraphicsDevice, 1240, 720, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            fade = 0;

            currentMenuState = MenuState.Menu;
            currentState = GameState.Menu;

            buttons = new Dictionary<string, List<Button>>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ///////////////////////////////////////////////////////////////////////////////
            // Loads song info from folder, first time doing somehting like this
            // Did this after loading in all the content texture files
            DirectoryInfo di = new DirectoryInfo("../../../Content/Music");
            FileInfo[] files = di.GetFiles("*.mp3");

            string[] songNames = new string[files.Length];
            songs = new Song[files.Length];

            for (int i = 0; i < songs.Length; i++)
            {
                songNames[i] = files[i].Name.Remove(files[i].Name.Length - 4, 4);
                songs[i] = Content.Load<Song>("Music/" + songNames[i]);
            }

            this.playlist = new Soundtrack(songs);
            ///////////////////////////////////////////////////////////////////////////////

            ReadOut = Content.Load<SpriteFont>("ReadOut");
            regular = Content.Load<SpriteFont>("regular");

            assets = new Dictionary<string, Texture2D>
            {
                {"square",Content.Load<Texture2D>("square")},
                {"button_active",Content.Load<Texture2D>("button_active")},
                {"button_inactive",Content.Load<Texture2D>("button_inactive")},
                {"behindSlots",Content.Load<Texture2D>("behindSlotsBlur")},
                {"behindMenu",Content.Load<Texture2D>("behindMenuBlur")},
                {"light_effect",Content.Load<Texture2D>("light_effect")},
                {"cloud",Content.Load<Texture2D>("cloud")},
                {"sparkle",Content.Load<Texture2D>("sparkle")},
                {"orb",Content.Load<Texture2D>("orb")},
                {"triangle",Content.Load<Texture2D>("triangle")},
            };

            hitMarker = Content.Load<SoundEffect>("hitmarker-sound-effect");
            buttonSound = Content.Load<SoundEffect>("button_sound");

            slot = new SlotMachine(assets, new Vector2(0, 185), false, 1);
            
            for (int i = 0; i < MenuGraphic.Length-1; i++)
            {
                int randNum = 0;

                // generates random idle speed for menu slot machines that cannot be still or the same as the main menu slots
                do
                {
                    randNum = rng.Next(-10, 10);
                }
                while (randNum == 0 || randNum == 2);
                    
                MenuGraphic[i] = new SlotMachine(assets, new Vector2(0, -90 + (200 * i)), true,  randNum * .5f);
            }

            MenuGraphic[MenuGraphic.Length-1] = new SlotMachine(assets, new Vector2(0, 230), true, 1);

            pointManager = new PointManager();

            CreateButtons();

            SetupAchievements();
        }

        protected override void Update(GameTime gameTime)
        {
            currentSceneTime = (float)gameTime.TotalGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //ps.Position = Mouse.GetState().Position.ToVector2();

            playlist.Play();

            #region Update logic

            AchievementManager.Update();

            switch (currentState)
            {
                case GameState.Game:

                    ReduceFade();

                    pointManager.Update(gameTime);

                    if (CanUseButtons(gameTime))
                    {
                        foreach (Button button in buttons["Game"])
                        {
                            button.Update(gameTime);
                        }
                    }

                    break;

                case GameState.Slots:

                    ReduceFade();

                    slot.Update(gameTime);

                    if (CanUseButtons(gameTime))
                    {
                        foreach (Button button in buttons["Slots"])
                        {
                            button.Update(gameTime);
                        }
                    }

                    break;

                case GameState.Pause:

                    if (prevState == GameState.Menu)
                    {
                        for (int i = 0; i < MenuGraphic.Length; i++)
                        {
                            MenuGraphic[i].Update(gameTime);
                        }
                    }

                    if (fade < .9)
                    {
                        fade += .05f;
                    }

                    if (CanUseButtons(gameTime))
                    {
                        foreach (Button button in buttons["Pause"])
                        {
                            button.Update(gameTime);
                        }
                    }

                    break;

                case GameState.Menu:

                    //int mousevalue = Mouse.GetState().ScrollWheelValue;

                    switch (currentMenuState)
                    {
                        case MenuState.Menu:

                            ReduceFade();

                            for (int i = 0; i < MenuGraphic.Length; i++)
                            {
                                MenuGraphic[i].Update(gameTime);
                            }

                            if (CanUseButtons(gameTime))
                            {
                                foreach (Button button in buttons["Menu"])
                                {
                                    button.Update(gameTime);
                                }
                            }

                            break;

                        case MenuState.Achievements:

                            int scrollValue = Mouse.GetState().ScrollWheelValue / 10;
                            int scrollAmount = 50;

                            if (prevScrollValue - scrollValue != 0 && achievements.Count > 8)
                            {
                                if (prevScrollValue - scrollValue > 0)
                                {
                                    for (int i = 0; i < achievements.Count; i++)
                                    {
                                        achievements[i].Position = new Rectangle(achievements[i].Position.X, Math.Clamp(achievements[i].Position.Y - scrollAmount, minScroll[i], maxScroll[i]), achievements[i].Position.Width, achievements[i].Position.Height);
                                    }
                                }
                                
                                if (prevScrollValue - scrollValue < 0)
                                {
                                    for (int i = 0; i < achievements.Count; i++)
                                    {
                                        achievements[i].Position = new Rectangle(achievements[i].Position.X, Math.Clamp(achievements[i].Position.Y + scrollAmount, minScroll[i], maxScroll[i]), achievements[i].Position.Width, achievements[i].Position.Height);
                                    }
                                }
                            }

                            prevScrollValue = scrollValue;

                            foreach (Button button in buttons["Achievements"])
                            {
                                button.Update(gameTime);
                            }

                            break;

                        case MenuState.Credit:

                            break;
                    }

                    break;
            }

            if ((int)(gameTime.TotalGameTime.Seconds) % 5 == 0)
            {
                AchievementManager.SaveAchievements();
                PointManager.Save();
            }

            #endregion

            //ps.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin();

            #region MainGameDraw

            switch (currentState)
            {
                case GameState.Game:

                    pointManager.Draw(_spriteBatch);

                    foreach (Button button in buttons["Game"])
                    {
                        button.Draw(_spriteBatch);
                    }

                    DrawFade();

                    break;

                case GameState.Slots:

                    slot.Draw(_spriteBatch);

                    foreach (Button button in buttons["Slots"])
                    {
                        button.Draw(_spriteBatch);
                    }

                    DrawFade();

                    break;

                case GameState.Pause:

                    switch (prevState)
                    {
                        case GameState.Game:
                            pointManager.Draw(_spriteBatch);
                            break;

                        case GameState.Slots:
                            slot.Draw(_spriteBatch);
                            break;

                        case GameState.Menu:
                            DrawMenu();
                            break;
                    }

                    switch(prevState)
                    {
                        case GameState.Game:
                            
                            buttons["Game"][1].Draw(_spriteBatch);

                            break;

                        case GameState.Slots:

                            buttons["Slots"][1].Draw(_spriteBatch);

                            break;

                        case GameState.Menu:

                            buttons["Menu"][1].Draw(_spriteBatch);

                            break;
                    }
                    
                    _spriteBatch.Draw(
                        assets["square"],
                        new Rectangle(
                            (int)(100 * Math.Clamp(fade * 2 + .1, 0, 1)), 100,
                            (int)(1040 * Math.Clamp(fade * 1.5f + .1, 0, 1)), 520),
                        Color.Black * (float)Math.Clamp(fade * 1.5f, 0, .9f));

                    foreach (Button button in buttons["Pause"])
                    {
                        button.Draw(_spriteBatch);
                    }

                    _spriteBatch.DrawString(ReadOut, MediaPlayer.Queue.ActiveSong.Name + " By Kevin Mcleod", new Vector2(165, 120), Color.Gray);

                    break;

                case GameState.Menu:

                    switch (currentMenuState)
                    {
                        case MenuState.Menu:

                            DrawMenu();

                            break;

                        case MenuState.Achievements:

                            DrawAchievements();

                            foreach (Button button in buttons["Achievements"])
                            {
                                button.Draw(_spriteBatch);
                            }

                            break;

                        case MenuState.Credit:

                            break;
                    }

                    break;
            }

            #endregion

            //ps.Draw(_spriteBatch);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            _spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 1240, 720), Color.White);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reads in achievements from file
        /// </summary>
        public void SetupAchievements()
        {
            AchievementManager.GenerateAchievements();

            minScroll = new List<int>();
            maxScroll = new List<int>();

            achievements = new List<Achievement>();

            int positionArrayY = (int)((AchievementManager.NumAchievements + 1) / 2);

            positionArray = new Rectangle[2, positionArrayY];

            for (int i = 0; i < AchievementManager.Achievements.Values.Count; i++)
            {
                for (int j = 0; j < AchievementManager.Achievements.ElementAt(i).Value.Values.Count; j++)
                {
                    achievements.Add(AchievementManager.Achievements.ElementAt(i).Value.ElementAt(j).Value);
                    minScroll.Add(0);
                    maxScroll.Add(0);
                }
            }

            int width = 450;
            int height = 125;
            int spacing = 30;

            int yPos = 0;

            for (int i = 0; i < achievements.Count; i++)
            {
                positionArray[i % 2, yPos] = new Rectangle((1240 - ((2 * width) + spacing)) / 2 + (i % 2) * (width + spacing), 200 + yPos * (height + spacing), width, height);
                achievements[i].Position = positionArray[i % 2, yPos];

                if (i % 2 == 1) { yPos++; }
            }

            for (int i = 0; i< achievements.Count; i++)
            {
                minScroll[i] = achievements[i].Position.Y - (achievements[achievements.Count - 1].Position.Y - 600);
                maxScroll[i] = achievements[i].Position.Y;
            }
        }

        /// <summary>
        /// Clears and sets up achievements
        /// </summary>
        public void SetupAchievementsClear()
        {
            AchievementManager.Clear();

            minScroll = new List<int>();
            maxScroll = new List<int>();

            achievements = new List<Achievement>();

            int positionArrayY = (int)((AchievementManager.NumAchievements + 1) / 2);

            positionArray = new Rectangle[2, positionArrayY];

            for (int i = 0; i < AchievementManager.Achievements.Values.Count; i++)
            {
                for (int j = 0; j < AchievementManager.Achievements.ElementAt(i).Value.Values.Count; j++)
                {
                    achievements.Add(AchievementManager.Achievements.ElementAt(i).Value.ElementAt(j).Value);
                    minScroll.Add(0);
                    maxScroll.Add(0);
                }
            }

            int width = 450;
            int height = 125;
            int spacing = 30;

            int yPos = 0;

            for (int i = 0; i < achievements.Count; i++)
            {
                positionArray[i % 2, yPos] = new Rectangle((1240 - ((2 * width) + spacing)) / 2 + (i % 2) * (width + spacing), 200 + yPos * (height + spacing), width, height);
                achievements[i].Position = positionArray[i % 2, yPos];

                if (i % 2 == 1) { yPos++; }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                minScroll[i] = achievements[i].Position.Y - (achievements[achievements.Count - 1].Position.Y - 600);
                maxScroll[i] = achievements[i].Position.Y;
            }
        }

        public void DrawAchievements()
        {
            _spriteBatch.Draw(assets["behindSlots"], new Rectangle(200, 0, 720, 200), null, Color.Black * .35f, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);
            _spriteBatch.Draw(assets["square"], new Rectangle(0, 0, 100, 720), Color.Gray);
            _spriteBatch.Draw(assets["behindSlots"], new Rectangle(1240, 0, 720, 200), null, Color.Black * .35f, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);
            _spriteBatch.Draw(assets["square"], new Rectangle(1140, 0, 100, 720), Color.Gray);
            _spriteBatch.Draw(assets["behindSlots"], new Rectangle(0, 0, 1240, 250), Color.Black * .35f);

            for (int i = 0; i < achievements.Count; i++)
            {
                achievements[i].Draw(_spriteBatch);
            }

            _spriteBatch.Draw(assets["square"], new Rectangle(0, 0, 1240, 150), Color.Gray);
            
        }

        public void DrawMenu()
        {
            for (int i = 0; i < MenuGraphic.Length - 1; i++)
            {
                MenuGraphic[i].Draw(_spriteBatch);
            }

            _spriteBatch.Draw(assets["square"], new Rectangle(0, 0, 1240, 720), Color.Black * .75f);
            //_spriteBatch.Draw(assets["square"], new Rectangle(0, 120, 1240, 470), Color.Black * .25f);
            //_spriteBatch.Draw(assets["square"], new Rectangle(0, 180, 1240, 350), Color.Black * .25f);

            _spriteBatch.Draw(assets["behindSlots"], new Rectangle(0, -100, 1240, 900), Color.Black * .5f);
            _spriteBatch.Draw(assets["behindSlots"], new Rectangle(0, -30, 1240, 800), Color.Black);
            MenuGraphic[MenuGraphic.Length - 1].Draw(_spriteBatch);

            _spriteBatch.Draw(assets["behindMenu"], new Rectangle(0, 0, 1240, 720), Color.Black);
            _spriteBatch.Draw(assets["square"], new Rectangle(30, 100, 440, 520), Color.Gray);

            foreach (Button button in buttons["Menu"])
            {
                button.Draw(_spriteBatch);
            }

            DrawFade();
        }

        private void DrawFade()
        {
            if (fade != 0)
            {
                _spriteBatch.Draw(
                    assets["square"],
                    new Rectangle(
                        (int)(100 * Math.Clamp(fade * 2 + .1, 0, 1)), 100,
                        (int)(1040 * Math.Clamp(fade * 1.5f + .1, 0, 1)), 520),
                    Color.Black * (float)Math.Clamp(fade * 1.5f, 0, .9f));
            }
        }

        #region Button Functions

        /// <summary>
        /// Resets everything data driven
        /// </summary>
        private void Reset()
        {
            PointManager.Clear();
            slot.Clear();
            SetupAchievementsClear();
        }

        /// <summary>
        /// Saves everything and quits
        /// </summary>
        private void Quit()
        {
            PointManager.Save();
            AchievementManager.SaveAchievements();

            Exit();
        }

        /// <summary>
        /// Button timer
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        private bool CanUseButtons(GameTime gameTime)
        {
            bool canUse = false;

            if (gameTime.TotalGameTime.TotalSeconds - sceneSwitchTime > .1f) { canUse = true; }

            return canUse;
        }

        /// <summary>
        /// Saves prev game state
        /// </summary>
        private void SavePrevState()
        {
            prevState = currentState;
        }

        /// <summary>
        /// Changes state to Menu, within menu
        /// </summary>
        private void ReturnToMenu()
        {
            sceneSwitchTime = currentSceneTime;
            currentMenuState = MenuState.Menu;
        }

        /// <summary>
        /// Not used, but changes state to credits
        /// </summary>
        private void Credits()
        {
            sceneSwitchTime = currentSceneTime;
            currentMenuState = MenuState.Credit;
        }

        /// <summary>
        /// Changes state to achievements
        /// </summary>
        private void Achievements()
        {
            sceneSwitchTime = currentSceneTime;
            currentMenuState = MenuState.Achievements;
        }

        /// <summary>
        /// Changes state to paused
        /// </summary>
        private void Pause()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = GameState.Pause;
        }

        /// <summary>
        /// Changes state to menu
        /// </summary>
        private void Menu()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = GameState.Menu;
        }

        /// <summary>
        /// Changes state to game
        /// </summary>
        private void Game()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = GameState.Game;
        }

        /// <summary>
        /// Changes state to slots
        /// </summary>
        private void Slots()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = GameState.Slots;
            slot.Slots();
        }

        /// <summary>
        /// Changes current state to prev state
        /// </summary>
        private void PrevState()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = prevState;
        }

        // This doesnt need explanation, its just bc I didnt want to put this code in 3 places
        private void ReduceFade()
        {
            if (fade > 0)
            {
                fade -= .05f;
            }
        }

        #endregion

        /// <summary>
        /// Generates all buttons in use
        /// </summary>
        private void CreateButtons()
        {
            Texture2D[] buttonAssets = new Texture2D[]
            {
                assets["button_active"],
                assets["button_inactive"]
            };

            float pressTimerMenu = .5f;
            float pressTimer = .1f;

            buttons.Add("Slots", new List<Button>());
            buttons["Slots"].Add(new Button(buttonAssets, new Rectangle(60, 565, 90, 75), "Options", regular, Color.Black, Color.White, pressTimerMenu));
            buttons["Slots"][0].OnLeftClick += SavePrevState;
            buttons["Slots"][0].OnLeftClick += Pause;
            buttons["Slots"].Add(new Button(buttonAssets, new Rectangle(1090, 565, 90, 75), "Clicker", regular, Color.Black, Color.White, pressTimerMenu));
            buttons["Slots"][1].OnLeftClick += Game;

            buttons.Add("Game", new List<Button>());
            buttons["Game"].Add(new Button(buttonAssets, new Rectangle(60, 565, 90, 75), "Options", regular, Color.Black, Color.White, pressTimerMenu));
            buttons["Game"][0].OnLeftClick += SavePrevState;
            buttons["Game"][0].OnLeftClick += Pause;
            buttons["Game"].Add(new Button(buttonAssets, new Rectangle(1090, 565, 90, 75), "Slots", regular, Color.Black, Color.White, pressTimerMenu));
            buttons["Game"][1].OnLeftClick += Slots;

            buttons.Add("Pause", new List<Button>());
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(60, 565, 90, 75), "Return", Game1.regular, Color.Black, Color.White, pressTimerMenu));
            buttons["Pause"][0].OnLeftClick += PrevState;

            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(210, 180, 90, 75), "Mute", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Pause"][1].OnLeftClick += playlist.Mute;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(360, 180, 90, 30), "Vol /\\", Game1.regular, Color.Black, Color.White, pressTimer));
            buttons["Pause"][2].OnLeftClick += playlist.VolumeUp;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(360, 225, 90, 30), "Vol \\/", Game1.regular, Color.Black, Color.White, pressTimer));
            buttons["Pause"][3].OnLeftClick += playlist.VolumeDown;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(165, 180, 30, 75), "<", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Pause"][4].OnLeftClick += playlist.Previous;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(315, 180, 30, 75), ">", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Pause"][5].OnLeftClick += playlist.PlayNext;

            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(165, 280, 285, 75), "Menu", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Pause"][6].OnLeftClick += Menu;

            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(465, 180, 90, 30), "Sound /\\", Game1.regular, Color.Black, Color.White, pressTimer));
            buttons["Pause"][7].OnLeftClick += slot.VolumeUp;

            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(465, 225, 90, 30), "Sound \\/", Game1.regular, Color.Black, Color.White, pressTimer));
            buttons["Pause"][8].OnLeftClick += slot.VolumeDown;

            // This isn't perfect, I spent way too much time on it for it to not be perfect xD
            //                   Screen height - spacing on top / how many buttons
            int numButtons = 4;
            int buttonHeight = 75;
            int menuButtonSpacing = buttonHeight + 30;
            int topSpacing = (int)(((720 - (numButtons * buttonHeight)) - ((numButtons - 1) * menuButtonSpacing)) * 1.5);

            buttons.Add("Menu", new List<Button>());
            buttons["Menu"].Add(new Button(buttonAssets, new Rectangle(107, topSpacing + (menuButtonSpacing * 0), 285, buttonHeight), "Play", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Menu"][0].OnLeftClick += Game;

            buttons["Menu"].Add(new Button(buttonAssets, new Rectangle(107, topSpacing + (menuButtonSpacing * 1), 285, buttonHeight), "Options", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Menu"][1].OnLeftClick += SavePrevState;
            buttons["Menu"][1].OnLeftClick += Pause;

            buttons["Menu"].Add(new Button(buttonAssets, new Rectangle(107, topSpacing + (menuButtonSpacing * 2), 285, buttonHeight), "Achievements", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Menu"][2].OnLeftClick += Achievements;
            
            buttons["Menu"].Add(new Button(buttonAssets, new Rectangle(107, topSpacing + (menuButtonSpacing * 3), 285, buttonHeight), "Quit", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Menu"][3].OnLeftClick += Quit;

            buttons.Add("Achievements", new List<Button>());
            buttons["Achievements"].Add(new Button(buttonAssets, new Rectangle(30, 30, 285, buttonHeight), "Back", Game1.ReadOut, Color.Black, Color.White, pressTimerMenu));
            buttons["Achievements"][0].OnLeftClick += ReturnToMenu;

            buttons["Achievements"].Add(new Button(buttonAssets, new Rectangle(330, 30, 285, buttonHeight), "Clear Progress", Game1.ReadOut, Color.Black, Color.White, pressTimerMenu));
            buttons["Achievements"][1].OnLeftClick += Reset;
        }
    }
}