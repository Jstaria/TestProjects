using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGolf_TeamB
{
    internal class PowerMeter : GameObject
    {
        // For shapebatch
        private GraphicsDevice graphicsDevice;

        //private Vector2 ballPosition;
        private Vector2 mousePosition;
        private Vector2 mouseDirection;
        //private float angle;
        private Rectangle sprite;

        private GolfBall ball;
        private int colorSwitch;

        private float estimatedPow;

        private int length;

        /// <summary>
        /// Tracks the state of the power meter
        /// </summary>
        public enum State
        {
            tracking,
            pow1,
            pow2,
            pow3,
            shooting
        }
        public State meterState;

        public PowerMeter(Texture2D asset, Rectangle position, GolfBall ball) : base(asset, position)
        {
            this.asset = asset;
            this.position = position;

            meterState = State.tracking;
            angle = 0;

            this.ball = ball;

            sprite = new Rectangle(0, 0, 200, 200);

            graphicsDevice = this.asset.GraphicsDevice;
        }

        /// <summary>
        /// Gets the current position of the mouse for the Power Meter
        /// </summary>
        /// <param name="mousePosition"></param>
        public void GetMousePos(Vector2 mousePosition)
        {
            this.mousePosition = mousePosition;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.End();
            ShapeBatch.Begin(graphicsDevice);

            Vector2 startPos = new Vector2(ball.X + ball.Position.Width / 2, ball.Y + ball.Position.Height / 2);

            Vector2 currentMouseDirection = ball.CurrentMousePosDir;

            Color color = Color.White;

            float angle1 = 92;
            float angle2 = 87;

            int randomAngle = 0;

            Random random = new Random();

            if (estimatedPow > 0 && estimatedPow <= 2)
            {
                color = Color.CornflowerBlue;

                if (length > 50)
                {
                    length -= 10;
                }
            }

            else if (estimatedPow > 2 && estimatedPow <= 5)
            {
                color = Color.LimeGreen;
                if (length < 70)
                {
                    length += 10;
                }
                else if (length > 70)
                {
                    length -= 10;
                }
            }

            else if (estimatedPow > 5 && estimatedPow <= 7)
            {
                color = Color.Yellow;
                if (length < 100)
                {
                    length += 10;
                }
                else if (length > 100)
                {
                    length -= 10;
                }
            }

            else if (estimatedPow > 7 && estimatedPow <= 13)
            {
                color = Color.Orange;
                if (length < 150)
                {
                    length += 10;
                }
                else if (length > 150)
                {
                    length -= 10;
                }
            }

            else if (estimatedPow > 13 && estimatedPow < 20)
            {
                color = Color.Red;
                if (length < 200)
                {
                    length += 10;
                }
                else if (length > 200)
                {
                    length -= 20;
                }

                randomAngle = random.Next(-1, 1);
            }

            else 
            {
                if (colorSwitch < 10)
                {
                    color = Color.DarkSlateGray;
                }

                else if (colorSwitch > 10)
                {
                    color = Color.Gold;
                }
                
                if (colorSwitch > 20)
                {
                    colorSwitch = 0;
                }

                colorSwitch++;
                if (length < 300)
                {
                    length += 20;
                }

                randomAngle = random.Next(-2, 2);
            }

            

            angle1 = (float)Math.Atan2(mouseDirection.X, mouseDirection.Y) + MathHelper.ToRadians(-angle1 + randomAngle);
            angle2 = (float)Math.Atan2(mouseDirection.X, mouseDirection.Y) + MathHelper.ToRadians(-angle2 + randomAngle);

            Vector2 pos1 = new Vector2(ball.X + ball.Position.Width / 2, ball.Y + ball.Position.Height / 2);
            Vector2 pos2 = ShapeBatch.Line(startPos, length, angle1, 5, Color.Transparent, color);
            Vector2 pos3 = ShapeBatch.Line(startPos, length, angle2, 5, Color.Transparent, color);

            ShapeBatch.Line(pos2, pos3, 5, color);

            ShapeBatch.Triangle(pos1, pos2, pos3, new Color(0, 0, 0, 150));

            ShapeBatch.End();
            sb.Begin();
            //sb.Draw(asset, new Rectangle((int)X, (int)Y, position.Width, position.Height), Color.Red);

            mouseDirection = Vector2.Lerp(mouseDirection, currentMouseDirection, .2f);
        }

        public override void DrawRotation(SpriteBatch sb, Vector2 mousePos, Rectangle source, GolfBall ball)
        {
            base.DrawRotation(sb, mousePos, sprite, ball);
        }

        public override void Update(GameTime gameTime)
        {
            // Instead of focusing on the angle and position of the mouse, which is subsequently done using draw rotation in Game1,
            // You can implement the draw rotation here using the enum to draw what we need. 
            // By this I mean we need a couple different textures that correspond to our power level which is based off of time.
            // Unfortunately, in power meter we need an update method that adds the hold and release functionality.
            // For every half a millisecond of time, we gain y amount of power
            // I fixed draw rotation to work, there looks to be a good start here, but theres a little bit more to do.
            //Definitely try to work on that new control functionality, you'l have to create this and then pass it into the ball.Power property 

            //update position of meter and angle of the mouse

            positionV = new Vector2(position.X, position.Y);

            //FSM for the power meter
            switch (meterState)
            {
                //meter will follow around the mouse
                case State.tracking:
                    position = new Rectangle(new Point((int)ball.X, (int)ball.Y), new Point(100));
                    sprite = new Rectangle(0, 0, 200, 200);
                    break;
                case State.pow1:
                    position = new Rectangle(new Point((int)ball.X, (int)ball.Y), new Point(100));
                    sprite = new Rectangle(0, 200, 200, 200);
                    break;
                case State.pow2:
                    position = new Rectangle(new Point((int)ball.X, (int)ball.Y), new Point(100));
                    sprite = new Rectangle(0, 400, 200, 200);
                    break;
                case State.pow3:
                    position = new Rectangle(new Point((int)ball.X, (int)ball.Y), new Point(100));
                    sprite = new Rectangle(0, 600, 200, 200);
                    break;

                //meter disappears
                case State.shooting:
                    position = new Rectangle(new Point(0,0), new Point(100));
                    break;
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Returns the distance between the mouse and the ball WHEN LMB CLICKED
        /// </summary>
        public float MouseDistanceSet()
        {
            float dx = ball.X - mousePosition.X;
            float dy = ball.Y - mousePosition.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Returns the distance between mouse and the ball updated every frame
        /// </summary>
        /// <returns></returns>
        public float EstimatedPow(MouseState currentMouseState)
        {
            float dx = ball.X - currentMouseState.X;
            float dy = ball.Y - currentMouseState.Y;
            float d = (float)Math.Sqrt(dx * dx + dy * dy);

            estimatedPow = Vector2.Clamp(new Vector2(15 * (d / 275), 0), Vector2.Zero, new Vector2(20, 0)).X;

            return estimatedPow;
        }
    }
}
