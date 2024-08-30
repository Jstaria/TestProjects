using MarchingSquares;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets
{
    internal class Debug : Singleton<Debug>
    {
        List<IDebug> debugObjs = new List<IDebug>();

        public void AddDebugObj(IDebug debugObj)
        {
            debugObjs.Add(debugObj);
        }

        public void Draw(GraphicsDevice graphics)
        {
            ShapeBatch.Begin(graphics);

            foreach (IDebug DOs in debugObjs)
            {
                DOs.Draw();
            }

            ShapeBatch.End();
        }
    }
}
