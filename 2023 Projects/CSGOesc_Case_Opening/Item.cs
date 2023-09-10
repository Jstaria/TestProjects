using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    internal class Item
    {
        private int weight;
        private Color color;
        private string name;

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

        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public bool HoveredOver
        {
            get { return hoveredOver; }
            set { hoveredOver = value; }
        }

        public int Min
        {
            get { return min; }
            set { min = value; }
        }

        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public Color Color
        {
            get { return color; }
        }

        public string Name
        {
            get { return name; }
        }

        public string UniqueID { get; private set; }

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
