using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickerSlots
{
    internal class Item
    {
        // Particular item spawning parameter
        private int weight;

        private Color color;
        private string name;

        // Because these are meant to be visual,
        // I need a way of each item keeping track of
        // whether or not it is in fact, hovered over
        private bool hoveredOver;
        private bool previouslyHovered;

        private float expand;

        public static SoundEffectInstance sound;

        public int ItemNumber { get; set; }

        public float Expand
        {
            get { return expand; }
            set { expand = value; }
        }

        // For random num gen
        // Bottom weight value
        private int min;
        // Max weight value
        private int max;

        public float Volume { set { sound.Volume = value; } }
        public int Weight { get { return weight; } set { weight = value; } }
        public bool HoveredOver { get { return hoveredOver; } set { hoveredOver = value; } }
        public int Min { get { return min; } set { min = value; } }
        public int Max { get { return max; } set { max = value; } }
        public Color Color { get { return color; } }
        public string Name { get { return name; } }

        // I was thinking of the UniqueID as a way to predtermine seeds for like a loot box or to grab it from a list
        // Ultimately I don't know how useful it actually is with less expensive alternatives
        public string UniqueID { get; private set; }

        /// <summary>
        /// Item to be given out during slots
        /// </summary>
        /// <param name="weight">Rarity Number</param>
        /// <param name="color"></param>
        /// <param name="name"></param>
        /// <param name="uniqueID">Unique Identifier since you can have multiple of the same items</param>
        /// <param name="volume"></param>
        public Item(int weight, Color color, string name, string uniqueID, float volume)
        {
            this.weight = weight;
            this.color = color;
            this.name = name;
            this.UniqueID = uniqueID;

            this.expand = 1;

            sound = Game1.hitMarker.CreateInstance();

            sound.Volume = volume;
        }

        public void Update(bool idle)
        {
            if (hoveredOver && expand < 40)
            {
                if (hoveredOver != previouslyHovered && !idle)
                {
                    sound.Play();
                }
                expand += 4;
            }

            else if (!hoveredOver)
            {
                if (expand > 0)
                {
                    expand *= .975f;

                    if (expand < .2)
                    {
                        expand = 0;
                    }
                }
            }

            previouslyHovered = hoveredOver;
        }
    }
}
