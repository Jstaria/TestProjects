using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static SpriteFont ReadOut;

        public static SoundEffect hitMarker;

        private static Soundtrack playlist;

        private SlotMachine slot;
        
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playlist = new Soundtrack(new Song[]
            {
                Content.Load<Song>("Elevator-music"),
                Content.Load<Song>("Fluffing-a-Duck"),
                Content.Load<Song>("Kevin-MacLeod-Investigations"),
                Content.Load<Song>("Lobby-Time"),
                Content.Load<Song>("Monkeys-Spinning-Monkeys"),
                Content.Load<Song>("Scheming-Weasel-faster"),
                Content.Load<Song>("Sneaky-Snitch"),
            });

            ReadOut = Content.Load<SpriteFont>("ReadOut");

            Dictionary<string, Texture2D> assets = new Dictionary<string, Texture2D>
            {
                {"square",Content.Load<Texture2D>("square")},
                {"button_active",Content.Load<Texture2D>("button_active")},
                {"button_inactive",Content.Load<Texture2D>("button_inactive")}
            };

            hitMarker = Content.Load<SoundEffect>("hitmarker-sound-effect");

            slot = new SlotMachine(assets, Vector2.Zero);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            playlist.Play();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            slot.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin();

            slot.Draw(_spriteBatch);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public static void PlayNext()
        {
            playlist.PlayNext();
        }
    }
}