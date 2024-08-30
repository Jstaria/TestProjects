using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets
{
    internal interface IObject
    {
        public Vector2 Position { get;}
        public Vector2 DrawPosition { get; }
        public Vector2 Size { get;}   
        public Texture2D Texture { get; }
    }
}
