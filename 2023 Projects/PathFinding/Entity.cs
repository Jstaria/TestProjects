using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PathFinding
{
    internal class Entity
    {
        private Texture2D asset;
        private Vector2 position;

        private const int size = 10;
        private Vector2 centerOffset;

        private bool isMoving;

        public Entity(Texture2D asset, int gridX, int gridY, int gridLengthX)
        {
            this.asset = asset;

            this.centerOffset = new Vector2(1000 / gridLengthX / 2, 1000 / gridLengthX / 2);

            this.position = new Vector2(
                gridX * (1000 / gridLengthX) + centerOffset.X, 
                gridY * (1000 / gridLengthX) + centerOffset.Y);

            this.isMoving = false;
        }

        public void Update(Tile[,] visualGrid)
        {
            int padding = 0;

            Tile current = visualGrid[
                (int)Math.Clamp(Math.Round((position.X + padding), 10, MidpointRounding.AwayFromZero) / ((1000 + padding * 2) / visualGrid.GetLength(0)), 0, visualGrid.GetLength(0) - 1),
                (int)Math.Clamp(Math.Round((position.Y + padding), 10, MidpointRounding.AwayFromZero) / ((1000 + padding * 2) / visualGrid.GetLength(1)), 0, visualGrid.GetLength(1) - 1)];

            Vector2 nextPosition = CheckNeighbors(current, visualGrid);

            int num = 50;

            if (nextPosition != position)
            {
                if (this.position.X < nextPosition.X)
                {
                    this.position.X += (centerOffset.X * 2 / num);
                }
                else if (this.position.X > nextPosition.X)
                {
                    this.position.X -= (centerOffset.X * 2 / num);
                }

                if (this.position.Y < nextPosition.Y)
                {
                    this.position.Y += (centerOffset.Y * 2 / num);
                }
                else if (this.position.Y > nextPosition.Y)
                {
                    this.position.Y -= (centerOffset.Y * 2 / num);
                }
            }
        }

        private Vector2 CheckNeighbors(Tile current, Tile[,] visualGrid)
        {
            Vector2 nextLocation = current.PositionV;

            int currentNum = current.Number;

            Tile[] surroundingTiles = new Tile[8];


            if (current.y - 1 >= 0 && !visualGrid[current.x, current.y - 1].IsWall) // top
            {
                surroundingTiles[0] = visualGrid[current.x, current.y - 1];
            }
            
            if (current.y + 1 < visualGrid.GetLength(1) && !visualGrid[current.x, current.y + 1].IsWall) // bottom
            {
                surroundingTiles[1] = visualGrid[current.x, current.y + 1];
            }
            
            if (current.x + 1 < visualGrid.GetLength(0) && !visualGrid[current.x + 1, current.y].IsWall) // Right
            {
                surroundingTiles[2] = visualGrid[current.x + 1, current.y];
            }

            if (current.x - 1 >= 0 && !visualGrid[current.x - 1, current.y].IsWall) // left
            {
                surroundingTiles[4] = visualGrid[current.x - 1, current.y];
            }

            if (current.x + 1 < visualGrid.GetLength(0) && current.y - 1 >= 0 && !visualGrid[current.x + 1, current.y - 1].IsWall) // top right
            {
                surroundingTiles[3] = visualGrid[current.x + 1, current.y - 1];
            }

            if (current.x + 1 < visualGrid.GetLength(0) && current.y + 1 < visualGrid.GetLength(1) && !visualGrid[current.x + 1, current.y + 1].IsWall) // bottom right
            {
                surroundingTiles[5] = visualGrid[current.x + 1, current.y + 1];
            }

            if (current.x - 1 >= 0 && current.y - 1 >= 0 && !visualGrid[current.x - 1, current.y - 1].IsWall) // top left
            {
                surroundingTiles[6] = visualGrid[current.x - 1, current.y - 1];
            }

            if (current.x - 1 >= 0 && current.y + 1 < visualGrid.GetLength(1) && !visualGrid[current.x - 1, current.y + 1].IsWall)
            {
                surroundingTiles[7] = visualGrid[current.x - 1, current.y + 1]; // bottom left
            }

            for (int i = 0; i < surroundingTiles.Length; i++)
            {
                if (surroundingTiles[i] != null && surroundingTiles[i].Number == currentNum - 1)
                {
                    nextLocation = surroundingTiles[i].PositionV + centerOffset;
                    return nextLocation;
                }
            }

            return nextLocation + centerOffset;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, new Rectangle((int)position.X - size / 2, (int)position.Y - size / 2, size, 10), Color.Purple);
        }

    }
}
