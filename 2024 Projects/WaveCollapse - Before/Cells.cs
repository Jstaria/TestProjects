using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace WaveCollapse
{
    internal static class Cells
    {
        private static Dictionary<string, Cell> cellDict = new Dictionary<string, Cell>();
        
        public static Dictionary<string, Cell> CellDict { get { return cellDict; } }

        public static void GenerateCells(Dictionary<string,Texture2D> assets)
        {
            List<string> data = FileIO.ReadFrom("tile");

            for (int i = 0; i < assets.Count; i++)
            {
                cellDict.Add((i + 1).ToString(), new Cell(assets[(i+1).ToString()], Vector2.Zero));

                
                string[] line = data[i].Split(",");

                List<string> left = new List<string>();
                List<string> right = new List<string>();
                List<string> up = new List<string>();
                List<string> down = new List<string>();

                List<string> current = new List<string>();

                for (int j = 0; j < line.Length; j++)
                {
                    switch (line[j])
                    {
                        case "Left":
                            current = left;
                            break;
                        case "Right":
                            current = right;
                            break;
                        case "Up":
                            current = up;
                            break;
                        case "Down":
                            current = down;
                            break;

                        default:
                            current.Add(line[j]);

                            break;
                    }
                }

                cellDict[(i + 1).ToString()].SetNeighbors(left,right,up,down);
            }
        }
    }
}
