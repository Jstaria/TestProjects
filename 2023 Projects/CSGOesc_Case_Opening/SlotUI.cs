using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace CSGOesc_Case_Opening
{
    internal class SlotUI
    {
        private Dictionary<string, Texture2D> assets;
        private Vector2 position;

        private float timeOfLastSpin;
        private Item prevWonItem;
        private bool canSpin;
        private bool idle;

        private List<Item> activeItems;
        private List<Vector2> activeItemsPos;

        private int totalWeight;
        private float num;
        private int spinCount;
        private int prevSpinCount;

        public static int spinCost = 15;

        private bool menuActive;
        private int idleSpeed;

        private int itemBoxWidth;
        private int itemBoxHeight;

        private StreamReader itemReader;
        private Random rng;

        public bool HasSpun { get; private set; }

        private float rand;
        public bool Idle
        {
            get { return idle; }
            set { idle = value; }
        }

        public List<Item> Items { get; private set; }

        public Particle spinParticle { get; set; }

        public SlotUI (Dictionary<string, Texture2D> assets, Vector2 position, bool menuActive, float idleSpeed)
        {
            this.menuActive = menuActive;
            this.assets = assets;
            this.position = position;
            this.prevWonItem = null;
            this.canSpin = true;

            this.Items = new List<Item>();
            this.activeItems = new List<Item>();
            this.activeItemsPos = new List<Vector2>();
            this.idle = true;

            this.num = idleSpeed;
            this.spinCount = 0;
            this.prevSpinCount = 0;
            this.totalWeight = 0;

            GetItems();

            this.spinParticle = null;
            this.itemBoxWidth = 100;
            this.itemBoxHeight = 250;

            this.rng = new Random();
        }

        public Item[] Update(GameTime gameTime)
        {
            if (HasSpun)
            {
                timeOfLastSpin = (float)gameTime.TotalGameTime.TotalSeconds;
            }

            if (spinParticle != null)
            {
                spinParticle.Update();
            }

            Item[] wonItems = new Item[2];

            if (spinCount > 0)
            {
                if (idle)
                {
                    num = idleSpeed;
                    canSpin = true;
                }
                else
                {
                    float timeSinceLastSpin = (float)gameTime.TotalGameTime.TotalSeconds - timeOfLastSpin;
                    //System.Diagnostics.Debug.WriteLine(timeSinceLastSpin);

                    // if (num > 0)
                    // {
                    //     num = (float)Math.Pow(2, 30 * ((rng.Next(10, 26) / 10) * timeSinceLastSpin + 3) / rng.Next(14,17)) - .05f;
                    // }

                    // condition ? value_if_true : value_if_false

                    // condition ? value_if_true : (condition ? value_if_true : value_if_false)
                    num *= (float)(num > 5 ? 0.992 : (num > 0.005 ? rand : 0));

                    if (num == 0)
                    {
                        idle = true;
                    }
                }
            }
            
            int numItems = 16;

            for (int i = 0; i < numItems; i++)
            { 
                int randNum = rng.Next(totalWeight);

                string randString = null;

                for (int j = 0; j < 20; j++)
                {
                    randString += (char)rng.Next(65, 123);
                }

                if (activeItems.Count < numItems)
                {
                    activeItems.Add(GrabItem(randNum, randString));
                    activeItemsPos.Add(new Vector2(-2 * itemBoxWidth + i * itemBoxWidth, position.Y));
                }

                else if (activeItemsPos[i].X < -itemBoxWidth)
                {
                    activeItems[i] = GrabItem(randNum, randString);
                    activeItemsPos[i] = new Vector2(activeItemsPos[activeItems.Count - 1].X + i * itemBoxWidth, position.Y);
                }

                activeItems[i].Update(idle);               
                activeItemsPos[i] -= new Vector2(num, 0);

                if (!menuActive)
                {
                    if (new Rectangle((int)activeItemsPos[i].X, (int)activeItemsPos[i].Y, itemBoxWidth, itemBoxHeight).Intersects(
                    new Rectangle(1240 / 2 - 1, 0, 2, 720)))
                    {
                        activeItems[i].HoveredOver = true;

                        wonItems[1] = activeItems[i];

                        if (num == 0)
                        {
                            wonItems[0] = activeItems[i];
                        }
                    }

                    else
                    {
                        activeItems[i].HoveredOver = false;
                    }
                }
            }

            HasSpun = false;

            return wonItems;
        }

        /// <summary>
        /// Draws the deck of items currently in rotation
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < activeItems.Count; i++)
            {
                Rectangle visualBox = new Rectangle((int)(activeItemsPos[i].X), (int)(activeItemsPos[i].Y - activeItems[i].Expand), (int)(itemBoxWidth), (int)(itemBoxHeight + activeItems[i].Expand * 2));
                sb.Draw(assets["square"], visualBox, activeItems[i].Color);
            }

            int count = 0;

            for (int i = 0; i < activeItems.Count; i++)
            {
                if (activeItems[i].HoveredOver)
                {
                    count = i;

                    Rectangle visualBox = new Rectangle((int)(activeItemsPos[i].X - activeItems[i].Expand / 2), (int)(activeItemsPos[i].Y - activeItems[i].Expand), (int)(itemBoxWidth + activeItems[i].Expand * 1.5f), (int)(itemBoxHeight + activeItems[i].Expand * 2));
                    sb.Draw(assets["square"], visualBox, activeItems[i].Color);
                }
            }

            if (!menuActive)
            {
                sb.Draw(assets["square"], new Rectangle(620, 0, 2, 720), Color.Black);
            }
        }

        public void Spin()
        {
            prevSpinCount = spinCount;

            MouseState currentMouseState = Mouse.GetState();
            if (new Rectangle(0, 0, 1240, 720).Intersects(new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1)) &&
                currentMouseState.LeftButton == ButtonState.Pressed && canSpin && PointManager.TotalPoints >= spinCost && idle)
            {
                num = rng.Next(70,100);

                rand = rng.Next(974, 986) * .001f;
                System.Diagnostics.Debug.WriteLine(rand);

                spinCount++;

                idle = false;

                canSpin = false;

                HasSpun = true;

                spinParticle = new Particle(Mouse.GetState().Position.ToVector2(), .975f, string.Format("-" + spinCost.ToString()));

                PointManager.SubtractPoints(spinCost);
            }
        }

        /// <summary>
        /// Picks from the assorted deck of rarites via a random number,
        /// It copies over the data from one of the templates to insure it is a unique card,
        /// as well as giving each card their own unique ID
        /// </summary>
        /// <param name="randNum"></param>
        /// <returns></returns>
        private Item GrabItem(int randNum, string randString)
        {
            Item newItem = null;

            foreach (Item item in Items)
            {
                if (item.Min < randNum && randNum <= item.Max)
                {
                    newItem = new Item(item.Weight, item.Color, item.Name, randString);
                }
            }

            return newItem;
        }

        /// <summary>
        /// Retrieves list of items from text file
        /// </summary>
        private void GetItems()
        {
            string stream = "../../../Content/" + "items" + ".txt";
            StreamReader itemReader = new StreamReader(stream);

            string line = null;

            while ((line = itemReader.ReadLine()) != null)
            {
                string[] itemDetails = line.Split(',');

                Items.Add(
                    new Item(
                        int.Parse(itemDetails[4]),
                        new Color(
                            int.Parse(itemDetails[1]),
                            int.Parse(itemDetails[2]),
                            int.Parse(itemDetails[3])),
                        itemDetails[0], 
                        "PLACEHOLDER"));

                Items[Items.Count - 1].Min = totalWeight;
                Items[Items.Count - 1].Max = totalWeight + int.Parse(itemDetails[4]);

                totalWeight += int.Parse(itemDetails[4]);

            }

            if (itemReader != null)
            {
                itemReader.Close();
            }
        }
    }
}
