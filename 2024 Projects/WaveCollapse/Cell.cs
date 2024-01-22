using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WaveCollapse
{
    internal class Cell
    {
        private Texture2D asset;
        private Vector2 position;

        // Stores the tiles that are compatible with a certaindirection for this tile
        private Dictionary<Vector2, List<int>> compatibleDirections;

        public Vector2 Position { get { return position; } }

        public Cell(Texture2D asset, Vector2 position, Dictionary<Vector2, List<int>> compatibleDirections)
        {
            this.asset = asset;
            this.position = position;
            this.compatibleDirections = compatibleDirections;
        }

        public List<int> GetCompatibleNeighbors(Vector2 direction)
        {
            return compatibleDirections[direction];
        }
    }
}
