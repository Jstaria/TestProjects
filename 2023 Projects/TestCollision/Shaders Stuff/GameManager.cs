using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


// Manages [what I assume to be the shaders]

namespace TestCollision
{
    internal class GameManager
    {
        private readonly Sprite sprite;
        private readonly Vector2 pos00;

        public GameManager()
        {
            sprite = new Sprite(Globals.Content.Load<Texture2D>("happyben"));
            pos00 = new Vector2(Game1.width / 2, Game1.height / 2);
        }

        public void Update()
        {

        }

        public void Draw()
        {
            Globals.SpriteBatch.Begin();

            sprite.Draw(pos00);

            Globals.SpriteBatch.End();
        }
    }
}
