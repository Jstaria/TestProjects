using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;

namespace Projection_Collision
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 midPoint;
        private Vector2 normal;

        private Projection satCollision = new Projection();

        private Vector2[] corners;

        private Polygon line;
        private Polygon triangle;
        private Polygon square;
        private Polygon pentagon;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // triangle
            float theta = (float)Math.PI * 2.0f / 3.0f;

            triangle.points = new List<Vector2>();
            triangle.model = new List<Vector2>();
            triangle.origin = new Vector2(200, 200);
            triangle.angle = 0.0f;

            for (int i = 0; i < 3; i++)
            {
                triangle.points.Add(new Vector2(100.0f * (float)Math.Cos(theta * i), 100.0f * (float)Math.Sin(theta * i)));
                triangle.model.Add(new Vector2(100.0f * (float)Math.Cos(theta * i), 100.0f * (float)Math.Sin(theta * i)));
            }

            triangle.overlap = false;

            satCollision.AddPolygon = triangle;

            // Square
            square.points = new List<Vector2>();
            square.model = new List<Vector2>();

            square.origin = new Vector2(500, 200);
            square.angle = 0.0f;

            square.points.Add(new Vector2(-70, -70));
            square.points.Add(new Vector2(-70, +70));
            square.points.Add(new Vector2(+70, +70));
            square.points.Add(new Vector2(+70, -70));

            square.model.Add(new Vector2(-70, -70));
            square.model.Add(new Vector2(-70, +70));
            square.model.Add(new Vector2(+70, +70));
            square.model.Add(new Vector2(+70, -70));

            square.overlap = false;

            satCollision.AddPolygon = square;

            // pentagon
            theta = (float)Math.PI * 2.0f / 5.0f;

            pentagon.points = new List<Vector2>();
            pentagon.model = new List<Vector2>();
            pentagon.origin = new Vector2(100, 150);
            pentagon.angle = 0.0f;

            for (int i = 0; i < 5; i++)
            {
                pentagon.points.Add(new Vector2(50.0f * (float)Math.Cos(theta * i), 50.0f * (float)Math.Sin(theta * i)));
                pentagon.model.Add(new Vector2(50.0f * (float)Math.Cos(theta * i), 50.0f * (float)Math.Sin(theta * i)));
            }

            pentagon.overlap = false;

            satCollision.AddPolygon = pentagon;

            // line
            line.points = new List<Vector2>();
            line.model = new List<Vector2>();

            line.origin = new Vector2(350, 250);
            line.angle = 0.0f;

            line.points.Add(new Vector2(0, 0));
            line.points.Add(new Vector2(-70, +70));


            line.model.Add(new Vector2(0, 0));
            line.model.Add(new Vector2(-70, +70));


            line.overlap = false;

            satCollision.AddPolygon = line;

            satCollision.UpdateShapes();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            satCollision.Update(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            satCollision.Draw(GraphicsDevice);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}