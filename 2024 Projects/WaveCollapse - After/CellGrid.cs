using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
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
        }

        public void Update()
        {

        }

        public void Collapse1By1()
        {
            if (numCollapsed >= cellGrid.Length)
            {
                CreateGrid();
                return;
            }

            foreach (Cell cell in cellGrid)
            {
                if (cell.HasCollapsed) continue;

                currentCell = cell;
            }

            List<int> optionsList = new List<int>();

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

            if (currentCell.Options.Count == 0)
            {
                CreateGrid();
                return;
            }

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
                }
            }
        }

        public void Collapse()
        {
            int numCollapsed = 0;
            Cell currentCell = cellGrid[0, 0];

            do
            {
                foreach (Cell cell in cellGrid)
                {
                    if (cell.HasCollapsed) continue;

                    currentCell = cell;
                }

                List<int> optionsList = new List<int>();

                foreach (Cell cell in cellGrid)
                {
                    if (cell.HasCollapsed) continue;

                    if (cell.Options.Count <= currentCell.Options.Count)
                    {
                        currentCell = cell;
                    }
                }

                int randomOption = currentCell.Options[random.Next(currentCell.Options.Count)];
                optionsList.Add(randomOption);
                currentCell.Options = optionsList;
                currentCell.HasCollapsed = true;
                currentCell.Asset = assets[optionsList[0]];
                Debug.WriteLine(currentCell.ArrayPosition[0] + "," + currentCell.ArrayPosition[1]);

                numCollapsed++;

                for (int k = 0; k < directions.Count; k++)
                {
                    int tempX = currentCell.ArrayPosition[0] + directions[k].X;
                    int tempY = currentCell.ArrayPosition[1] + directions[k].Y;

                    Debug.WriteLine("temp: " + tempX + "," + tempY);

                    if (IsInArray(tempX, tempY, cellGrid) && !cellGrid[tempX, tempY].HasCollapsed)
                    {
                        int cellNum = currentCell.Options[0] - 1;
                        int dirNum = (k) % directions.Count;
                        cellGrid[tempX, tempY].Options = cellGrid[tempX, tempY].Options.Intersect(allCellOptions[cellNum][dirNum]).ToList();
                    }
                }


            } while (numCollapsed < cellGrid.Length);
        }

        private bool IsInArray(int x, int y, Cell[,] cellArray)
        {
            return
                x < cellArray.GetLength(0) && y < cellArray.GetLength(1) &&
                x >= 0 && y >= 0;
        }

        // Set generic variables for the grid cells
        public void CreateGrid()
        {
            numCollapsed = 0;
            cellGrid = new Cell[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cellGrid[i, j] = new Cell(options, new Point(i * scale + position.X, j * scale + position.Y));
                    cellGrid[i, j].ArrayPosition = new int[] { i, j };
                }
            }
        }

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
        }
    }
}
