using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carousel_Menu
{
    public class CarouselTexture
    {
        public Texture2D Texture { get; }
        public Matrix World { get; set; }

        public CarouselTexture(Texture2D texture, Matrix world)
        {
            Texture = texture;
            World = world;
        }
    }
}
