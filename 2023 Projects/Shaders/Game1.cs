using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading;

namespace Shaders
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Effect grayShader;
        private Effect basicShader;
        private Effect rainbowShader;
        private Effect throbShader;
        private Effect pixelShader;
        private Effect randShader;

        private Stopwatch timer;

        private Texture2D ben;

        private int width;
        private int height;

        private float count;
        private int countInt;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            width = _graphics.PreferredBackBufferWidth;
            height = _graphics.PreferredBackBufferHeight;

            count = 250;
            countInt = 0;

            timer = new Stopwatch();
            timer.Start();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            grayShader = Content.Load<Effect>("TestShader");
            basicShader = Content.Load<Effect>("BasicShader");
            rainbowShader = Content.Load<Effect>("Rainbow");
            throbShader = Content.Load<Effect>("Throb");
            pixelShader = Content.Load<Effect>("Pixel");
            randShader = Content.Load<Effect>("random");

            ben = Content.Load<Texture2D>("happyben");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (timer.Elapsed.TotalSeconds == .1)
            {
                timer.Restart();
            }

            if (count < 2)
            {
                count = 250;
            }

            count *= .99f;
            countInt++;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Grey effect
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            _spriteBatch.Begin(effect: grayShader);

            _spriteBatch.Draw(ben, new Rectangle(50, 50, 100, 200), Color.White);

            _spriteBatch.End();
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // Rainbow shader
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, effect: rainbowShader);

            rainbowShader.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            rainbowShader.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(ben, new Rectangle(150, 50, 100, 200), Color.White);

            _spriteBatch.End();
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // Anti-aliasing shader
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            basicShader.Parameters["xSize"].SetValue((float)width);
            basicShader.Parameters["ySize"].SetValue((float)height);
            basicShader.Parameters["xDraw"].SetValue((float)100);
            basicShader.Parameters["yDraw"].SetValue((float)100);
            basicShader.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            basicShader.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(ben, new Rectangle(50, 250, 100, 200), Color.White);

            _spriteBatch.Draw(ben, new Rectangle(250, 50, 200, 400), Color.White);

            _spriteBatch.End();
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // normal ben
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            _spriteBatch.Begin(SpriteSortMode.Immediate);

            _spriteBatch.Draw(ben, new Rectangle(150, 250, 100, 200), Color.White);

            rainbowShader.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            rainbowShader.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(ben, new Rectangle(450, 50, 200, 400), Color.White);

            _spriteBatch.End();
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // Throbbing ben
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            _spriteBatch.Begin(SpriteSortMode.Immediate);

            throbShader.Parameters["SINLOC"].SetValue((float)Math.Sin(timer.Elapsed.TotalSeconds * 5) * (float)Math.PI / 5);
            throbShader.Parameters["filterColor"].SetValue(Color.Red.ToVector4());
            throbShader.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(ben, new Rectangle(650, 50, 100, 200), Color.White);

            _spriteBatch.End();
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // Pixelly ben
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            _spriteBatch.Begin(SpriteSortMode.Immediate, effect: pixelShader);

            pixelShader.Parameters["pixelsX"].SetValue((float)256);
            pixelShader.Parameters["pixelsY"].SetValue((float)512);
            pixelShader.Parameters["pixelation"].SetValue((float)8);
            pixelShader.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(ben, new Rectangle(750, 50, 100, 200), Color.White);

            pixelShader.Parameters["pixelsX"].SetValue((float)256);
            pixelShader.Parameters["pixelsY"].SetValue((float)512);
            pixelShader.Parameters["pixelation"].SetValue((float)count);
            pixelShader.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(ben, new Rectangle(750, 250, 100, 200), Color.White);

            _spriteBatch.End();
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // Ambient Light
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            _spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, effect: randShader);

            randShader.Parameters["countInt"].SetValue(countInt);
            randShader.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(ben, new Rectangle(650, 250, 100, 200), Color.White);

            _spriteBatch.End();
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            

            base.Draw(gameTime);
            
        }
    }
}