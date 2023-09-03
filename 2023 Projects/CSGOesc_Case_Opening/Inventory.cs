﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    internal class Inventory
    {
        private int itemNumber;

        private Dictionary<String, Dictionary<string, Item>> inventory;
        private Dictionary<String, Dictionary<string, Button>> itemButtons;

        private List<Item> items;

        private Texture2D[] buttonAssets;

        public int NumItems { get; private set; }

        public Inventory(List<Item> items)
        {
            this.itemNumber = 0;
            this.items = items;
            this.inventory = new Dictionary<string, Dictionary<string, Item>>();
            this.itemButtons = new Dictionary<string, Dictionary<string, Button>>();

            this.itemButtons.Add("Rarity", new Dictionary<string, Button>());

            this.buttonAssets = new Texture2D[]
            {
                Game1.assets["button_active"],
                Game1.assets["button_inactive"]
            };

            for (int i = 0; i < items.Count; i++)
            {
                Rectangle rect = new Rectangle();
                if (i < 4)
                {
                    rect = new Rectangle(i * 200 + i * 60 + 130, 30, 200, 200);
                }
                else
                {
                    rect = new Rectangle((i - 4) * 200 + (i - 4) * 60 + 260, 260, 200, 200);
                }

                inventory.Add(items[i].Name, new Dictionary<string, Item>());
                itemButtons["Rarity"].Add(items[i].Name, new Button(buttonAssets, rect, items[i].Name, Game1.ReadOut, Color.Black, items[i].Color, .5f));
            }

            LoadItems();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < itemButtons["Rarity"].Count; i++)
            {
                itemButtons["Rarity"].ElementAt(i).Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < itemButtons["Rarity"].Count; i++)
            {
                itemButtons["Rarity"].ElementAt(i).Value.Draw(sb);
            }

            for (int i = 0; i < inventory.Count; i++)
            {
                Rectangle rect = itemButtons["Rarity"].ElementAt(i).Value.Position;

                sb.DrawString(Game1.ReadOut, inventory[items[i].Name].Count.ToString(), new Vector2((rect.X + rect.Width / 2) - Game1.ReadOut.MeasureString(inventory[items[i].Name].Count.ToString()).X / 2, (rect.Y + rect.Height / 2) + 20), Color.Black);
            }
        }

        public void LoadItems()
        {
            List<string> itemInfo = FileIO.ReadFrom("SavedInventory");

            String[] info = null;

            foreach (string line in itemInfo)
            {
                info = line.Split(',');
                AddItem(new Item(int.Parse(info[0]), new Color(int.Parse(info[1]), int.Parse(info[2]), int.Parse(info[3])), info[4], info[5]), false);
            }

            if (itemInfo.Count > 0)
            {
                info = itemInfo[itemInfo.Count - 1].Split(',');

                itemNumber = int.Parse(info[info.Length - 1]);
            }   
        }

        /// <summary>
        /// Adds items to a dictionary of dictionaries for each specific type of item with a unique key for each item
        /// If that key already exists (which is almost impossible), it'll just skip over it because it is infinitely rarely ever gonna happen
        /// </summary>
        public void AddItem(Item item, bool append)
        {
            itemNumber++;

            if (inventory[item.Name].ContainsKey(item.UniqueID)) { return; }

            inventory[item.Name].Add(item.UniqueID, item);

            item.ItemNumber = itemNumber;

            if (append)
            {
                List<string> itemInfo = new List<string>
                {
                    item.Weight + "," +
                    item.Color.R.ToString() + "," +
                    item.Color.G.ToString() + "," +
                    item.Color.B.ToString() + "," +
                    item.Name + "," +
                    item.UniqueID + "," +
                    itemNumber
                };

                FileIO.AppendTo("SavedInventory", itemInfo);
            }
        }

        /// <summary>
        /// Removes item with unique identifier
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            if (!inventory[item.Name].ContainsKey(item.UniqueID)) { return; }

            inventory[item.Name].Remove(item.UniqueID);

            // remove item at item.ItemNumber
        }
    }
}
