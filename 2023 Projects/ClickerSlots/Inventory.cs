﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClickerSlots
{
    internal class Inventory
    {
        private int itemNumber;

        private Dictionary<String, Dictionary<string, Item>> inventory;
        private Dictionary<String, Dictionary<string, Button>> itemButtons;

        private List<Item> itemsInInv;
        private List<Item> items;

        private Texture2D[] buttonAssets;

        public int NumItems { get; private set; }
        public int InventoryValue { get; private set; }

        /// <summary>
        /// Manages all collected items
        /// </summary>
        public Inventory(List<Item> items)
        {
            this.itemNumber = 0;
            this.items = items;
            this.itemsInInv = new List<Item>();
            this.inventory = new Dictionary<string, Dictionary<string, Item>>();
            this.itemButtons = new Dictionary<string, Dictionary<string, Button>>();

            this.itemButtons.Add("Rarity", new Dictionary<string, Button>());

            this.buttonAssets = new Texture2D[]
            {
                Game1.assets["button_active"],
                Game1.assets["button_inactive"]
            };

            SetupInv(true);

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

            Vector2 pos = new Vector2(620, 600) - Game1.ReadOut.MeasureString(PointManager.CurrentPoints.ToString()) / 2;

            sb.DrawString(Game1.ReadOut, PointManager.CurrentPoints.ToString(), pos, Color.White);
        }

        /// <summary>
        /// Sells all current items
        /// </summary>
        public void SellAll()
        {
            int points = 0;

            for (int i = 0; i < inventory.Count; i++)
            {
                for (int j = 0; j < inventory.ElementAt(i).Value.Count; j++)
                {
                    string name = inventory.ElementAt(i).Value.ElementAt(j).Value.Name;

                    RemoveItem(inventory.ElementAt(i).Value[inventory.ElementAt(i).Value.Values.ToList()[j].UniqueID]);

                    // I totally could have made it so each item has a currency amount when it spawns
                    points += (
                    name == "Common" ? 5 :
                    name == "Uncommon" ? 10 :
                    name == "Rare" ? 20 :
                    name == "Epic" ? 40 :
                    name == "Mystic" ? 75 :
                    name == "Legendary" ? 200 :
                    name == "Royal" ? 2000 : 0);

                    j--;
                }
            }

            InventoryValue = 0;
            PointManager.AddPoints(points);
        }

        /// <summary>
        /// Sells recently won item
        /// </summary>
        public void SellWonItem()
        {
            string name = SlotMachine.WonItem.Name;

            RemoveItem(inventory[name][inventory[name].Values.ToList()[0].UniqueID]);

            int points =
            name == "Common" ? 5 :
            name == "Uncommon" ? 10 :
            name == "Rare" ? 20 :
            name == "Epic" ? 40 :
            name == "Mystic" ? 75 :
            name == "Legendary" ? 200 :
            name == "Royal" ? 2000 : 0;

            PointManager.AddPoints(points);

            InventoryValue -= points;
        }

        /// <summary>
        /// Sells one item
        /// </summary>
        /// <param name="name">Basic name of item</param>
        public void SellOne(string name)
        {
            if (inventory[name].Count > 0)
            {
                RemoveItem(inventory[name][inventory[name].Values.ToList()[0].UniqueID]);

                int points = 
                name == "Common" ? 5 :
                name == "Uncommon" ? 10 :
                name == "Rare" ? 20 :
                name == "Epic" ? 40 :
                name == "Mystic" ? 75 :
                name == "Legendary" ? 200 :
                name == "Royal" ? 2000 : 0;

                PointManager.AddPoints(points);

                InventoryValue -= points;
            }

            // condition ? value_if_true : value_if_false
        }

        /// <summary>
        /// All this does right now is removes an item from the inventory, is meant to be a lootbox system, thinkn cookie clicker but more gambling
        /// </summary>
        /// <param name="name"></param>
        public void OpenOne(string name)
        {
            if (inventory[name].Count > 0)
            {
                RemoveItem(inventory[name][inventory[name].Values.ToList()[0].UniqueID]);
            }
        }

        /// <summary>
        /// Sets up item dictionaries
        /// </summary>
        /// <param name="setup">Determines if the inventory buttons need to be setup again</param>
        public void SetupInv(bool setup)
        {
            for (int i = 0; i < items.Count; i++)
            {
                inventory.Add(items[i].Name, new Dictionary<string, Item>());

                if (setup)
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

                    itemButtons["Rarity"].Add(items[i].Name, new Button(buttonAssets, rect, items[i].Name, Game1.ReadOut, Color.Black, items[i].Color, .1f));
                    itemButtons["Rarity"][items[i].Name].OnRightClickString += SellOne;
                    //itemButtons["Rarity"][items[i].Name].OnLeftClickString += OpenOne;
                }
            }
        }

        /// <summary>
        /// Loads items from save file
        /// </summary>
        private void LoadItems()
        {
            List<string> itemInfo = FileIO.ReadFrom("SavedInventory");

            String[] info = null;

            foreach (string line in itemInfo)
            {
                info = line.Split(',');
                AddItem(new Item(int.Parse(info[0]), new Color(int.Parse(info[1]), int.Parse(info[2]), int.Parse(info[3])), info[4], info[5], Item.sound.Volume), false);

                string name = info[0];

                InventoryValue +=
                name == "Common" ? 5 :
                name == "Uncommon" ? 10 :
                name == "Rare" ? 20 :
                name == "Epic" ? 40 :
                name == "Mystic" ? 75 :
                name == "Legendary" ? 200 :
                name == "Royal" ? 2000 : 0;
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
            itemsInInv.Add(item);

            string name = item.Name;

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

            InventoryValue +=
                name == "Common" ? 5 :
                name == "Uncommon" ? 10 :
                name == "Rare" ? 20 :
                name == "Epic" ? 40 :
                name == "Mystic" ? 75 :
                name == "Legendary" ? 200 :
                name == "Royal" ? 2000 : 0;
        }

        /// <summary>
        /// Removes item with unique identifier
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            if (!inventory[item.Name].ContainsKey(item.UniqueID)) { return; }

            inventory[item.Name].Remove(item.UniqueID);
            itemsInInv.Remove(item);

            List<string> itemInfo = FileIO.ReadFrom("SavedInventory");

            itemInfo.RemoveAt(item.ItemNumber-1);

            itemNumber = 0;

            List<string> newItemInfo = new List<string>();

            foreach (String line in itemInfo)
            {
                itemNumber++;

                String[] split = line.Split(',');

                split[split.Length - 1] = itemNumber.ToString();
                itemsInInv[itemNumber-1].ItemNumber = itemNumber; 

                newItemInfo.Add(
                    split[0] + "," +
                    split[1] + "," +
                    split[2] + "," +
                    split[3] + "," +
                    split[4] + "," +
                    split[5] + "," +
                    split[6]);
            }

            FileIO.WriteTo("SavedInventory", newItemInfo);

            // remove item at item.ItemNumber
        }

        /// <summary>
        /// Clears save file of all inventory, and clears internal lists of all items
        /// </summary>
        public void Clear()
        {
            FileIO.WriteTo("SavedInventory", new List<string>());

            InventoryValue = 0;
            itemNumber = 0;
            inventory.Clear();
            itemsInInv.Clear();

            // Only sets up the lists, basically clearing them
            SetupInv(false);
            LoadItems();
        }
    }
}
