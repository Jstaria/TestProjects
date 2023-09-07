using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CSGOesc_Case_Opening
{
    internal class Achievement
    {
        public string Type { get; private set; }
        public int NeededAmount { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public float Progress { get; private set; }
        public bool Completed { get; private set; }

        public Rectangle Position { get; set; }

        public Achievement(string name, string description, bool completed, string type)
        {
            this.Type = type;
            this.Name = name;
            this.Description = description;
            this.Completed = completed;
            this.NeededAmount = 0;
        }

        public Achievement(string name, string description, int neededAmount, bool completed, string type)
        {
            this.Type = type;
            this.Name = name;
            this.Description = description;
            this.Completed = completed;
            this.Progress = completed ? 1 : 0;
            this.NeededAmount = neededAmount;
        }

        /// <summary>
        /// Achievement progress based on an amount
        /// </summary>
        /// <param name="amountCurrently"></param>
        public void Update(int amountCurrently)
        {
            float percent = (float)Math.Round((double)amountCurrently / NeededAmount, 5);
            Progress = Math.Clamp(percent, 0, 1);
            Completed = (Progress == 1);
        }

        /// <summary>
        /// Achievement completed
        /// </summary>
        public void Update()
        {
            Completed = true;
            Progress = 1;
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 textLength = Game1.ReadOut.MeasureString(Name);
            Vector2 textPos = new Vector2(
                (Position.X + Position.Width / 2) - textLength.X / 2,
                (Position.Y + Position.Height / 2) - textLength.Y / 2
            );

            sb.Draw(Game1.assets["square"], Position, Color.Gray);
            sb.Draw(Game1.assets["square"], Position, Color.Red * .1f);

            Color lerpColor = Progress < .5f ? Color.Lerp(Color.Red, Color.Yellow, Progress * 2) : Color.Lerp(Color.Yellow, Color.Green, (Progress - .5f) * 2);

            sb.Draw(Game1.assets["square"], new Rectangle(Position.X, Position.Y, (int)(Position.Width * Progress), Position.Height), Position, lerpColor);

            sb.Draw(Game1.assets["square"], new Rectangle(Position.X + 40, Position.Y + 15, Position.Width - 80, Position.Height - 30), Color.White);

            sb.DrawString(Game1.ReadOut, Name, textPos + new Vector2(0, -25), Color.Black);

            textLength = Game1.regular.MeasureString(Description);
            textPos = new Vector2(
                (Position.X + Position.Width / 2) - textLength.X / 2,
                (Position.Y + Position.Height / 2) - textLength.Y / 2
            );

            sb.DrawString(Game1.regular, Description, textPos + new Vector2(0, 20), Color.Black);
        }
    }
}
