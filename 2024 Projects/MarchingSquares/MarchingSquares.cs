using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
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
        private float speed;

        private float smoothOffset;

        private SquarePointData[,] squareArray;
        private Random random;
        private Texture2D circleAsset;
        private Dictionary<string, Vector2> lines;
        private Dictionary<string, Vector2> smoothedLines;
        private FastNoiseLite noise;
        private bool isSmooth = true;
        private float count = 0;
        private float prevScrollValue;

        private List<Vector2> circles = new List<Vector2>() { new Vector2(500, 500), new Vector2(750, 750) };

        public bool IsSmooth { get { return isSmooth; } }

        public MarchingSquares(int width, int height, int screenHeight, Texture2D circleAsset, float speed, float frequency)
        {
            this.width = width;
            this.height = height;
            this.scale = screenHeight / height;
            this.speed = speed;

            this.squareArray = new SquarePointData[width + 1, height + 1];

            SetPositions();
            CreateLineList();

            this.random = new Random();

            this.circleAsset = circleAsset;

            this.noise = new FastNoiseLite();
            this.noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            this.noise.SetSeed(random.Next());
            this.noise.SetFrequency(frequency);
        }

        public MarchingSquares(int width, int height, int screenHeight, Texture2D circleAsset, float speed, float frequency, int seed)
        {
            this.width = width;
            this.height = height;
            this.scale = screenHeight / height;
            this.speed = speed;

            this.squareArray = new SquarePointData[width + 1, height + 1];

            SetPositions();
            CreateLineList();

            this.random = new Random();

            this.circleAsset = circleAsset;

            this.noise = new FastNoiseLite();
            this.noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            this.noise.SetSeed(seed);
            this.noise.SetFrequency(frequency);
        }


        /// <summary>
        /// Update the values for the noise in the squareArray
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < width + 1; i++)
            {
                for (int j = 0; j < height + 1; j++)
                {
                    squareArray[i, j].Data = noise.GetNoise(squareArray[i, j].Position.X, squareArray[i, j].Position.Y, count);
                }
            }

            count += speed;

            float currentScrollValue = Mouse.GetState().ScrollWheelValue;

            if (currentScrollValue - prevScrollValue < 0) smoothOffset -= .1f;
            if (currentScrollValue - prevScrollValue > 0) smoothOffset += .1f;

            smoothOffset = MathHelper.Clamp(smoothOffset, 0, 1);

            prevScrollValue = currentScrollValue;
        }

        #region startMethods

        // Sets values for array using noise once
        public void SetValuesRandomly()
        {
            for (int i = 0; i < squareArray.GetLength(0); i++)
            {
                for (int j = 0; j < squareArray.GetLength(1); j++)
                {
                    squareArray[i, j].Data = noise.GetNoise(i, j);
                }
            }
        }

        private void SetPositions()
        {
            for (int i = 0; i < width + 1; i++)
            {
                for (int j = 0; j < height + 1; j++)
                {
                    Vector2 position = new Vector2(i * scale, j * scale);

                    squareArray[i, j] = new SquarePointData(position);
                }
            }
        }

        #endregion

        private int GetState(int a, int b, int c, int d)
        {
            return a * 8 + b * 4 + c * 2 + d * 1;
        }

        #region metaBalls

        public void UpdateMetaBalls()
        {
            circles[1] = circles[1] + new Vector2(1, 0);

            for (int i = 0; i < width + 1; i++)
            {
                for (int j = 0; j < height + 1; j++)
                {
                    float d = 0;

                    foreach (Vector2 pos in circles)
                    {
                        d += Math.Clamp(1 - (Vector2.Distance(squareArray[i, j].Position, pos) / 200), 0, int.MaxValue);

                    }

                    squareArray[i, j].Data = d;
                }
            }
        }

        #endregion

        #region nonsmoothed

        public void Draw()
        {
            //for (int i = 0; i < width + 1; i++)
            //{
            //    for (int j = 0; j < height + 1; j++)
            //    {
            //        ShapeBatch.Circle(squareArray[i, j].Position, 10, Color.White * squareArray[i, j].Data);

            //    }
            //}

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float a = squareArray[i, j].Data;
                    float b = squareArray[i + 1, j].Data;
                    float c = squareArray[i + 1, j + 1].Data;
                    float d = squareArray[i, j + 1].Data;

                    int triangleNum = GetState((int)Math.Ceiling(a), (int)Math.Ceiling(b), (int)Math.Ceiling(c), (int)Math.Ceiling(d));

                    DrawLine(triangleNum, scale / 15, Color.White, squareArray[i, j].Position);
                }
            }
        }

        private void CreateLineList()
        {
            float lerpAmt = .5f;
            Vector2 a = new Vector2(MathHelper.Lerp(0, scale, lerpAmt), 0); // top edge
            Vector2 b = new Vector2(scale, MathHelper.Lerp(0, scale, lerpAmt));   // right edge
            Vector2 c = new Vector2(MathHelper.Lerp(0, scale, lerpAmt), scale);  // bottom edge
            Vector2 d = new Vector2(0, MathHelper.Lerp(0, scale, lerpAmt));  // left edge

            lines = new Dictionary<string, Vector2>()
            {
                { "a", a },
                { "b", b },
                { "c", c },
                { "d", d },
            };
        }

        private void DrawLine(float num, float width, Color color, Vector2 position)
        {
            switch (num)
            {
                case 1:
                case 14:
                    ShapeBatch.Line(lines["c"] + position, lines["d"] + position, width, color);
                    break;

                case 2:
                case 13:
                    ShapeBatch.Line(lines["b"] + position, lines["c"] + position, width, color);
                    break;

                case 3:
                case 12:
                    ShapeBatch.Line(lines["b"] + position, lines["d"] + position, width, color);
                    break;

                case 4:
                case 11:
                    ShapeBatch.Line(lines["a"] + position, lines["b"] + position, width, color);
                    break;

                case 5:
                    ShapeBatch.Line(lines["a"] + position, lines["d"] + position, width, color);
                    ShapeBatch.Line(lines["b"] + position, lines["c"] + position, width, color);
                    break;

                case 6:
                case 9:
                    ShapeBatch.Line(lines["a"] + position, lines["c"] + position, width, color);
                    break;

                case 7:
                case 8:
                    ShapeBatch.Line(lines["a"] + position, lines["d"] + position, width, color);
                    break;

                case 10:
                    ShapeBatch.Line(lines["a"] + position, lines["b"] + position, width, color);
                    ShapeBatch.Line(lines["c"] + position, lines["d"] + position, width, color);
                    break;
            }
        }

        #endregion

        #region smoothed

        public void DrawSmooth()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //ShapeBatch.Circle(squareArray[i,j].Position, scale / 10, new Color(255 * squareArray[i, j].Data, 255 * squareArray[i, j].Data, 255 * squareArray[i, j].Data));

                    float a = squareArray[i, j].Data;
                    float b = squareArray[i + 1, j].Data;
                    float c = squareArray[i + 1, j + 1].Data;
                    float d = squareArray[i, j + 1].Data;

                    CreateLineListSmoothed(a + 1, b + 1, c + 1, d + 1);

                    int triangleNum = GetState((int)Math.Ceiling(a), (int)Math.Ceiling(b), (int)Math.Ceiling(c), (int)Math.Ceiling(d));

                    DrawLineSmoothed(triangleNum, scale / 15, Color.White, squareArray[i, j].Position);
                }
            }
        }

        private void CreateLineListSmoothed(float a1, float b1, float c1, float d1)
        {
            float lerpAmt = (1 - a1) / (b1 - a1);
            Vector2 a = new Vector2(MathHelper.Lerp(0, scale, MathHelper.Lerp(lerpAmt, .5f, smoothOffset)), 0); // top edge

            lerpAmt = (1 - b1) / (c1 - b1);
            Vector2 b = new Vector2(scale, MathHelper.Lerp(0, scale, MathHelper.Lerp(lerpAmt, .5f, smoothOffset)));   // right edge

            lerpAmt = (1 - d1) / (c1 - d1);
            Vector2 c = new Vector2(MathHelper.Lerp(0, scale, MathHelper.Lerp(lerpAmt, .5f, smoothOffset)), scale);  // bottom edge

            lerpAmt = (1 - a1) / (d1 - a1);
            Vector2 d = new Vector2(0, MathHelper.Lerp(0, scale, MathHelper.Lerp(lerpAmt, .5f, smoothOffset)));  // left edge

            smoothedLines = new Dictionary<string, Vector2>()
            {
                { "a", a },
                { "b", b },
                { "c", c },
                { "d", d },
            };
        }

        private void DrawLineSmoothed(float num, float width, Color color, Vector2 position)
        {
            switch (num)
            {
                case 1:
                case 14:
                    ShapeBatch.Line(smoothedLines["c"] + position, smoothedLines["d"] + position, width, color);
                    break;

                case 2:
                case 13:
                    ShapeBatch.Line(smoothedLines["b"] + position, smoothedLines["c"] + position, width, color);
                    break;

                case 3:
                case 12:
                    ShapeBatch.Line(smoothedLines["b"] + position, smoothedLines["d"] + position, width, color);
                    break;

                case 4:
                case 11:
                    ShapeBatch.Line(smoothedLines["a"] + position, smoothedLines["b"] + position, width, color);
                    break;

                case 5:
                    ShapeBatch.Line(smoothedLines["a"] + position, smoothedLines["d"] + position, width, color);
                    ShapeBatch.Line(smoothedLines["b"] + position, smoothedLines["c"] + position, width, color);
                    break;

                case 6:
                case 9:
                    ShapeBatch.Line(smoothedLines["a"] + position, smoothedLines["c"] + position, width, color);
                    break;

                case 7:
                case 8:
                    ShapeBatch.Line(smoothedLines["a"] + position, smoothedLines["d"] + position, width, color);
                    break;

                case 10:
                    ShapeBatch.Line(smoothedLines["a"] + position, smoothedLines["b"] + position, width, color);
                    ShapeBatch.Line(smoothedLines["c"] + position, smoothedLines["d"] + position, width, color);
                    break;
            }
        }

        #endregion
    }
}
