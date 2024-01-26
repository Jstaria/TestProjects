using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaveCollapse___After
{
    internal class CellGrid
    {
        private Cell[,] cellGrid;
        private Point position;
        private Dictionary<int, Texture2D> assets;

        private int width;
        private int height;
        private int scale;

        private Random random;

        private List<int> options = new List<int>();
        private List<List<int>>[] allCellOptions;
        private List<Point> directions;

        private Cell currentCell;
        private int numCollapsed = 0;

        private List<Cell> nextCells;

        private PriorityQueue<Cell, int> priorityCells;

        private float timeAtStart;
        private float timeSinceStart;

        public CellGrid(int width, int height, Dictionary<int, Texture2D> assets, Point position, int scale)
        {
            this.cellGrid = new Cell[width, height];
            this.assets = assets;
            this.position = position;

            this.width = width;
            this.height = height;
            this.scale = scale;

            this.random = new Random();

            this.directions = new List<Point>();
            this.directions.Add(new Point(-1, 0));
            this.directions.Add(new Point(0, -1));
            this.directions.Add(new Point(1, 0));
            this.directions.Add(new Point(0, 1));

            this.currentCell = cellGrid[0, 0];
            this.nextCells = new List<Cell>();
            this.priorityCells = new PriorityQueue<Cell, int>();
        }

        public void Collapse1By1Queued(GameTime gt)
        {
            // If we are done, we start over
            if (priorityCells.Count <= 0)
            {
                Thread.Sleep(2000);
                CreateGrid();
                timeAtStart = (float)Math.Round(gt.TotalGameTime.TotalSeconds, 2);
                return;
            }

            // Dequeue first cell in the priority queue
            currentCell = priorityCells.Dequeue();

            List<int> optionsList = new List<int>();

            // Restart if we are done
            if (currentCell.Options.Count == 0)
            {
                CreateGrid();
                timeAtStart = (float)Math.Round(gt.TotalGameTime.TotalSeconds, 2);
                return;
            }

            // Set the option for the currentCell
            int randomOption = currentCell.Options[random.Next(currentCell.Options.Count)];
            optionsList.Add(randomOption);
            currentCell.Options = optionsList;
            currentCell.HasCollapsed = true;
            currentCell.Asset = assets[optionsList[0]];
            //Debug.WriteLine(currentCell.ArrayPosition[0] + "," + currentCell.ArrayPosition[1]);

            numCollapsed++;

            for (int k = 0; k < directions.Count; k++)
            {
                int tempX = currentCell.ArrayPosition[0] + directions[k].X;
                int tempY = currentCell.ArrayPosition[1] + directions[k].Y;

                //Debug.WriteLine("temp: " + tempX + "," + tempY);

                if (IsInArray(tempX, tempY, cellGrid) && !cellGrid[tempX, tempY].HasCollapsed)
                {
                    int cellNum = currentCell.Options[0] - 1;
                    int dirNum = (k) % directions.Count;
                    cellGrid[tempX, tempY].Options = cellGrid[tempX, tempY].Options.Intersect(allCellOptions[cellNum][dirNum]).ToList();

                    // We then add to our priority queue where it manages cell priority by its ocount option for us
                    priorityCells.Enqueue(cellGrid[tempX, tempY], cellGrid[tempX, tempY].Options.Count);
                }
            }

            timeSinceStart = (float)Math.Round(gt.TotalGameTime.TotalSeconds - timeAtStart, 2);
        }

        public void Collapse1By1Listed(GameTime gt)
        {
            // If we are done, we start over
            if (nextCells.Count == 0)
            {
                Thread.Sleep(2000);
                CreateGrid();
                timeAtStart = (float)Math.Round(gt.TotalGameTime.TotalSeconds, 2);
                return;
            }

            // Grab first cell from our sorted list
            currentCell = nextCells[0];

            List<int> optionsList = new List<int>();

            // Restart if we are done
            if (currentCell.Options.Count == 0)
            {
                CreateGrid();
                timeAtStart = (float)Math.Round(gt.TotalGameTime.TotalSeconds, 2);
                return;
            }

            // Set the option for the currentCell
            int randomOption = currentCell.Options[random.Next(currentCell.Options.Count)];
            optionsList.Add(randomOption);
            currentCell.Options = optionsList;
            currentCell.HasCollapsed = true;
            nextCells.Remove(currentCell);
            currentCell.Asset = assets[optionsList[0]];
            //Debug.WriteLine(currentCell.ArrayPosition[0] + "," + currentCell.ArrayPosition[1]);

            numCollapsed++;

            // Find cells next to current and set their options to the adjacent tile options fo current cell
            for (int k = 0; k < directions.Count; k++)
            {
                int tempX = currentCell.ArrayPosition[0] + directions[k].X;
                int tempY = currentCell.ArrayPosition[1] + directions[k].Y;

                //Debug.WriteLine("temp: " + tempX + "," + tempY);

                if (IsInArray(tempX, tempY, cellGrid) && !cellGrid[tempX, tempY].HasCollapsed)
                {
                    // Look up the adjacent tileset options and set them for the adjacent cell
                    int cellNum = currentCell.Options[0] - 1;
                    int dirNum = (k) % directions.Count;
                    cellGrid[tempX, tempY].Options = cellGrid[tempX, tempY].Options.Intersect(allCellOptions[cellNum][dirNum]).ToList();

                    // then we add to our sorted list
                    InsertCell(cellGrid[tempX, tempY], nextCells);
                }
            }

            timeSinceStart = (float)Math.Round(gt.TotalGameTime.TotalSeconds - timeAtStart, 2);
        }

        /// <summary>
        /// Will update a tile every frame with no data structure to sort tile options
        /// </summary>
        /// <param name="gt"></param>
        public void Collapse1By1(GameTime gt)
        {
            // If we are done, we start over
            if (numCollapsed >= cellGrid.Length)
            {
                Thread.Sleep(2000);
                CreateGrid();
                timeAtStart = (float)Math.Round(gt.TotalGameTime.TotalSeconds,2);
                return;
            }

            // Loop through each cell to find a non collapsed cell
            foreach (Cell cell in cellGrid)
            {
                if (cell.HasCollapsed) continue;

                currentCell = cell;
            }

            List<int> optionsList = new List<int>();

            // Loop through every position to find a cell with less options than our current one
            for (int i = 0; i < cellGrid.GetLength(0); i++)
            {
                for (int j = 0; j < cellGrid.GetLength(1); j++)
                {
                    Cell cell = cellGrid[i, j];

                    if (cell.HasCollapsed) continue;

                    if (cell.Options.Count < currentCell.Options.Count)
                    {
                        currentCell = cell;
                    }
                }
            }

            // If the current cell has 0 options, we start over, because backtracking doesnt exist yet
            if (currentCell.Options.Count == 0)
            {
                CreateGrid();
                timeAtStart = (float)Math.Round(gt.TotalGameTime.TotalSeconds, 2);
                return;
            }

            // Set the option for the currentCell
            int randomOption = currentCell.Options[random.Next(currentCell.Options.Count)];
            optionsList.Add(randomOption);
            currentCell.Options = optionsList;
            currentCell.HasCollapsed = true;
            currentCell.Asset = assets[optionsList[0]];
            //Debug.WriteLine(currentCell.ArrayPosition[0] + "," + currentCell.ArrayPosition[1]);

            numCollapsed++;

            // Find cells next to current and set their options to the adjacent tile options fo current cell
            for (int k = 0; k < directions.Count; k++)
            {
                int tempX = currentCell.ArrayPosition[0] + directions[k].X;
                int tempY = currentCell.ArrayPosition[1] + directions[k].Y;

                //Debug.WriteLine("temp: " + tempX + "," + tempY);

                if (IsInArray(tempX, tempY, cellGrid) && !cellGrid[tempX, tempY].HasCollapsed)
                {
                    // Look up the adjacent tileset options and set them for the adjacent cell
                    int cellNum = currentCell.Options[0] - 1;
                    int dirNum = (k) % directions.Count;
                    cellGrid[tempX, tempY].Options = cellGrid[tempX, tempY].Options.Intersect(allCellOptions[cellNum][dirNum]).ToList();
                }
            }

            timeSinceStart = (float)Math.Round(gt.TotalGameTime.TotalSeconds - timeAtStart, 2);
        }

        /// <summary>
        /// General do it all at once collapse
        /// </summary>
        public void Collapse()
        {
            int numCollapsed = 0;
            Cell currentCell = cellGrid[0, 0];

            do
            {
                // Find cell first cell that hasn't collapsed
                foreach (Cell cell in cellGrid)
                {
                    if (cell.HasCollapsed) continue;

                    currentCell = cell;
                }

                List<int> optionsList = new List<int>();

                // Find the cell with the least options
                foreach (Cell cell in cellGrid)
                {
                    if (cell.HasCollapsed) continue;

                    if (cell.Options.Count <= currentCell.Options.Count)
                    {
                        currentCell = cell;
                    }
                }

                // Set the option for the currentCell
                int randomOption = currentCell.Options[random.Next(currentCell.Options.Count)];
                optionsList.Add(randomOption);
                currentCell.Options = optionsList;
                currentCell.HasCollapsed = true;
                currentCell.Asset = assets[optionsList[0]];
                Debug.WriteLine(currentCell.ArrayPosition[0] + "," + currentCell.ArrayPosition[1]);

                numCollapsed++;

                // Find cells next to current and set their options to the adjacent tile options fo current cell
                for (int k = 0; k < directions.Count; k++)
                {
                    int tempX = currentCell.ArrayPosition[0] + directions[k].X;
                    int tempY = currentCell.ArrayPosition[1] + directions[k].Y;

                    Debug.WriteLine("temp: " + tempX + "," + tempY);

                    // Look up the adjacent tileset options and set them for the adjacent cell
                    if (IsInArray(tempX, tempY, cellGrid) && !cellGrid[tempX, tempY].HasCollapsed)
                    {
                        int cellNum = currentCell.Options[0] - 1;
                        int dirNum = (k) % directions.Count;
                        cellGrid[tempX, tempY].Options = cellGrid[tempX, tempY].Options.Intersect(allCellOptions[cellNum][dirNum]).ToList();
                    }
                }


            } while (numCollapsed < cellGrid.Length);
        }

        /// <summary>
        /// Inserts cell into list from least to greatest # of options
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellList"></param>
        private void InsertCell(Cell cell, List<Cell> cellList)
        {
            if (cellList.Count == 0) { cellList.Add(cell); return; }

            for (int i = 0; i < cellList.Count; i++)
            {
                if (cell.Options.Count <= cellList[i].Options.Count)
                {
                    cellList.Insert(i, cell);
                    return;
                }
            }
        }

        /// <summary>
        /// Checks to see if a position is in the current array
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cellArray"></param>
        /// <returns></returns>
        private bool IsInArray(int x, int y, Cell[,] cellArray)
        {
            return
                x < cellArray.GetLength(0) && y < cellArray.GetLength(1) &&
                x >= 0 && y >= 0;
        }

        /// <summary>
        /// Set generic variables for the grid cells
        /// </summary>
        public void CreateGrid()
        {
            numCollapsed = 0;
            cellGrid = new Cell[width, height];
            nextCells = new List<Cell>();
            priorityCells = new PriorityQueue<Cell, int>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cellGrid[i, j] = new Cell(options, new Point(i * scale + position.X, j * scale + position.Y));
                    cellGrid[i, j].ArrayPosition = new int[] { i, j };
                }
            }

            nextCells.Add(cellGrid[0, 0]);
            priorityCells.Enqueue(cellGrid[0, 0], cellGrid[0, 0].Options.Count);
        }

        /// <summary>
        /// Generates adjacency options for each tile type
        /// </summary>
        public void CreateCellOptionList()
        {
            List<string> data = FileIO.ReadFrom("tiles");

            allCellOptions = new List<List<int>>[data.Count];
            options = new List<int>();

            // Setup the 3D list of information
            for (int i = 0; i < allCellOptions.Length; i++)
            {
                options.Add(i + 1);
                allCellOptions[i] = new List<List<int>>();

                for (int j = 0; j < 4; j++)
                {
                    allCellOptions[i].Add(new List<int>());
                }
            }

            string[] cellData;

            for (int i = 0; i < data.Count; i++)
            {
                cellData = data[i].Split(",");

                List<int> currentList = new List<int>();

                for (int j = 1; j < cellData.Length; j++)
                {

                    switch (cellData[j])
                    {
                        case "Left":

                            currentList = allCellOptions[i][0];
                            break;

                        case "Up":

                            currentList = allCellOptions[i][1];
                            break;

                        case "Right":

                            currentList = allCellOptions[i][2];
                            break;

                        case "Down":

                            currentList = allCellOptions[i][3];
                            break;

                        default:

                            currentList.Add(int.Parse(cellData[j]));
                            break;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Cell cell in cellGrid)
            {
                if (cell.Asset == null) continue;
                sb.Draw(cell.Asset, new Rectangle(cell.Position, new Point(scale, scale)), Color.White);
            }

            sb.Draw(assets[1], new Rectangle(40, 40, 520, 110), new Color(0,0,0,150));

            sb.DrawString(Game1.basicFont, timeSinceStart.ToString(), new Vector2(50, 50), Color.White);
        }
    }
}
