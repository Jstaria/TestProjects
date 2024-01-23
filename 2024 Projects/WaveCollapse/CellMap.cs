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

        public Cell[,] CellArray { get { return cellArray; } }
        public Vector2 Position { get { return position; } }

        public CellMap() : this(new Vector2(0,-200), 10, 10, 75) { }

        public CellMap(Vector2 position, int width, int height, int scale)
        {
            this.cellArray = new Cell[width, height];
            this.position = position;
            this.width = width;
            this.height = height;
            this.scale = scale;
            this.previousArrayPositions = new List<int[]>();
            this.random = new Random();
        }

        public void Update(GameTime gameTime)
        {
            if (!doneGenerating)
            {
                int[] randomPos = new int[2];
                if (previousArrayPositions.Count > 1)
                {
                    randomPos = previousArrayPositions[random.Next(previousArrayPositions.Count)];
                }
                else { randomPos = new int[] { 5, 5 }; }

                List<string> data = FileIO.ReadFrom("tile");
                string[] line = data[data.Count - 1].Split(",");

                List<string> left = new List<string>();
                List<string> right = new List<string>();
                List<string> up = new List<string>();
                List<string> down = new List<string>();

                List<string> current = new List<string>();

                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case "Left":
                            current = left;
                            break;
                        case "Right":
                            current = right;
                            break;
                        case "Up":
                            current = up;
                            break;
                        case "Down":
                            current = down;
                            break;

                        case "S":

                            cellArray[randomPos[0], randomPos[1]] = new Cell(Game1.tiles[line[i+1]], new Vector2(randomPos[0] * scale + position.X, randomPos[1] * scale + position.Y));
                            i++;
                            break;

                        default:

                            current.Add(line[i]);

                            break;
                    }
                }

                cellArray[5, 5].SetNeighbors(left, right, up, down);
            }

            if (count % 60 == 0)
            {
                Cell current = cellArray[5, 5];

                List<string> strin = current.GetCompatibleNeighbors(new Vector2(1, 0));
                cellArray[6, 5] = new Cell(Game1.tiles[strin[random.Next(strin.Count)]], new Vector2(6 * scale + position.X, 5 * scale + position.Y));

                strin = current.GetCompatibleNeighbors(new Vector2(-1, 0));
                cellArray[4, 5] = new Cell(Game1.tiles[strin[random.Next(strin.Count)]], new Vector2(4 * scale + position.X, 5 * scale + position.Y));

                strin = current.GetCompatibleNeighbors(new Vector2(0, 1));
                cellArray[5, 4] = new Cell(Game1.tiles[strin[random.Next(strin.Count)]], new Vector2(5 * scale + position.X, 4 * scale + position.Y));

                strin = current.GetCompatibleNeighbors(new Vector2(0, -1));
                cellArray[5, 6] = new Cell(Game1.tiles[strin[random.Next(strin.Count)]], new Vector2(5 * scale + position.X, 6 * scale + position.Y));
            }

            count++;
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
