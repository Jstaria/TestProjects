using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Enemy class that follows the player around slowly, and if they collide, the game ends

namespace TestCollision
{
    internal class Enemy : GameObject
    {
        private float speed;

        public float Angle
        {
            get { return angle; }
        }

        public Enemy(Texture2D asset, Rectangle position) : base(asset, position)
        {
            this.asset = asset;
            this.position = position;
            this.positionV = new Vector2(position.X, position.Y);
            this.origin = new Vector2(asset.Width / 2, asset.Height / 2);

            speed = 1;
        }

        // Checks collision for player and the enemy
        public bool CheckCollision(GameObject check)
        {
            return this.Position.Intersects(check.Position);
        }

        // First enemy check collision, designed using vectors and rectangles, bounce off of each other, and get stuck easily
        
        public void EnemyCollisionCheck(GameObject enemy) 
        {

            speed = 1f;

            position.X = (int)X;
            position.Y = (int)Y;


            if (enemy != null)
            {
                if (this.position.Intersects(new Rectangle(enemy.Position.X,enemy.Position.Y,enemy.Position.Width,enemy.Position.Height)))
                {
                    if (this.position.Bottom >= enemy.Position.Top + 5)
                    {
                        speed *= .25f;
                        this.Y += 3;
                    }
                    if (this.position.Top <= enemy.Position.Bottom - 5)
                    {
                        speed *= 1.25f;
                        enemy.Y -= 3;
                    }

                    if (this.position.Left <= enemy.Position.Right - 5)
                    {
                        speed *= 1.25f;
                        this.X += 3;
                    }

                    if (this.position.Right >= enemy.Position.Left + 5)
                    {
                        speed *= 1.25f;
                        enemy.X -= 3;
                    }
                   
                }
            }
        }


        public void DrawRotation(SpriteBatch sb)
        {
            sb.Draw(asset, this.positionV, null, Color.White, angle, origin, 1, SpriteEffects.None, 0);
        }
    

        public override void Update(GameTime gameTime) { }

        // Needs the player position for this to work, so therefore new Update method
        public void Update(GameTime gameTime, Vector2 playerPosition, Enemy enemy)
        {
            // ==========================================================
            // My attempt at using trig for the direction of the enemy
            // to get a smoother direction at the player
            // Not the first iteration, I had some help with the math for this
            // off of a tutorial after my math which I had thought was full proof wasn't
            // This sort of worked, but the enemy stops right before touching the player

            // fixed

            EnemyCollisionCheck(enemy);

            Vector2 movement;

            Vector2 direction =
                playerPosition + origin -
                new Vector2(this.X, this.Y);

            

            float distance = Vector2.Distance(
                playerPosition + origin,
                new Vector2(this.X, this.Y));

            angle = MathF.Atan2(direction.Y, direction.X);

            direction.Normalize();
            movement = direction;


            this.positionV += direction * speed;
            

            
            // ==========================================================

             
            /*
            if (positionV.X < playerPosition.X && flags[0])
            {
                positionV.X += speedX;
            }

            if (positionV.X > playerPosition.X && flags[1])
            {
                positionV.X -= speedX;
            }

            if (positionV.Y < playerPosition.Y && flags[2])
            {
                positionV.Y += speedY;
            }
            if (positionV.Y > playerPosition.Y && flags[3])
            {
                positionV.Y -= speedY;
            }
            */
        }

        // Spawns the snail at a random position, usually above the player, sometimes below
        public void RandomPosition(Random rng, int width, int height, GameObject player)
        {
            int numX = rng.Next(width);
            int numY = rng.Next(height);

            if (CheckCollision(player) && 
                (numX > width / 2 + 50 || numX < width / 2 - 50) && 
                (numY > height / 2 + 50 || numY < height /2 - 50)) 
            {
                position = new Rectangle(numX, numY, this.position.Width, this.position.Height);
                positionV = new Vector2(numX, numY);
            }
            
            else
            {
                numX = rng.Next(width);
                numY = rng.Next(height);
                position = new Rectangle(numX, numY, this.position.Width, this.position.Height);
                positionV = new Vector2(numX, numY);
            }
        }
    }
}