using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WaveCollapse___After
{
    internal class CellGrid
    {
        private Cell[,] cellGrid;
        private Point position;
        private Texture2D[] assets;

        private int width;
        private int height;
        private int scale;

        private List<int> options = new List<int>();

        private List<List<int>>[] allCellOptions;

        public CellGrid(int width, int height, Texture2D[] assets, Point position, int scale)
        {
            this.cellGrid = new Cell[width, height];
            this.assets = assets;
            this.position = position;

            this.width = width;
            this.height = height;
            this.scale = scale;
        }

        public void Update()
        {

        }

        // Set generic variables for the grid cells
        public void CreateGrid()
        {
            cellGrid = new Cell[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cellGrid[i, j] = new Cell(options, new Point(i * scale + position.X, j * scale + position.Y));
                }
            }
        }

        public void CreateCellOptionList()
        {
            List<string> data = FileIO.ReadFrom("tiles");

            allCellOptions = new List<List<int>>[data.Count];

            // Setup the 3D list of information
            for (int i = 0; i < allCellOptions.Length; i++)
            {
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
    }
}
