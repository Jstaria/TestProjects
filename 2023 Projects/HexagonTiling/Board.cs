using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexagonTiling
{
    struct Polygon
    {
        public List<Vector2> points; // Transformed points
        public Vector2 origin;   // Position of shape
        public float angle;      // Direction of shape
        public List<Vector2> model;  // "Model" of shape
        public bool connectedCol;
    }

    internal class Board
    {
        private Polygon mousePos;

        private double time;
        private int imageSize;

        private Vector2 startPos;
        private Texture2D asset;

        private Tile[,] board;
        private Polygon[,] boardObjColBoxes;

        public Board(Vector2 startPos, Texture2D asset, int Columns, int Rows, int spacing, int imageSize)
        {
            this.startPos = startPos;
            this.asset = asset;

            this.imageSize = imageSize;

            this.board = new Tile[Rows, Columns];
            this.boardObjColBoxes = new Polygon[Rows, Columns];

            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    board[row,col] = new Tile(
                        startPos + new Vector2(
                            col * (imageSize + spacing) * .80f, 
                            row * (imageSize + spacing) + ((imageSize + spacing) / 2) * (col % 2)), 
                        asset, 
                        imageSize);

                    boardObjColBoxes[row, col] = new Polygon();
                    boardObjColBoxes[row, col].origin = board[row, col].Origin;

                    // Hexagon
                    float theta = (float)Math.PI * 2.0f / 6.0f;

                    boardObjColBoxes[row, col].points = new List<Vector2>();
                    boardObjColBoxes[row, col].model = new List<Vector2>();

                    for (int i = 0; i < 6; i++)
                    {
                        boardObjColBoxes[row, col].points.Add(new Vector2(imageSize / 2 * (float)Math.Cos(theta * i), imageSize / 2 * (float)Math.Sin(theta * i)));
                        boardObjColBoxes[row, col].model.Add(new Vector2(imageSize / 2 * (float)Math.Cos(theta * i), imageSize / 2 * (float)Math.Sin(theta * i)));
                    }
                }
            }

            UpdateShapes();
        }

        public void UpdateShapes()
        {
            // for each polygon
            for (int i = 0; i < boardObjColBoxes.GetLength(0); i++)
            {
                for (int j = 0; j < boardObjColBoxes.GetLength(1); j++)
                {
                    Polygon polygon = boardObjColBoxes[i,j];

                    polygon.connectedCol = false;

                    boardObjColBoxes[i,j] = polygon;

                    // For each point
                    for (int k = 0; k < boardObjColBoxes[i, k].points.Count; k++)
                    {
                        Vector2 point = boardObjColBoxes[i, j].model[k];
                        float angle = boardObjColBoxes[i, j].angle;
                        Vector2 origin = boardObjColBoxes[i, j].origin;

                        // Rotates points via 'angle'
                        boardObjColBoxes[i, j].points[k] =
                            new Vector2(
                                ((point.X * (float)Math.Cos(angle)) - (point.Y * (float)Math.Sin(angle)) + origin.X),
                                ((point.X * (float)Math.Sin(angle)) + (point.Y * (float)Math.Cos(angle)) + origin.Y));
                    }
                }
            }
        }

        public void UpdateShape()
        {
            // For each point
            for (int k = 0; k < mousePos.points.Count; k++)
            {
                Vector2 point = mousePos.model[k];
                float angle = mousePos.angle;
                Vector2 origin = mousePos.origin;

                // Rotates points via 'angle'
                mousePos.points[k] =
                    new Vector2(
                        ((point.X * (float)Math.Cos(angle)) - (point.Y * (float)Math.Sin(angle)) + origin.X),
                        ((point.X * (float)Math.Sin(angle)) + (point.Y * (float)Math.Cos(angle)) + origin.Y));
            }
        }

        public bool Collide(Polygon one, Polygon two)
        {
            Polygon polygon1 = one;
            Polygon polygon2 = two;

            for (int shape = 0; shape < 2; shape++)
            {
                // Swap to check each shape on each other
                if (shape == 1)
                {
                    polygon1 = two;
                    polygon2 = one;
                }

                // For each edge
                for (int i = 0; i < polygon1.points.Count; i++)
                {
                    int j = (i + 1) % polygon1.points.Count;

                    // Creates Normal
                    Vector2 axisProj = new Vector2(-(polygon1.points[j].Y - polygon1.points[i].Y), polygon1.points[j].X - polygon1.points[i].X);

                    // Sets min and max for new 1D axis
                    float min_r1 = float.PositiveInfinity;
                    float max_r1 = float.NegativeInfinity;

                    for (int points = 0; points < polygon1.points.Count; points++)
                    {
                        float dot = (polygon1.points[points].X * axisProj.X + polygon1.points[points].Y * axisProj.Y);

                        min_r1 = Math.Min(min_r1, dot);
                        max_r1 = Math.Max(max_r1, dot);
                    }

                    // Sets min and max for new 1D axis along shape two
                    float min_r2 = float.PositiveInfinity;
                    float max_r2 = float.NegativeInfinity;

                    for (int points = 0; points < polygon2.points.Count; points++)
                    {
                        float dot = (polygon2.points[points].X * axisProj.X + polygon2.points[points].Y * axisProj.Y);

                        min_r2 = Math.Min(min_r2, dot);
                        max_r2 = Math.Max(max_r2, dot);
                    }

                    if (!(max_r2 >= min_r1 && max_r1 >= min_r2))
                    {
                        //System.Diagnostics.Debug.WriteLine(false);
                        return false;
                    }
                }
            }
            //System.Diagnostics.Debug.WriteLine(true);
            return true;
        }

        public void Update(GameTime gameTime)
        {
            UpdateShapes();

            mousePos = new Polygon();

            MouseState mPos = Mouse.GetState();

            int mouseSize = 10;

            mousePos.origin = new Vector2(mPos.X, mPos.Y);
            mousePos.points = new List<Vector2>()
            {
                new Vector2(- mouseSize, - mouseSize),
                new Vector2(- mouseSize, + mouseSize),
                new Vector2(+ mouseSize, + mouseSize),
                new Vector2(+ mouseSize, - mouseSize)
            };
            mousePos.model = new List<Vector2>()
            {
                new Vector2(- mouseSize, - mouseSize),
                new Vector2(- mouseSize, + mouseSize),
                new Vector2(+ mouseSize, + mouseSize),
                new Vector2(+ mouseSize, - mouseSize)
            };

            UpdateShape();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {

                    bool collide = false;

                    if (new Rectangle((int)mousePos.points[0].X, (int)mousePos.points[0].Y, mouseSize * 2, mouseSize * 2).Intersects(board[i,j].Position))
                    {
                        collide = Collide(boardObjColBoxes[i, j], mousePos);

                        //System.Diagnostics.Debug.WriteLine(collide);   
                    }


                    // I don't know if there is a less ugly way to do this because it is a 2D array of hexagons
                    // therefore there are two different rules for the adjacent tiles
                    if (collide && j % 2 == 0)
                    {
                        if (i - 1 >= 0)
                        {
                            boardObjColBoxes[i - 1, j].connectedCol = true;
                        }

                        if (i - 1 >= 0 && j - 1 >= 0)
                        {
                            boardObjColBoxes[i - 1, j - 1].connectedCol = true;
                        }

                        if (i - 1 >= 0 && j + 1 < board.GetLength(1))
                        {
                            boardObjColBoxes[i - 1, j + 1].connectedCol = true;
                        }

                        if (j - 1 >= 0)
                        {
                            boardObjColBoxes[i, j - 1].connectedCol = true;
                        }

                        if (j + 1 < board.GetLength(1))
                        {
                            boardObjColBoxes[i, j + 1].connectedCol = true;
                        }

                        if (i + 1 < board.GetLength(0))
                        {
                            boardObjColBoxes[i + 1, j].connectedCol = true;
                        }
                    }

                    else if (collide && j % 2 == 1)
                    {
                        if (i - 1 >= 0)
                        {
                            boardObjColBoxes[i - 1, j].connectedCol = true;
                        }

                        if (j - 1 >= 0)
                        {
                            boardObjColBoxes[i, j - 1].connectedCol = true;
                        }

                        if (j + 1 < board.GetLength(1))
                        {
                            boardObjColBoxes[i, j + 1].connectedCol = true;
                        }

                        if (i + 1 < board.GetLength(0) && j - 1 >= 0)
                        {
                            boardObjColBoxes[i + 1, j - 1].connectedCol = true;
                        }

                        if (i + 1 < board.GetLength(0) && j + 1 < board.GetLength(1))
                        {
                            boardObjColBoxes[i + 1, j + 1].connectedCol = true;
                        }

                        if (i + 1 < board.GetLength(0))
                        {
                            boardObjColBoxes[i + 1, j].connectedCol = true;
                        }
                    }

                    board[i, j].IsInCollision = collide;
                }
            }

            time = gameTime.TotalGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch sb, GraphicsDevice GraphicsDevice, Effect effect)
        {
            foreach (Tile tile in board)
            {
                tile.Draw(sb, effect, time);
            }

            sb.End();
            ShapeBatch.Begin(GraphicsDevice);

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Polygon polygon = boardObjColBoxes[i, j];

                    Color color = Color.DarkOrange;
                    float lineWidth = imageSize / 20;

                    if (polygon.connectedCol && !board[i,j].IsInCollision)
                    {
                        color = Color.Lime;
                        lineWidth = imageSize / 20 * 1.5f;
                    }

                    for (int k = 0; k < polygon.points.Count; k++)
                    {
                        ShapeBatch.Line(polygon.points[k], polygon.points[(k + 1) % polygon.points.Count], lineWidth, color);
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                ShapeBatch.Line(mousePos.points[i], mousePos.points[(i + 1) % mousePos.points.Count], 3, Color.Firebrick * .8f);
            }

            ShapeBatch.End();
            sb.Begin();
        }
    }
}
