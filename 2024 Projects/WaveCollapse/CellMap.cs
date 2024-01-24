using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

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

        private int[] currentPos;

        private int scale;

        // Lookup variables for algorithm
        private List<int[]> activeArrayPositions;
        private List<Vector2> directions = new List<Vector2>();

        private Random random;

        private bool doneGenerating = false;
        private bool startGenerating = false;

        private Dictionary<string, Cell> cellDict;

        public Cell[,] CellArray { get { return cellArray; } }
        public Vector2 Position { get { return position; } }

        public CellMap(Vector2 position, int width, int height, int scale, Dictionary<string, Cell> cellDict)
        {
            this.cellArray = new Cell[width, height];
            this.position = position;
            this.width = width;
            this.height = height;
            this.scale = scale;
            this.activeArrayPositions = new List<int[]>();
            this.random = new Random();
            this.cellDict = cellDict;

            this.directions.Add(new Vector2(-1, 0));
            this.directions.Add(new Vector2(1, 0));
            this.directions.Add(new Vector2(0, -1));
            this.directions.Add(new Vector2(0, 1));

            this.activeArrayPositions.Add(new int[] { width / 2, height / 2 });
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void CreateBoard()
        {
            // Create random cell from cell dict and make sure it is a viable cell
            int num = random.Next(1, cellDict.Count);

            while (cellDict[num.ToString()].GetCompatibleNeighbors(new Vector2(0,1)).Count <= 0)
            {
                num = random.Next(cellDict.Count);
            }

            // Copy over first cell
            cellArray[0, 0] = CreateCell(cellDict[num.ToString()], position);

            // Loop through all vertices and create the board
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cell cell = cellArray[i,j];

                    if (cell != null) continue;

                    Dictionary<Vector2, List<string>> comDir = new Dictionary<Vector2, List<string>>();

                    comDir.Add(new Vector2(-1, 0), new List<string>());
                    comDir.Add(new Vector2(1, 0), new List<string>());
                    comDir.Add(new Vector2(0, -1), new List<string>());
                    comDir.Add(new Vector2(0, 1), new List<string>());

                    // Get all compatible directions from neighbors
                    for (int k = 0; k < directions.Count; k++)
                    {
                        int x = i + (int)directions[k].X;
                        int y = j + (int)directions[k].Y;

                        // Check if directions are in the array
                        if (IsInArray(x, y, cellArray) && cellArray[x,y] != null)
                        {
                            Cell temp = cellArray[x, y];

                            comDir[directions[k]] = temp.GetCompatibleNeighbors(-directions[k]);
                        }
                    }

                    List<string> masterList = new List<string>();

                    for (int k = 0; k < comDir.Count; k++)
                    {
                        masterList.AddRange(comDir.Values.ToArray()[k]);
                    }

                    for (int k = 0;k < comDir.Count; k++)
                    {
                        if (comDir.Values.ToArray()[k].Count <= 0) continue;

                        masterList = masterList.Intersect(comDir.Values.ToArray()[k]).ToList();
                    }

                    if (masterList.Count <= 0) 
                    { 


                        continue; 
                    }

                    cellArray[i, j] = CreateCell(cellDict[masterList[random.Next(masterList.Count)]], new Vector2(i * scale, j * scale) + position);
                }
            }
        }

        /// <summary>
        /// Will return if a position is in the 2D array
        /// </summary>
        /// <param name="arrayPos"></param>
        /// <param name="cellArray"></param>
        /// <returns></returns>
        private bool IsInArray(int[] arrayPos, Cell[,] cellArray)
        {
            return
                arrayPos[0] < cellArray.GetLength(0) && arrayPos[1] < cellArray.GetLength(1) &&
                arrayPos[0] >= 0 && arrayPos[1] >= 0;
        }

        private bool IsInArray(int x, int y, Cell[,] cellArray)
        {
            return
                x < cellArray.GetLength(0) && y < cellArray.GetLength(1) &&
                x >= 0 && y >= 0;
        }

        /// <summary>
        /// Copies info from cell to create a new cell
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private Cell CreateCell(Cell cell, Vector2 pos)
        {
            Dictionary<Vector2,List<string>> comDir = cell.CompatibleDirections;

            Cell temp = new Cell(cell.Asset, pos);
            temp.SetNeighbors(comDir.ElementAt(0).Value, comDir.ElementAt(1).Value, comDir.ElementAt(2).Value, comDir.ElementAt(3).Value);
            temp.HasCollapsed = true;

            return temp;
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
