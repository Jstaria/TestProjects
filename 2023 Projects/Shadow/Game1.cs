using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Numerics;
using Vector4 = Microsoft.Xna.Framework.Vector4;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Shadow
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D square;
        private Texture2D ben2D;
        private Texture2D light;
        private Texture2D floor;

        public static SpriteFont text;
        
        private WallManager wallManager;
        private Player ben;
        private Camera camera;

        private Effect rainbowShader;
        private Effect pixelShader;

        public static int width;
        public static int height;

        private float time;

        private Vector2 mousePos;
        private Vector2 aimedAtPos;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 900;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            width = _graphics.PreferredBackBufferWidth;
            height = _graphics.PreferredBackBufferHeight;

            time = 0;

            MouseState mouseState = Mouse.GetState();

            aimedAtPos = new Vector2(mouseState.X,mouseState.Y);

            wallManager = new WallManager();

            camera = new Camera();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            rainbowShader = Content.Load<Effect>("Rainbow");
            pixelShader = Content.Load<Effect>("Pixel");

            square = Content.Load<Texture2D>("square");
            ben2D = Content.Load<Texture2D>("happyben");
            light = Content.Load<Texture2D>("lightorb");
            text = Content.Load<SpriteFont>("text");
            floor = Content.Load<Texture2D>("floor");

            wallManager.AddWall(new Wall(new Rectangle(width / 2 - 250, height / 2 - 250, 200, 200), square));
            wallManager.AddWall(new Wall(new Rectangle(width / 2 + 50, height / 2 - 250, 200, 200), square));
            wallManager.AddWall(new Wall(new Rectangle(width / 2 + 50, height / 2 + 50, 200, 200), square));
            wallManager.AddWall(new Wall(new Rectangle(width / 2 - 250, height / 2 + 50, 200, 200), square));

            ben = new Player(new Rectangle((width - 30) / 2, (height - 60) / 2, 30, 60), ben2D);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState currentMS = Mouse.GetState();

            mousePos = new Vector2(currentMS.X, currentMS.Y);

            time += .01f;

            ben.Update();

            camera.Follow(ben);
            camera.FollowDirection(Player.cameraPos, camera.Transform);

            aimedAtPos = Vector2.Lerp(aimedAtPos, mousePos, .04f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: camera.Transform);

            _spriteBatch.Draw(floor, new Vector2(width / 2 - floor.Width/2, height / 2 - floor.Height/2), Color.White);

            // Just grabs the lightPosition so the wall shadows dont freak out (jitter)
            // Basically modified to just be a GetLightPos method just for the walls
            ben.FlashLight(GraphicsDevice, _spriteBatch, null, aimedAtPos, camera.Transform, false);

            wallManager.DrawShadows(_spriteBatch, GraphicsDevice, camera.Transform);

            wallManager.DrawWalls(_spriteBatch);

            // Actually draws the flashlight
            ben.FlashLight(GraphicsDevice, _spriteBatch, null, aimedAtPos, camera.Transform, true);

            ben.Draw(_spriteBatch);

            _spriteBatch.End();

            _spriteBatch.Begin(blendState: blendStateSubtract, transformMatrix: camera.Transform);

            _spriteBatch.Draw(light, new Rectangle(ben.Position.X + ben.Position.Width / 2 - 1500, ben.Position.Y + ben.Position.Height / 2 - 1500, 3000, 3000), Color.White);

            _spriteBatch.End();

            /*
            // If rainbow is active
            rainbowShader.Parameters["time"].SetValue(time);
            rainbowShader.CurrentTechnique.Passes[0].Apply();

            wallManager.DrawShadows(_spriteBatch, GraphicsDevice, rainbowShader);
            wallManager.DrawWalls(_spriteBatch);

            _spriteBatch.End();
            _spriteBatch.Begin(effect: pixelShader, transformMatrix: camera.Transform);

            // If pixel is active
            pixelShader.Parameters["pixelsX"].SetValue(64);
            pixelShader.Parameters["pixelsY"].SetValue(128);
            pixelShader.Parameters["pixelation"].SetValue(4);
            pixelShader.CurrentTechnique.Passes[0].Apply();

            ben.FlashLight(GraphicsDevice, _spriteBatch, pixelShader, aimedAtPos);
            ben.Draw(_spriteBatch);

            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, blendState: blendStateSubtract, samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(light, new Rectangle(ben.Position.X + ben.Position.Width / 2 - 1500, ben.Position.Y + ben.Position.Height / 2 - 1500, 3000, 3000), Color.White);
            */

            base.Draw(gameTime);
        }

        // Credit to https://community.monogame.net/t/why-doesnt-my-simple-subtractive-2d-lighting-work/16521/2
        public readonly static BlendState blendStateSubtract = new BlendState
        {
            ColorSourceBlend = Blend.One,
            AlphaSourceBlend = Blend.One,

            ColorDestinationBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,

            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaBlendFunction = BlendFunction.ReverseSubtract
        };

    }
}