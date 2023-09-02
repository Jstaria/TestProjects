using Microsoft.Xna.Framework;
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
        private Dictionary<String, Dictionary<string, Item>> inventory; 

        public int NumItems { get; private set; }

        public Inventory(List<Item> items)
        {
            this.inventory = new Dictionary<string, Dictionary<string, Item>>();

            foreach (Item item in items)
            {
                inventory.Add(item.Name, new Dictionary<string, Item>());
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch sb)
        {

        }

        /// <summary>
        /// Adds items to a dictionary of dictionaries for each specific type of item with a unique key for each item
        /// If that key already exists (which is almost impossible), it'll just skip over it because it is infinitely rarely ever gonna happen
        /// </summary>
        public void AddItem(Item item)
        {
            if (inventory[item.Name].ContainsKey(item.UniqueID)) { return; }

            inventory[item.Name].Add(item.UniqueID, item);
        }

        /// <summary>
        /// Removes item with unique identifier
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            if (!inventory[item.Name].ContainsKey(item.UniqueID)) { return; }

            inventory[item.Name].Remove(item.UniqueID);
        }
    }
}
