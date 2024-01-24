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

        public bool HasCollapsed { get { return hasCollapsed; } }
        public Point Position { get { return position; } }
        public List<int> Options { get { return options; } }
        public Texture2D Asset { get { return asset; } }

        public Cell(List<int> options, Point position)
        {
            this.options = options;
            this.position = position;
        }

        public void RemoveOption(int option)
        {
            options.Remove(option);
        }
    }
}
