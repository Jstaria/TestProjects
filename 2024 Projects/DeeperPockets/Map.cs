using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeeperPockets.Singletons;
using Microsoft.Xna.Framework;

namespace DeeperPockets
{
    internal class Map
    {
        HitBox currentMapBox;
        public Map() 
        {
            currentMapBox = new HitBox(new Vector2(-1920 / 3f, -1080 / 3f), new Vector2(1920 * 1.5f, 1080 * 1.5f), Color.Yellow, 5);
            Global.Instance.currentScreenBox = currentMapBox;
        }

        public void Update() {
            currentMapBox.Update();
        }
    }
}
