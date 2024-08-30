using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets
{
    internal class Global : Singleton<Global>
    {
        public Vector2 WorldOffset; // Shift for everything minus player
        public Vector2 CameraOffset; // Shift for everything plus player
    }
}
