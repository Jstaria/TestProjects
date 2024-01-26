using AfterImage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarchingSquares
{
    internal class MarchingSquares
    {
        private int width;
        private int height;
        private int scale;

        private int[,] squareArray;

        private Random random;

        private Texture2D circleAsset;

        private Dictionary<string,Vector2> Lines;

        public MarchingSquares(int width, int height, int scale, Texture2D circleAsset)
        {
            this.width = width;
            this.height = height;

            this.squareArray = new int[width + 1, height + 1];

            this.random = new Random(0);

            this.circleAsset = circleAsset;
            this.scale = scale;

            CreateLineList();
        }

        public void SetValuesRandomly()
        {
            for (int i = 0; i < squareArray.GetLength(0); i++)
            {
                for (int j = 0; j < squareArray.GetLength(1); j++)
                {
                    squareArray[i, j] = random.Next(2);
                }
            }
        }

        private void CreateLineList()
        {
            Vector2 a = new Vector2(scale * .5f, 0); // top edge
            Vector2 b = new Vector2(scale, scale * .5f);   // right edge
            Vector2 c = new Vector2(scale, scale * .5f);  // bottom edge
            Vector2 d = new Vector2(0, scale * .5f);  // left edge

            Lines = new Dictionary<string, Vector2>()
            {
                { "a", a },
                { "b", b },
                { "c", c },
                { "d", d },
            };
        }

        private int GetState(int a, int b, int c, int d)
        {
            return a * 8 + b * 4 + c * 2 + d * 1;
        }

        public void Draw()
        {
            for (int i = 0; i < squareArray.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < squareArray.GetLength(1) - 1; j++)
                {
                    Vector2 position = new Vector2(i * scale, j * scale);

                    ShapeBatch.Circle(position, scale / 10, new Color(255 * squareArray[i, j], 255 * squareArray[i, j], 255 * squareArray[i, j]));

                    int aa = squareArray[i, j];
                    int ab = squareArray[i + 1, j];
                    int ba = squareArray[i, j + 1];
                    int bb = squareArray[i + 1, j + 1];

                    int triangleNum = aa * 8 + ab * 4 + ba * 2 + bb * 1;

                    DrawLine(triangleNum, scale / 10, Color.White, position);
                }
            }
        }

        private void DrawLine(float num, float width, Color color, Vector2 position)
        {
            switch (num)
            {
                case 1:
                case 14:
                    ShapeBatch.Line(Lines["c"] + position, Lines["d"] + position, width, color);
                    break;

                case 2:
                case 13:
                    ShapeBatch.Line(Lines["b"] + position, Lines["c"] + position, width, color);
                    break;

                case 3:
                case 12:
                    ShapeBatch.Line(Lines["b"] + position, Lines["d"] + position, width, color);
                    break;

                case 4:
                case 11:
                    ShapeBatch.Line(Lines["a"] + position, Lines["b"] + position, width, color);
                    break;

                case 5:
                    ShapeBatch.Line(Lines["a"] + position, Lines["d"] + position, width, color);
                    ShapeBatch.Line(Lines["b"] + position, Lines["c"] + position, width, color);
                    break;

                case 6:
                case 9:
                    ShapeBatch.Line(Lines["a"] + position, Lines["c"] + position, width, color);
                    break;

                case 7:
                case 8:
                    ShapeBatch.Line(Lines["a"] + position, Lines["d"] + position, width, color);
                    break;

                case 10:
                    ShapeBatch.Line(Lines["a"] + position, Lines["b"] + position, width, color);
                    ShapeBatch.Line(Lines["c"] + position, Lines["d"] + position, width, color);
                    break;
            }
        }
    }
}
