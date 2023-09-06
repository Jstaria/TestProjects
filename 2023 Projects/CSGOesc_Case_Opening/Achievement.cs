using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    internal class Achievement
    {
        private int neededAmount;

        public string Name { get; private set; }
        public string Description { get; private set; }
        public float Progress { get; private set; }
        public bool Completed { get; private set; }

        public Rectangle Position { get; set; }

        public Achievement(string name, string description, bool completed)
        {
            this.Name = name;
            this.Description = description;
            this.Completed = completed;
            this.neededAmount = 0;
        }

        public Achievement(string name, string description, int neededAmount, bool completed)
        {
            this.Name = name;
            this.Description = description;
            this.Completed = completed;
            this.neededAmount = neededAmount;
        }

        /// <summary>
        /// Achievement progress based on an amount
        /// </summary>
        /// <param name="amountCurrently"></param>
        public void Update(int amountCurrently)
        {
            Completed = Math.Clamp(Progress = amountCurrently / neededAmount, 0, 1) == 1;
        }

        /// <summary>
        /// Achievement completed
        /// </summary>
        public void Update()
        {
            Completed = true;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Game1.assets["square"], Position, Color.White);
            sb.DrawString(Game1.ReadOut, Name, new Vector2(Position.X, Position.Y), Color.Black);
        }
    }
}
