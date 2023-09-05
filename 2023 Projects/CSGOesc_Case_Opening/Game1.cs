using Microsoft.Xna.Framework;
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

namespace CSGOesc_Case_Opening
{
    public enum GameState
    {
        Game,
        Slots,
        Pause,
        Menu
    }

    public class Game1 : Game
    {
        private RenderTarget2D renderTarget;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random rng;

        public static SpriteFont ReadOut;
        public static SpriteFont regular;

        private SlotMachine[] MenuGraphic;

        public static SoundEffect hitMarker;

        private float fade;

        private Dictionary<string, List<Button>> buttons;

        public static Dictionary<string, Texture2D> assets;

        private GameState prevState;
        private GameState currentState;

        private float sceneSwitchTime;
        private float currentSceneTime;

        private SlotMachine slot;
        private PointManager pointManager;

        private Soundtrack playlist;
        private Song[] songs;

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
            // TODO: Add your initialization logic here

            rng = new Random();

            MenuGraphic = new SlotMachine[5];

            renderTarget = new RenderTarget2D(GraphicsDevice, 1240, 720, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            fade = 0;

            currentState = GameState.Menu;

            buttons = new Dictionary<string, List<Button>>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DirectoryInfo di = new DirectoryInfo("../../../Content/Music");
            FileInfo[] files = di.GetFiles("*.mp3");

            string[] songNames = new string[files.Length];
            songs = new Song[files.Length];

            // I have no clue why the Uri access is denied, this was my attempt using stack overflow because I have absolutely no clue how to grant permissions, let alone to myself

            //Uri uri = new Uri("../../../Music", UriKind.Relative);

            //DirectoryInfo di = new DirectoryInfo("../../../Music");
            //DirectorySecurity ds = di.GetAccessControl();
            //
            //ds.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            //di.SetAccessControl(ds);

            for (int i = 0; i < songs.Length; i++)
            {
                songNames[i] = files[i].Name.Remove(files[i].Name.Length - 4, 4);
                songs[i] = Content.Load<Song>("Music/" + songNames[i]);

                //songs[i] = Song.FromUri("../../../Music", uri);
            }

            this.playlist = new Soundtrack(songs);

            ReadOut = Content.Load<SpriteFont>("ReadOut");
            regular = Content.Load<SpriteFont>("regular");

            assets = new Dictionary<string, Texture2D>
            {
                {"square",Content.Load<Texture2D>("square")},
                {"button_active",Content.Load<Texture2D>("button_active")},
                {"button_inactive",Content.Load<Texture2D>("button_inactive")}
            };

            hitMarker = Content.Load<SoundEffect>("hitmarker-sound-effect");

            slot = new SlotMachine(assets, new Vector2(0, 185), false, 1);
            
            for (int i = 0; i < MenuGraphic.Length-1; i++)
            {
                MenuGraphic[i] = new SlotMachine(assets, new Vector2(0, -100 + (200 * i)), true, rng.Next(3,10) * .5f);
            }

            MenuGraphic[MenuGraphic.Length-1] = new SlotMachine(assets, new Vector2(0, 230), true, 1);

            pointManager = new PointManager();

            CreateButtons();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            currentSceneTime = (float)gameTime.TotalGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            playlist.Play();

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

                    ReduceFade();

                    for(int i = 0; i < MenuGraphic.Length; i++)
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
            }

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

                    break;

                case GameState.Menu:

                    DrawMenu();

                    break;
            }

            #endregion

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            _spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 1240, 720), Color.White);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void DrawMenu()
        {
            for (int i = 0; i < MenuGraphic.Length - 1; i++)
            {
                MenuGraphic[i].Draw(_spriteBatch);
            }

            _spriteBatch.Draw(assets["square"], new Rectangle(0, 0, 1240, 720), Color.Black * .75f);
            _spriteBatch.Draw(assets["square"], new Rectangle(0, 120, 1240, 470), Color.Black * .25f);
            _spriteBatch.Draw(assets["square"], new Rectangle(0, 180, 1240, 350), Color.Black * .25f);

            MenuGraphic[MenuGraphic.Length - 1].Draw(_spriteBatch);

            _spriteBatch.Draw(assets["square"], new Rectangle(0, 0, 500, 720), Color.Gray);

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

        private bool CanUseButtons(GameTime gameTime)
        {
            bool canUse = false;

            if (gameTime.TotalGameTime.TotalSeconds - sceneSwitchTime > .1f) { canUse = true; }

            return canUse;
        }

        private void SavePrevState()
        {
            prevState = currentState;
        }

        private void Pause()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = GameState.Pause;
        }

        private void Menu()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = GameState.Menu;
        }

        private void Game()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = GameState.Game;
        }

        private void Slots()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = GameState.Slots;
            slot.CloseInventory();
        }

        private void PrevState()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = prevState;
        }

        private void ReduceFade()
        {
            if (fade > 0)
            {
                fade -= .05f;
            }
        }

        #endregion

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
            buttons["Slots"].Add(new Button(buttonAssets, new Rectangle(1090, 565, 90, 75), "Home", regular, Color.Black, Color.White, pressTimerMenu));
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
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(360, 180, 90, 30), "Vol Up", Game1.regular, Color.Black, Color.White, pressTimer));
            buttons["Pause"][2].OnLeftClick += playlist.VolumeUp;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(360, 225, 90, 30), "Vol Down", Game1.regular, Color.Black, Color.White, pressTimer));
            buttons["Pause"][3].OnLeftClick += playlist.VolumeDown;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(165, 180, 30, 75), "<", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Pause"][4].OnLeftClick += playlist.Previous;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(315, 180, 30, 75), ">", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Pause"][5].OnLeftClick += playlist.PlayNext;

            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(165, 280, 285, 75), "Menu", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Pause"][6].OnLeftClick += Menu;

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
            buttons["Menu"][2].OnLeftClick += Game;
            
            buttons["Menu"].Add(new Button(buttonAssets, new Rectangle(107, topSpacing + (menuButtonSpacing * 3), 285, buttonHeight), "Credits", Game1.ReadOut, Color.Black, Color.White, pressTimer));
            buttons["Menu"][3].OnLeftClick += Game;

        }
    }
}