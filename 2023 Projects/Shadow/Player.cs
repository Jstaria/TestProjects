using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadow
{
    internal class Player : Object
    {
        public static Vector2 lightPos;
        public static Vector2 cameraPos;

        public Rectangle Position
        {
            get { return position; }
        }

        public Player(Rectangle position, Texture2D asset) : base(position, asset)
        {
            this.position = position;
            this.asset = asset;
            Player.lightPos = new Vector2(0, 0);
        }

        public override void Update()
        {
            KeyboardState kb = Keyboard.GetState();

            int speedY = 2;
            int speedX = 2;


            if (kb.IsKeyDown(Keys.W))
            {
                //if (position.Y < -50)
                //{
                //    position.Y = Game1.height - 50;
                //}

                foreach (Wall wall in WallManager.walls)
                {
                    if (new Rectangle(position.X, position.Y - speedY, position.Width, position.Height).Intersects(wall.Position))
                    {
                        speedY = 0;
                    }
                }

                position.Y -= speedY;

            }
            if (kb.IsKeyDown(Keys.S))
            {
                //if (position.Y > Game1.height)
                //{
                //    position.Y = -50;
                //}

                foreach (Wall wall in WallManager.walls)
                {
                    if (new Rectangle(position.X, position.Y + speedY, position.Width, position.Height).Intersects(wall.Position))
                    {
                        speedY = 0;
                    }
                }

                position.Y += speedY;
            }

            if (kb.IsKeyDown(Keys.A))
            {
                //if (position.X < -50)
                //{
                //    position.X = Game1.width - 50;
                //}

                foreach (Wall wall in WallManager.walls)
                {
                    if (new Rectangle(position.X - speedX, position.Y, position.Width, position.Height).Intersects(wall.Position))
                    {
                        speedX = 0;
                    }
                }

                position.X -= speedX;
            }

            if (kb.IsKeyDown(Keys.D))
            {
                //if (position.X > Game1.width)
                //{
                //    position.X = -50;
                //}

                foreach (Wall wall in WallManager.walls)
                {
                    if (new Rectangle(position.X + speedX, position.Y, position.Width, position.Height).Intersects(wall.Position))
                    {
                        speedX = 0;
                    }
                }

                position.X += speedX;
            }

        }

        /// <summary>
        /// Draws the aura of the flashlight, and the darkness behind the flashlight
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="sb"></param>
        /// <param name="effect"></param>
        /// <param name="mousePos"></param>
        public void FlashLight(GraphicsDevice GraphicsDevice, SpriteBatch sb, Effect effect, Vector2 mousePos, Matrix transform, bool drawShapes)
        {
            if (drawShapes)
            {
                sb.End();
                ShapeBatch.Begin(GraphicsDevice);
            }
            // ShapeBatch.Line(origin, mousePos, Color.White);
            //ShapeBatch.Circle(new Vector2(position.X + position.Width / 2, position.Y + position.Height / 2), 200, new Color(15, 10, 5, .1f), new Color(0, 0, 0, 0));
            //ShapeBatch.Circle(new Vector2(position.X + position.Width / 2, position.Y + position.Height / 2), Game1.width * .5f, new Color(0, 0, 0, .5f), new Color(0, 0, 0, 1.5f));

            //ShapeBatch.Triangle(new Vector2(position.X + position.Width / 2, position.Y + position.Height / 2),)

            Vector2 benV = new Vector2(Game1.width / 2, Game1.height / 2);
            Vector2 benT = Vector2.Transform(new Vector2(position.X + position.Width / 2, position.Y + position.Height / 2), transform);

            // Fade out
            float angle = MathHelper.ToRadians(25);
            Vector2 direction1 = mousePos - benV;
            direction1.Normalize();

            direction1 = new Vector2(
                direction1.X * (float)Math.Cos(angle) - direction1.Y * (float)Math.Sin(angle),
                direction1.X * (float)Math.Sin(angle) + direction1.Y * (float)Math.Cos(angle));

            angle = MathHelper.ToRadians(335);
            Vector2 direction2 = mousePos - benV;
            direction2.Normalize();

            direction2 = new Vector2(
                direction2.X * (float)Math.Cos(angle) - direction2.Y * (float)Math.Sin(angle),
                direction2.X * (float)Math.Sin(angle) + direction2.Y * (float)Math.Cos(angle));

            // Harsh shadow behind
            angle = MathHelper.ToRadians(55);

            Vector2 direction = mousePos - benV;
            direction.Normalize();

            lightPos = benT + direction * 10;
            cameraPos = benV + direction * 100;

            if (drawShapes)
            {
                ShapeBatch.Circle(lightPos, 20, new Color(255, 255, 10, .1f), new Color(0, 0, 0, 0));

                direction = new Vector2(
                    direction.X * (float)Math.Cos(angle) - direction.Y * (float)Math.Sin(angle),
                    direction.X * (float)Math.Sin(angle) + direction.Y * (float)Math.Cos(angle));

                Vector2 directionOpp = direction;

                angle = MathHelper.ToRadians(305);

                direction = mousePos - benV;
                direction.Normalize();

                direction = new Vector2(
                    direction.X * (float)Math.Cos(angle) - direction.Y * (float)Math.Sin(angle),
                    direction.X * (float)Math.Sin(angle) + direction.Y * (float)Math.Cos(angle));
                Vector2 point1 = benT - directionOpp * 2600;
                Vector2 point2 = benT + direction * 1000;
                Vector2 point3 = benT - direction * 1000;

                ShapeBatch.Triangle(point1, point2, point3, Color.Black);

                ShapeBatch.Triangle(benT, point2, benT + direction2 * 1000, new Color(0, 0, 0, 0), Color.Red, new Color(0, 0, 0, 0));

                point1 = benT - direction * 2600;
                point2 = benT + directionOpp * 1000;
                point3 = benT - directionOpp * 1000;

                ShapeBatch.Triangle(point1, point2, point3, Color.Black);

                ShapeBatch.Triangle(benT, point2, benT + direction1 * 1000, new Color(0, 0, 0, 0), Color.DeepSkyBlue, new Color(0, 0, 0, 0));

                ShapeBatch.End();
                sb.Begin(SpriteSortMode.Immediate, effect: effect, transformMatrix: transform);
            }
        }
    }
}
