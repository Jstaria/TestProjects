using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveCollapse___After
{
    internal class Cell
    {
        private List<int> options;
        private bool hasCollapsed;
        private Point position;
        private Texture2D asset;

        public bool HasCollapsed { get { return hasCollapsed; } set { hasCollapsed = value; } }
        public Point Position { get { return position; } }
        public List<int> Options { get { return options; } set { options.Clear(); options.AddRange(value); } }
        public Texture2D Asset { get { return asset; } set { asset = value; } }

        public int[] ArrayPosition { get; set; }

        public Cell(List<int> options, Point position)
        {
            this.options = new List<int>();
            this.options.AddRange(options);
            this.position = position;
        }

        public void RemoveOption(int option)
        {
            options.Remove(option);
        }
    }
}
