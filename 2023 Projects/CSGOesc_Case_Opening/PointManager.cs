using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace CSGOesc_Case_Opening
{
    internal class PointManager
    {
        Random rng = new Random();
        public static int TotalPoints { get; private set; }
        public static int ClickTotal { get; private set; }

        public List<Particle> clickParticles;

        private Dictionary<String, int> items;
        private Button ClickMe;
        private Button Achievements;

        public PointManager()
        {
            clickParticles = new List<Particle>();

            ClickTotal = 1;
            TotalPoints = 15;

            items = new Dictionary<String, int>();
            ClickMe = new Button(new Texture2D[]
            {
                Game1.assets["button_active"],
                Game1.assets["button_inactive"]
            }, 
            new Rectangle((1240 - 200) / 2, 160, 200, 200), "Click Me", Game1.ReadOut, Color.Black, Color.White, 0);

            Achievements = new Button(new Texture2D[]
            {
                Game1.assets["button_active"],
                Game1.assets["button_inactive"]
            },
            new Rectangle((1240 - 200) / 2 - 20, 160, 200, 220), "Achievements", Game1.regular, Color.Black, Color.White, 0);

            ClickMe.OnLeftClick += AddPoint;
            ClickMe.OnLeftClick += SpawnParticle;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < clickParticles.Count; i++)
            {
                clickParticles[i].Update();

                if (clickParticles[i].LifeSpan == 0)
                {
                    clickParticles.RemoveAt(i);
                    i--;
                }
            }

            ClickMe.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Game1.assets["square"], new Rectangle(0, 0, 1240, 720), Color.Black * .2f);
            sb.Draw(Game1.assets["square"], new Rectangle(ClickMe.Position.X - 150, ClickMe.Position.Y - 150, ClickMe.Position.Width + 300, ClickMe.Position.Height + 300), Color.Black * .2f);
            sb.Draw(Game1.assets["square"], new Rectangle(ClickMe.Position.X - 50, ClickMe.Position.Y - 50, ClickMe.Position.Width + 100, ClickMe.Position.Height + 100), Color.Black * .2f);

            ClickMe.Draw(sb);

            foreach (Particle particle in clickParticles)
            {
                particle.Draw(sb);
            }

            sb.Draw(Game1.assets["square"], new Rectangle(0, 500, 1240, 720), Color.Gray);

            DrawPoints(sb, new Vector2(620, 600) - Game1.ReadOut.MeasureString(TotalPoints.ToString()) / 2);
        }

        public static void DrawPoints(SpriteBatch sb, Vector2 pos)
        {
            sb.DrawString(Game1.ReadOut, TotalPoints.ToString(), pos, Color.White);
        }

        public void AddPoint()
        {
            TotalPoints += ClickTotal;
        }

        public static void SubtractPoints(int amount)
        {
            TotalPoints -= amount;
        }

        public static void AddPoints(int amount)
        {
            TotalPoints += amount;
        }

        public void SpawnParticle()
        {
            clickParticles.Add(new Particle(new Vector2(rng.Next(520, 721), rng.Next(160, 361)), .95f, string.Format("+" + ClickTotal.ToString())));
        }
    }
}
