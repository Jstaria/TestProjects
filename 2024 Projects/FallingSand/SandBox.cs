using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FallingSand
{
    public enum BrushState
    {
        Sand,
        Barrier,
        Dupe,
        Erase
    }

    internal class SandBox
    {
        private SandParticle[,] sandBox;

        private int width, height, scale;

        private Random random = new Random();

        private int HueValue = 0;
        private int DupeHue = 0;

        private int strokeSize;
        private int rightStrokeSize;

        private BrushState currentBrush = BrushState.Sand;
        private MouseState prevMouse;

        public BrushState CurrentState { get { return currentBrush; } }

        public SandBox(int width, int height, int screenHeight, int strokeSize, int rightStrokeSize)
        {
            this.width = width;
            this.height = height;
            this.scale = screenHeight / height;
            this.sandBox = new SandParticle[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    sandBox[i, j] = new SandParticle(Color.White, 0);
                }
            }

            sandBox[width / 2, 0] = new SandParticle(Color.White, 1);

            this.strokeSize = strokeSize;
            this.rightStrokeSize = rightStrokeSize;
        }

        public void Update()
        {
            SandParticle[,] newSandBox = new SandParticle[width, height];

            DupeHue = (DupeHue + 1) % 360;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    newSandBox[i, j] = new SandParticle(sandBox[i, j].Color, 0);
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (sandBox[i, j].State == 0)
                        continue;
                    else if (sandBox[i, j].State == 2)
                    {
                        newSandBox[i, j] = sandBox[i, j];
                        continue;
                    }
                    else if (sandBox[i, j].State == 3)
                    {
                        newSandBox[i, j] = sandBox[i, j];

                        if (IsInArray(i, j + 1, sandBox))
                        {
                            newSandBox[i, j + 1].Color = getRGB();
                            newSandBox[i, j + 1].State = 1;
                        }

                        continue;
                    }

                    int RandomDir = random.Next(2);

                    if (!IsInArray(i, j + 1, sandBox) || sandBox[i, j + 1].State > 0)
                    {
                        newSandBox[i, j].State = 1;

                        if (RandomDir == 1 && IsInArray(i - 1, j + 1, sandBox) && sandBox[i, j + 1].State == 1 && sandBox[i - 1, j + 1].State == 0 && newSandBox[i - 1, j + 1].State == 0)
                        {
                            newSandBox[i - 1, j + 1] = sandBox[i, j];
                            newSandBox[i, j].State = 0;
                            newSandBox[i - 1, j + 1].Color = sandBox[i, j].Color;
                            newSandBox[i - 1, j + 1].State = 1;
                        }

                        if (RandomDir == 0 && IsInArray(i + 1, j + 1, sandBox) && sandBox[i, j + 1].State == 1 && sandBox[i + 1, j + 1].State == 0 && newSandBox[i + 1, j + 1].State == 0)
                        {
                            newSandBox[i + 1, j + 1] = sandBox[i, j];
                            newSandBox[i, j].State = 0;
                            newSandBox[i + 1, j + 1].Color = sandBox[i, j].Color;
                            newSandBox[i + 1, j + 1].State = 1;
                        }


                    }
                    else if (newSandBox[i, j + 1].State == 0 && sandBox[i, j + 1].State == 0)
                    {
                        newSandBox[i, j + 1] = sandBox[i, j];
                        newSandBox[i, j + 1].Color = sandBox[i, j].Color;
                        newSandBox[i, j].State = 0;
                        newSandBox[i, j + 1].State = 1;

                    }
                }
            }

            sandBox = newSandBox;

            CheckMouse();
        }

        public Color getRGB(int H, double S, double V)
        {
            double dC = (V * S);
            double Hd = ((double)H) / 60;
            double dX = (dC * (1 - Math.Abs((Hd % 2) - 1)));//dC * (1 - ((Hd + 1) % 2));

            int C = (int)(dC * 255);
            int X = (int)(dX * 255);

            //Console.WriteLine("H:" + H + " S:" + S + " V:" + V + ", C: " + C + " X:" + X + " Hd:" + Hd);

            if (Hd < 1)
            {
                return new Color(C, X, 0);
            }
            else if (Hd < 2)
            {
                return new Color(X, C, 0);
            }
            else if (Hd < 3)
            {
                return new Color(0, C, X);
            }
            else if (Hd < 4)
            {
                return new Color(0, X, C);
            }
            else if (Hd < 5)
            {
                return new Color(X, 0, C);
            }
            else if (Hd < 6)
            {
                return new Color(C, 0, X);
            }
            return new Color(0, 0, 0);
        }

        public Color getRGB()
        {
            return getRGB(DupeHue, .95f, 1f);
        }

        private void CheckMouse()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            if (mouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released)
            {
                currentBrush = (BrushState)(((int)currentBrush + 1) % 4);
            }

            if (mouse.X > 0 && mouse.Y > 0 && mouse.X < width * scale && mouse.Y < height * scale && mouse.LeftButton == ButtonState.Pressed)
            {
                int sandX = (int)MathF.Floor(mouse.X / scale);
                int sandY = (int)MathF.Floor(mouse.Y / scale);

                switch (currentBrush)
                {
                    case BrushState.Sand:

                        if (strokeSize > 1)
                        {
                            for (int i = -strokeSize / 2; i < strokeSize / 2; i++)
                            {
                                for (int j = -strokeSize / 2; j < strokeSize / 2; j++)
                                {
                                    if (!IsInArray(sandX + i, sandY + j, sandBox) || random.NextDouble() < .75) continue;
                                    sandBox[sandX + i, sandY + j] = new SandParticle(getRGB((HueValue + random.Next(-10, 10)) % 360, .95f, 1f), 1);
                                }
                            }
                        }

                        else
                        {
                            sandBox[sandX, sandY] = new SandParticle(getRGB((HueValue + random.Next(-10, 10)) % 360, .95f, 1f), 1);
                        }


                        HueValue = (HueValue + 1) % 360;

                        break;

                    case BrushState.Barrier:

                        if (rightStrokeSize > 1)
                        {
                            for (int i = -rightStrokeSize / 2; i < rightStrokeSize / 2; i++)
                            {
                                for (int j = -rightStrokeSize / 2; j < rightStrokeSize / 2; j++)
                                {
                                    if (!IsInArray(sandX + i, sandY + j, sandBox)) continue;
                                    sandBox[sandX + i, sandY + j] = new SandParticle(Color.DarkGray, 2);
                                }
                            }
                        }

                        else
                        {
                            sandBox[sandX, sandY] = new SandParticle(Color.DarkGray, 2);
                        }

                        break;

                    case BrushState.Dupe:

                        sandBox[sandX, sandY] = new SandParticle(Color.Brown, 3);

                        break;

                    case BrushState.Erase:

                        if (strokeSize > 1)
                        {
                            for (int i = -strokeSize / 3; i < strokeSize / 3; i++)
                            {
                                for (int j = -strokeSize / 3; j < strokeSize / 3; j++)
                                {
                                    if (!IsInArray(sandX + i, sandY + j, sandBox)) continue;
                                    sandBox[sandX + i, sandY + j].State = 0;
                                }
                            }
                        }

                        else
                        {
                            sandBox[sandX, sandY].State = 0;
                        }

                        break;
                }
            }
            prevMouse = mouse;
        }

        private bool IsInArray(int x, int y, SandParticle[,] cellArray)
        {
            return
                x < cellArray.GetLength(0) && y < cellArray.GetLength(1) &&
                x >= 0 && y >= 0;
        }

        public void Draw()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    SandParticle s = sandBox[i, j];
                    if (s == null) continue;

                    if (s.State == 0) continue;
                    ShapeBatch.Box(new Rectangle(new Point(i * scale, j * scale), new Point(scale, scale)), s.Color);
                }
            }
        }
    }
}
