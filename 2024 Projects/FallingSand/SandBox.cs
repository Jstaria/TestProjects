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
    internal class SandBox
    {
        private SandParticle[,] sandBox;

        private int width, height, scale;

        public SandBox(int width, int height, int screenHeight)
        {
            this.width = width;
            this.height = height;
            this.scale = screenHeight / height;
            this.sandBox = new SandParticle[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    sandBox[i, j] = new SandParticle();
                }
            }

            sandBox[width / 2, 0] = new SandParticle(new Point(width / 2 * scale, 0), Color.White, scale);
            sandBox[width / 2, 0].IsActive = true;
        }

        public void Update()
        {
            CheckMouse();
            
            SandParticle[,] newSandBox = sandBox;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!sandBox[i, j].IsActive || 
                        !IsInArray(i, j, sandBox) || 
                        !sandBox[i, j + 1].IsActive) 
                        continue;

                    
                    newSandBox[i, j + 1] = sandBox[i, j];
                    newSandBox[i, j] = new SandParticle();
                    newSandBox[i, j + 1].Position = new Point(i * scale, (j + 1) * scale);
                    newSandBox[i, j + 1].IsActive = true;
                }
            }

            sandBox = newSandBox;
        }

        public void CheckMouse()
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.X < 0 || mouse.Y < 0 || mouse.X > width * scale || mouse.Y > height * scale) return;

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                int sandX = (int)MathF.Floor(mouse.X / scale);
                int sandY = (int)MathF.Floor(mouse.Y / scale);

                sandBox[sandX, sandY] = new SandParticle(new Point(sandX * scale, sandY * scale), Color.White, scale);
            }
        }

        private bool IsInArray(int x, int y, SandParticle[,] cellArray)
        {
            return
                x < cellArray.GetLength(0) && y < cellArray.GetLength(1) &&
                x >= 0 && y >= 0;
        }

        public void Draw()
        {
            foreach (SandParticle s in sandBox)
            {
                if (s == null) continue;
                s.Draw();
            }
        }
    }
}
