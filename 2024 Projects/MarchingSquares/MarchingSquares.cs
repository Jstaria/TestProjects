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

        private float[,] squareArray;

        private Random random;

        private Texture2D circleAsset;

        private Dictionary<string,Vector2> Lines;

        private FastNoiseLite noise;

        public MarchingSquares(int width, int height, int scale, Texture2D circleAsset)
        {
            this.width = width;
            this.height = height;

            this.squareArray = new float[width + 1, height + 1];

            this.random = new Random();

            this.circleAsset = circleAsset;
            this.scale = scale;

            this.noise = new FastNoiseLite();
            this.noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            this.noise.SetSeed(random.Next());

            CreateLineList();
        }

        public void SetValuesRandomly()
        {
            for (int i = 0; i < squareArray.GetLength(0); i++)
            {
                for (int j = 0; j < squareArray.GetLength(1); j++)
                {
                    squareArray[i, j] = noise.GetNoise(i,j);
                }
            }
        }

        private void CreateLineList()
        {
            Vector2 a = new Vector2(scale * .5f, 0); // top edge
            Vector2 b = new Vector2(scale, scale * .5f);   // right edge
            Vector2 c = new Vector2(scale * .5f, scale);  // bottom edge
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
            for (int i = 0; i < width + 1; i++)
            {
                for (int j = 0; j < height + 1; j++)
                {
                    if (squareArray[i,j] <= .001) { continue; }

                    Vector2 position = new Vector2(i * scale, j * scale);

                    ShapeBatch.Circle(position, scale, Color.White * squareArray[i, j]);
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector2 position = new Vector2(i * scale, j * scale);

                    ShapeBatch.Circle(position, scale / 10, new Color(255 * squareArray[i, j], 255 * squareArray[i, j], 255 * squareArray[i, j]));

                    float a = squareArray[i, j];
                    float b = squareArray[i + 1, j];
                    float c = squareArray[i + 1, j + 1];
                    float d = squareArray[i, j + 1];

                    int triangleNum = GetState((int)Math.Ceiling(a), (int)Math.Ceiling(b), (int)Math.Ceiling(c), (int)Math.Ceiling(d));

                    //position += new Vector2(scale / 2, scale / 2);

                    DrawLine(triangleNum, scale / 15, Color.White, position);
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
