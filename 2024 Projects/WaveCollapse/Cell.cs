using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveCollapse
{
    internal class Cell
    {
        private Texture2D asset;
        private Vector2 position;

        // Stores the tiles that are compatible with a certaindirection for this tile
        private Dictionary<Vector2, List<string>> compatibleDirections;

        public Texture2D Asset { get { return asset; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public bool HasCollapsed { get; set; }

        public Cell(Texture2D asset, Vector2 position)
        {
            this.asset = asset;
            this.position = position;
            this.compatibleDirections = new Dictionary<Vector2, List<string>>();

            this.compatibleDirections.Add(new Vector2(-1, 0), new List<string>());
            this.compatibleDirections.Add(new Vector2(1, 0), new List<string>());
            this.compatibleDirections.Add(new Vector2(0, 1), new List<string>());
            this.compatibleDirections.Add(new Vector2(0, -1), new List<string>());
        }

        public List<string> GetCompatibleNeighbors(Vector2 direction)
        {
            return compatibleDirections[direction];
        }

        public void SetNeighbors(List<string> left, List<string> right, List<string> up, List<string> down)
        {
            compatibleDirections[new Vector2(-1, 0)] = left;
            compatibleDirections[new Vector2(1, 0)] = right;
            compatibleDirections[new Vector2(0, -1)] = up;
            compatibleDirections[new Vector2(0, 1)] = down;
        }
    }
}
