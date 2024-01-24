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
            this.directions.Add(new Vector2(0, 1));
            this.directions.Add(new Vector2(0, -1));

            this.activeArrayPositions.Add(new int[] { width / 2, height / 2 });
        }

        public void Update(GameTime gameTime)
        {
            if (doneGenerating) return;

            if (activeArrayPositions[0] != new int[] {-1, -1})
            {

                #region firstSpace

                if (!startGenerating)
                {
                    currentPos = activeArrayPositions[random.Next(activeArrayPositions.Count)];

                    int num = random.Next(1, cellDict.Count + 1);

                    while (cellDict[num.ToString()].GetCompatibleNeighbors(directions[0]).Count <= 0)
                    {
                        num = random.Next(1, cellDict.Count + 1);
                    }

                    cellArray[currentPos[0], currentPos[1]] = cellDict[num.ToString()];
                    cellArray[currentPos[0], currentPos[1]].Position = new Vector2(position.X + currentPos[0] * scale, position.Y + currentPos[1] * scale);
                    cellArray[currentPos[0], currentPos[1]].HasCollapsed = true;

                    for (int i = 0; i < directions.Count; i++)
                    {
                        int[] temp = new int[] { currentPos[0] + (int)directions[i].X, currentPos[1] + (int)directions[i].Y };

                        if (IsNeighbor(temp, cellArray) && !activeArrayPositions.Contains(temp))
                        {
                            activeArrayPositions.Add(temp);
                        }
                    }

                    startGenerating = true;
                }

                #endregion

                else
                {
                    // remove current space from active queue because we already set it
                    activeArrayPositions.Remove(currentPos);

                    // find new position to set
                    currentPos = activeArrayPositions[0];

                    Cell currentCell = cellArray[currentPos[0], currentPos[1]];

                    List<List<string>> directionOfTiles = new List<List<string>>()
                    {
                        new List<string>(), // left
                        new List<string>(), // right
                        new List<string>(), // up
                        new List<string>()  // down
                    };


                    // Check each directions from current to find next active positions to then collapse
                    for (int i = 0; i < directions.Count; i++)
                    {
                        int[] temp = new int[] { currentPos[0] + (int)directions[i].X, currentPos[1] + (int)directions[i].Y };

                        // If its a neighbor in the array, and it is not populated as well as not already being in the active list
                        if (IsNeighbor(temp, cellArray) && cellArray[temp[0], temp[1]] == null && !activeArrayPositions.Contains(temp))
                        {
                            activeArrayPositions.Add(temp);
                        }

                        // Must be a neighbor, is not null, and has collapsed. We then get the opposite direction of available tiles for this tile
                        // And add that range
                        else if (IsNeighbor(temp, cellArray) && cellArray[temp[0], temp[1]] != null && cellArray[temp[0], temp[1]].HasCollapsed)
                        {
                            directionOfTiles[i].AddRange(cellArray[temp[0], temp[1]].GetCompatibleNeighbors(-directions[i]));
                        }
                    }

                    // Create a master list that will store all the possible values
                    List<string> masterList = new List<string>();

                    for (int i = 0; i < directionOfTiles.Count; i++)
                    {
                        for (int j = 0; j < directionOfTiles[i].Count; j++)
                        {
                            // Check to make sure we don't add duplicates
                            if (masterList.Contains(directionOfTiles[i][j])) continue;

                            masterList.Add(directionOfTiles[i][j]);
                        }
                    }

                    // Finally, we find out which of the available direction lists we have the ability to place
                    for (int i = 0; i < directionOfTiles.Count; i++)
                    {
                        if (directionOfTiles[i].Count <= 0) continue;

                        masterList = masterList.Intersect(directionOfTiles[i]).ToList();
                    }

                    if (masterList.Count > 0)
                    {
                        string key = masterList[random.Next(masterList.Count)].ToString();

                        cellArray[currentPos[0], currentPos[1]] = cellDict[key];
                        cellArray[currentPos[0], currentPos[1]].Position = new Vector2(position.X + currentPos[0] * scale, position.Y + currentPos[1] * scale);
                        cellArray[currentPos[0], currentPos[1]].HasCollapsed = true;
                    }
                }
            }

            else
            {
                doneGenerating = true;
            }
            Thread.Sleep(60);
        }

        private bool IsNeighbor(int[] arrayPos, Cell[,] cellArray)
        {
            return
                arrayPos[0] < cellArray.GetLength(0) && arrayPos[1] < cellArray.GetLength(1) &&
                arrayPos[0] >= 0 && arrayPos[1] >= 0;
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
