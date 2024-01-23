using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaveCollapse
{
    internal class CellMap
    {
        int count = 0;

        // Cell Map values
        private Cell[,] cellArray;
        private Vector2 position;

        private int width;
        private int height;

        private int scale;

        // Lookup variables for algorithm
        private List<int[]> previousArrayPositions;

        private Random random;

        private bool doneGenerating = false;

        private Dictionary<string, Cell> cellDict;

        public Cell[,] CellArray { get { return cellArray; } }
        public Vector2 Position { get { return position; } }

        public CellMap(Vector2 position, int width, int height, int scale, Dictionary<string,Cell> cellDict)
        {
            this.cellArray = new Cell[width, height];
            this.position = position;
            this.width = width;
            this.height = height;
            this.scale = scale;
            this.previousArrayPositions = new List<int[]>();
            this.random = new Random();
            this.cellDict = cellDict;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cell cell in cellArray)
            {
                if (cell == null) continue;
                spriteBatch.Draw(cell.Asset, new Rectangle((int)cell.Position.X, (int)cell.Position.Y, scale, scale), Color.White);
            }
        }
    }
}
