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
        HitBox currentWalkableArea;

        public Map() 
        {
            currentMapBox = new HitBox(new Vector2(-1920 / 2, 1080 / 2 - 200), new Vector2(1920 * 2f, 200 * 2f), Color.Yellow, 5);
            Global.Instance.currentScreenBox = currentMapBox;
            Global.Instance.currentWalkableArea = currentMapBox;
        }

        public void Update() {
            currentMapBox.Update();
        }
    }
}
