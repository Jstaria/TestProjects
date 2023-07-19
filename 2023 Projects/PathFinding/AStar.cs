using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PathFinding
{
    internal class AStar
    {
        private Tile[,] visualGrid;
        private Random rng;

        private int padding;

        private Texture2D asset;

        public AStar(int padding, int XYDimension, Texture2D asset, SpriteFont font)
        {
            rng = new Random();

            this.padding = padding;

            visualGrid = new Tile[XYDimension, XYDimension];

            for (int i = 0; i < XYDimension; i++)
            {
                for (int j = 0; j < XYDimension; j++)
                {
                    bool isWallRNG = false;

                    if (rng.Next(100) > 75)
                    {
                        isWallRNG = true;
                    }

                    visualGrid[i, j] = new Tile(
                        new Rectangle(
                            -padding + (i * ((1000 + padding * 2) / XYDimension)),
                            -padding + (j * ((1000 + padding * 2) / XYDimension)),
                            (1000 + padding * 2) / XYDimension,
                            (1000 + padding * 2) / XYDimension),
                        asset,
                        isWallRNG,
                        i, j,
                        font);
                }
            }
        }

        public Tile[,] Algo(Vector2 pos)
        {
            ResetTiles();

            Tile start = visualGrid[
                (int)Math.Clamp(Math.Round((pos.X + padding), 10, MidpointRounding.AwayFromZero) / ((1000 + padding * 2) / visualGrid.GetLength(0)), 0, visualGrid.GetLength(0) - 1),
                (int)Math.Clamp(Math.Round((pos.Y + padding), 10, MidpointRounding.AwayFromZero) / ((1000 + padding * 2) / visualGrid.GetLength(1)), 0, visualGrid.GetLength(1) - 1)];

            start.Number = 1;
            start.IsPermenant = true;

            List<Tile> check = new List<Tile>
            {
                start
            } ;

            while (check.Count != 0)
            {
                for (int i = 0; i < check.Count; i++)
                {
                    int num = check[i].Number;

                    Tile next = null;

                    while ((next = GetUnvisitedNeighbor(check[i])) != null)
                    {
                        next.Number = num + 1;
                        next.IsPermenant = true;
                        check.Add(next);
                        
                    }

                    check.Remove(check[0]);
                    i--;
                }
            }

            return visualGrid;
        }

        private Tile GetUnvisitedNeighbor(Tile current)
        {
            Point currentIndex = new Point(current.x,current.y);

            // Top Right
            if (currentIndex.X + 1 < visualGrid.GetLength(0) && currentIndex.Y - 1 >= 0
            && !visualGrid[currentIndex.X + 1, currentIndex.Y - 1].IsPermenant && 
            (!visualGrid[currentIndex.X + 1, currentIndex.Y].IsWall &&
            !visualGrid[currentIndex.X, currentIndex.Y - 1].IsWall))
            {
                return visualGrid[currentIndex.X + 1, currentIndex.Y - 1];
            }

            // Middle Right
            if (currentIndex.X + 1 < visualGrid.GetLength(0)
                && !visualGrid[currentIndex.X + 1, currentIndex.Y].IsPermenant)
            {
                return visualGrid[currentIndex.X + 1, currentIndex.Y];
            }

            // Bottom Right
            if (currentIndex.X + 1 < visualGrid.GetLength(0) && currentIndex.Y + 1 < visualGrid.GetLength(1)
                && !visualGrid[currentIndex.X + 1, currentIndex.Y + 1].IsPermenant &&
                (!visualGrid[currentIndex.X + 1, currentIndex.Y].IsWall &&
                !visualGrid[currentIndex.X, currentIndex.Y + 1].IsWall))
            {
                return visualGrid[currentIndex.X + 1, currentIndex.Y + 1];
            }

            // Bottom Middle
            if (currentIndex.Y + 1 < visualGrid.GetLength(1)
                && !visualGrid[currentIndex.X, currentIndex.Y + 1].IsPermenant)
            {
                return visualGrid[currentIndex.X, currentIndex.Y + 1];
            }

            // Bottom Left
            if (currentIndex.X - 1 >= 0 && currentIndex.Y + 1 < visualGrid.GetLength(1)
                && !visualGrid[currentIndex.X - 1, currentIndex.Y + 1].IsPermenant &&
                (!visualGrid[currentIndex.X - 1, currentIndex.Y].IsWall &&
                !visualGrid[currentIndex.X, currentIndex.Y + 1].IsWall))
            {
                return visualGrid[currentIndex.X - 1, currentIndex.Y + 1];
            }

            // Middle Left
            if (currentIndex.X - 1 >= 0
                && !visualGrid[currentIndex.X - 1, currentIndex.Y].IsPermenant)
            {
                return visualGrid[currentIndex.X - 1, currentIndex.Y];
            }

            // Top Left
            if (currentIndex.X - 1 >= 0 && currentIndex.Y - 1 >= 0
                && !visualGrid[currentIndex.X - 1, currentIndex.Y - 1].IsPermenant &&
                (!visualGrid[currentIndex.X - 1, currentIndex.Y].IsWall &&
                !visualGrid[currentIndex.X, currentIndex.Y - 1].IsWall))
            {
                return visualGrid[currentIndex.X - 1, currentIndex.Y - 1];
            }

            // Top Middle
            if (currentIndex.Y - 1 >= 0
                && !visualGrid[currentIndex.X, currentIndex.Y - 1].IsPermenant)
            {
                return visualGrid[currentIndex.X, currentIndex.Y - 1];
            }


            return null!;
        }

        private void ResetTiles()
        {
            for (int i = 0; i < visualGrid.GetLength(0); i++)
            {
                for (int j = 0; j < visualGrid.GetLength(1); j++)
                {
                    if (visualGrid[i, j].IsWall)
                    {
                        visualGrid[i, j].IsPermenant = true;
                    }
                    else
                    {
                        visualGrid[i, j].IsPermenant = false;
                    }
                    
                    visualGrid[i, j].Number = int.MaxValue;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Tile tile in visualGrid)
            {
                tile.DrawTile(sb);
            }
        }
    }
}
