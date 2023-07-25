using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    public enum GameState
    {
        Game,
        Slots,
        Pause
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static SpriteFont ReadOut;
        public static SpriteFont regular;

        public static SoundEffect hitMarker;

        private float fade;

        private Dictionary<string, List<Button>> buttons;

        private Dictionary<string, Texture2D> assets;

        private GameState prevState;
        private GameState currentState;

        private BasicEffect BasicEffect;

        private SlotMachine slot;

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

            fade = 0;

            currentState = GameState.Slots;

            buttons = new Dictionary<string, List<Button>>(); 

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            songs = new Song[]
            {
                Content.Load<Song>("Elevator-music"),
                Content.Load<Song>("Fluffing-a-Duck"),
                Content.Load<Song>("Kevin-MacLeod-Investigations"),
                Content.Load<Song>("Lobby-Time"),
                Content.Load<Song>("Monkeys-Spinning-Monkeys"),
                Content.Load<Song>("Scheming-Weasel-faster"),
                Content.Load<Song>("Sneaky-Snitch"),
            };

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

            slot = new SlotMachine(assets, Vector2.Zero);

            CreateButtons();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        { 
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            playlist.Play();

            switch (currentState)
            {
                case GameState.Game:

                    ReduceFade();

                    break;

                case GameState.Slots:

                    ReduceFade();

                    slot.Update(gameTime);

                    foreach (Button button in buttons["Slots"])
                    {
                        button.Update(gameTime);
                    }

                    break;

                case GameState.Pause:

                    slot.Update(gameTime);

                    if (fade < .9)
                    {
                        fade += .05f;
                    }

                    foreach (Button button in buttons["Pause"])
                    {
                        button.Update(gameTime);
                    }

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin();

            switch (currentState)
            {
                case GameState.Game:

                    break;

                case GameState.Slots:

                    slot.Draw(_spriteBatch);

                    foreach (Button button in buttons["Slots"])
                    {
                        button.Draw(_spriteBatch);
                    }

                    if (fade != 0)
                    {
                        _spriteBatch.Draw(
                            assets["square"], 
                            new Rectangle(
                                (int)(100 * Math.Clamp(fade * 2 + .1, 0, 1)), 100, 
                                (int)(1040 * Math.Clamp(fade * 1.5f + .1, 0, 1)), 520), 
                            Color.Black * (float)Math.Clamp(fade * 1.5f, 0, .9f));
                    }

                    break;

                case GameState.Pause:

                    slot.Draw(_spriteBatch);

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
            }

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void SavePrevState()
        {
            prevState = currentState;
        }

        private void Pause()
        {
            currentState = GameState.Pause;
        }

        private void Slots()
        {
            currentState = GameState.Slots;
        }

        private void ReduceFade()
        {
            if (fade > 0)
            {
                fade -= .05f;
            }
        }

        private void CreateButtons()
        {
            Texture2D[] buttonAssets = new Texture2D[]
            {
                assets["button_active"],
                assets["button_inactive"]
            };

            buttons.Add("Slots", new List<Button>());
            buttons["Slots"].Add(new Button(buttonAssets, new Rectangle(60, 565, 90, 75), "Options", regular, Color.Black));
            buttons["Slots"][0].OnLeftClick += Pause;

            buttons.Add("Pause", new List<Button>());
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(60, 565, 90, 75), "Return", Game1.regular, Color.Black));
            buttons["Pause"][0].OnLeftClick += SavePrevState;
            buttons["Pause"][0].OnLeftClick += Slots;

            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(60 + 200, 565, 90, 75), "Mute", Game1.ReadOut, Color.Black));
            buttons["Pause"][1].OnLeftClick += playlist.Mute;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(60 + 200, 520, 90, 30), "Vol Up", Game1.regular, Color.Black));
            buttons["Pause"][2].OnLeftClick += playlist.VolumeUp;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(60 + 200, 655, 90, 30), "Vol Down", Game1.regular, Color.Black));
            buttons["Pause"][3].OnLeftClick += playlist.VolumeDown;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(15 + 200, 565, 30, 75), "<", Game1.ReadOut, Color.Black));
            buttons["Pause"][4].OnLeftClick += playlist.Previous;
                     
            buttons["Pause"].Add(new Button(buttonAssets, new Rectangle(165 + 200, 565, 30, 75), ">", Game1.ReadOut, Color.Black));
            buttons["Pause"][5].OnLeftClick += playlist.PlayNext;
        }
    }
}