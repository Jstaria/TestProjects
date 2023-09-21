using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Unused in the final build, could be added later on its own

namespace PuzzleGolf_TeamB
{
    /// <summary>
    /// System for keys and gates to be implemented into the game.
    /// </summary>
    internal class Gate : Wall
    {
        //private int keyAmount;
        private Color tint;
        private Key key;

        public Key Key
        {
            get { return key; }
        }

        public Gate(Key key, int x, int y, int screenH, Texture2D texture, Color tint)
           : base(false, false, null, x, y, screenH, texture, tint)
        {
            this.tint = tint;
        }


        //collects keys when ball intersects key location
        public void Update()
        {
            if (key.IsCollected)
            {
                // Send position off to nowhere's ville
                this.position = new Rectangle(-1000, -1000, 0, 0);
            }
        }
    }
}
