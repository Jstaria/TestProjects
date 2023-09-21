using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PuzzleGolf_TeamB
{
    /// <summary>
    /// Different types of tile obsticles like water or spikes
    /// </summary>
    internal class Tiles : Wall
    {
        private Color tint;
        
        public Tiles(int x, int y, int screenH, Texture2D texture, Color tint)
           : base( false, false, null, x, y, screenH,texture,tint)
        {
            this.tint = tint;
        }
       

        public void TWall(SpriteBatch sb)
        {
            sb.Draw(asset, position, tint);
        }
    }
}
