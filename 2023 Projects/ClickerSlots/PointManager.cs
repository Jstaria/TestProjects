using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace ClickerSlots
{
    internal class PointManager
    {
        private int ClicksPerScene;
        private double lastClickTime;
        private double time;
        private Random rng = new Random();
        private GameTime gameTime;

        public static int CurrentPoints { get; private set; }
        public static int TotalPoints { get; private set; }
        public static int ClickAmount { get; private set; }
        
        public static int TotalClicks { get; private set; }

        public List<Particle> clickParticles;

        private Dictionary<String, int> items;
        private Button ClickMe;
        private Button Achievements;
        private ParticleSystem ps;

        /// <summary>
        /// Manages points, clicks, and spins, as well as spawning the Click Gamemode
        /// </summary>
        public PointManager()
        {
            clickParticles = new List<Particle>();

            ///////////////////////////////////////
            // Info being used for achievements
            List<String> info = FileIO.ReadFrom("Clicker");

            CurrentPoints = int.Parse(info[0]);
            ClickAmount = int.Parse(info[1]);
            TotalClicks = int.Parse(info[2]);
            SlotUI.TotalSpins = int.Parse(info[3]);
            TotalPoints = int.Parse(info[4]);
            ////////////////////////////////////////

            items = new Dictionary<String, int>();
            ClickMe = new Button(new Texture2D[]
            {
                Game1.assets["button_active"],
                Game1.assets["button_inactive"]
            }, 
            new Rectangle((1240 - 200) / 2, 160, 200, 200), "Click Me", Game1.ReadOut, Color.Black, Color.White, 0);

            ClickMe.OnLeftClick += AddPoint;
            ClickMe.OnLeftClick += SpawnParticle;

            // Particle system that is interacted with number of clicks per time period
            ps = new ParticleSystem(0, Color.Yellow, Color.Red, .98f, Game1.assets["triangle"], new Rectangle(0, 530, 1240, 0), 1, true, 0);
        }

        public void Update(GameTime gameTime)
        {
            ps.Update();

            this.gameTime = gameTime;

            if (gameTime.TotalGameTime.TotalSeconds - lastClickTime > 10)
            {
                ClicksPerScene = 0;
            }

            /////////////////////////////////////////
            // Particle System Determinants
            ps.ParticleAmount =
                ClicksPerScene < 10 ? 0 :
                ClicksPerScene < 30 ? 100 :
                ClicksPerScene < 60 ? 250 :
                ClicksPerScene < 120 ? 400 :
                ClicksPerScene < 200 ? 1000 :
                2000;

            ps.Speed =
                ClicksPerScene < 30 ? 1 :
                ClicksPerScene < 60 ? 2 :
                ClicksPerScene < 120 ? 3 :
                ClicksPerScene < 200 ? 5 :
                7;
            /////////////////////////////////////////

            // Standard "Particle System" before Particle system class existed,
            // displays points gained form clicking the button
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
            ps.Draw(sb);

            sb.Draw(Game1.assets["square"], new Rectangle(0, 0, 1240, 720), Color.Black * .2f);
            sb.Draw(Game1.assets["square"], new Rectangle(ClickMe.Position.X - 150, ClickMe.Position.Y - 150, ClickMe.Position.Width + 300, ClickMe.Position.Height + 300), Color.Black * .2f);
            sb.Draw(Game1.assets["square"], new Rectangle(ClickMe.Position.X - 50, ClickMe.Position.Y - 50, ClickMe.Position.Width + 100, ClickMe.Position.Height + 100), Color.Black * .2f);

            ClickMe.Draw(sb);

            foreach (Particle particle in clickParticles)
            {
                particle.DrawString(sb);
            }

            sb.Draw(Game1.assets["square"], new Rectangle(0, 500, 1240, 720), Color.Gray);

            DrawPoints(sb, new Vector2(620, 600) - Game1.ReadOut.MeasureString(CurrentPoints.ToString()) / 2);
        }

        public static void DrawPoints(SpriteBatch sb, Vector2 pos)
        {
            sb.DrawString(Game1.ReadOut, CurrentPoints.ToString(), pos, Color.White);
        }

        /// <summary>
        /// Saves current achievement information for later launch
        /// </summary>
        public static void Save()
        {
            List<string> list = new List<string>
            {
                CurrentPoints.ToString(),
                ClickAmount.ToString(),
                TotalClicks.ToString(),
                SlotUI.TotalSpins.ToString(),
                TotalPoints.ToString()
            };

            FileIO.WriteTo("Clicker", list);
        }

        /// <summary>
        /// Adds single point to total points, and single click to total clicks
        /// </summary>
        public void AddPoint()
        {
            lastClickTime = gameTime.TotalGameTime.TotalSeconds;
            ClicksPerScene ++;
            CurrentPoints += ClickAmount;
            TotalPoints += ClickAmount;
            TotalClicks++;
        }

        /// <summary>
        /// Removes certain amount of points from current point amount only
        /// </summary>
        /// <param name="amount"></param>
        public static void SubtractPoints(int amount)
        {
            CurrentPoints -= amount;
        }

        /// <summary>
        /// Adds certain amount of points to both total and current
        /// </summary>
        /// <param name="amount"></param>
        public static void AddPoints(int amount)
        {
            CurrentPoints += amount;
            TotalPoints += amount;
        }

        /// <summary>
        /// Spawns particle randomly on button
        /// </summary>
        public void SpawnParticle()
        {
            clickParticles.Add(new Particle(new Vector2(rng.Next(520, 721), rng.Next(160, 361)), .95f, string.Format("+" + ClickAmount.ToString()), Game1.ReadOut));
        }

        /// <summary>
        /// Clears save data for point manager
        /// </summary>
        public static void Clear()
        {
            ClickAmount = 1;
            TotalClicks = 0;
            TotalPoints = 0;
            SlotUI.TotalSpins = 0;
            CurrentPoints = 0;

            Save();
        }
    }
}
