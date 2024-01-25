using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveCollapse___After
{
    internal class CellGrid
    {
        private Cell[,] cellGrid;
        private Cell[] cellArray;
        private Point position;
        private Dictionary<int, Texture2D> assets;

        private int width;
        private int height;
        private int scale;

        private Random random;

        private List<int> options = new List<int>();
        private List<List<int>>[] allCellOptions;
        private List<Point> directions;

        public CellGrid(int width, int height, Dictionary<int, Texture2D> assets, Point position, int scale)
        {
            this.cellGrid = new Cell[width, height];
            this.cellArray = new Cell[cellGrid.Length];
            this.assets = assets;
            this.position = position;

            this.width = width;
            this.height = height;
            this.scale = scale;

            this.random = new Random();

            this.directions = new List<Point>();
            this.directions.Add(new Point(-1, 0));
            this.directions.Add(new Point(1, 0));
            this.directions.Add(new Point(0, -1));
            this.directions.Add(new Point(0, 1));
        }

        public void Update()
        {

        }

        public void Collapse()
        {
            for (int i = 0; i < cellArray.Length; i++)
            {
                Cell currentCell = cellArray[0];
                int[] options = new int[0];
                List<int> optionsList = new List<int>();

                for (int j = 0; j < cellArray.Length; j++)
                {
                    if ((cellArray[j].HasCollapsed || cellArray[j].Options.Count == this.options.Count) && i > 0) continue;

                    if (cellArray[j].Options.Count < currentCell.Options.Count)
                    {
                        currentCell = cellArray[j];
                    }
                }

                options = currentCell.Options.ToArray();
                optionsList.Add(options[random.Next(options.Length)]);
                currentCell.Options = optionsList;
                currentCell.HasCollapsed = true;
                currentCell.Asset = assets[optionsList[0]];

                for (int k = 0; k < directions.Count; k++)
                {
                    int tempX = currentCell.ArrayPosition[0] + directions[k].X;
                    int tempY = currentCell.ArrayPosition[1] + directions[k].Y;

                    if (IsInArray(tempX,tempY, cellGrid) && !cellGrid[tempX,tempY].HasCollapsed)
                    {
                        int cellNum = currentCell.Options[0] - 1;
                        int dirNum = (k + 2) % directions.Count;
                        cellGrid[tempX, tempY].Options.Intersect(allCellOptions[cellNum][dirNum]).ToList(); 
                    }
                }
            }
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
            cellGrid = new Cell[width, height];

            int index = 0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cellGrid[i, j] = new Cell(options, new Point(i * scale + position.X, j * scale + position.Y));
                    cellGrid[i, j].ArrayPosition = new int[] { i, j };

                    cellArray[index++] = cellGrid[i, j];
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

                    switch(cellData[j])
                    {
                        case "Left":

                            currentList = allCellOptions[i][0];
                            break;

                        case "Right":

                            currentList = allCellOptions[i][1];
                            break;

                        case "Up":

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
