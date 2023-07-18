using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PathFinding
{
    internal class Tile
    {
        private Rectangle position;
        private Texture2D asset;
        private bool isWall;
        private int number;
        private bool isPermenant;

        public int x;
        public int y;

        private SpriteFont font;

        public Rectangle Position
        {
            get { return position; }
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public bool IsPermenant
        {
            get { return isPermenant; }
            set { isPermenant = value; }
        }

        public bool IsWall
        {
            get { return isWall; }
        }

        public Tile (Rectangle position, Texture2D asset, bool isWall, int x, int y, SpriteFont font)
        {
            this.position = position;
            this.asset = asset;
            this.isWall = isWall;
            this.number = int.MaxValue;
            this.isPermenant = false;
            this.x = x;
            this.y = y;

            this.font = font;
        }

        public void DrawTile(SpriteBatch sb)
        {
            Color color = Color.Green;

            //switch(number)
            //{
            //    case 1:
            //        color = Color.White;
            //        break;

            //    case 2:
            //        color = Color.LightYellow;
            //        break;

            //    case 3:
            //        color = Color.Yellow;
            //        break;

            //    case 4:
            //        color = Color.Orange;
            //        break;

            //    case 5:
            //        color = Color.OrangeRed;
            //        break;

            //    case 6:
            //        color = Color.Red;
            //        break;

            //    case 7:
            //        color = Color.MediumVioletRed;
            //        break;

            //    case 8:
            //        color = Color.Violet;
            //        break;

            //    case 9:
            //        color = Color.DarkViolet;
            //        break;

            //    case 10:
            //        color = Color.Navy;
            //        break;

            //    case 11:
            //        color = Color.Blue;
            //        break;

            //    default:
            //        color = Color.Black;
            //        break;
            //}

            if (this.isWall)
            {
                color = Color.Firebrick;
            }

            else
            {
                color = new Color(color.R - (number * 5), color.G - number * 5, color.B - (number * 10), color.A);
            }
            
            sb.Draw(asset, position, color);
           
            if (!this.isWall && number < 30)
            {
                sb.DrawString(font, number.ToString(), new Vector2(position.X + position.Width / 3, position.Y + position.Height / 3), Color.Black);
            }
        }
    }
}
