using DeeperPockets.Entities.Singletons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets.Singletons
{
    internal class Global : Singleton<Global>
    {
        public Vector2 WorldOffset; // Shift for everything minus player
        public Vector2 CameraOffset; // Shift for everything plus player

        public Camera camera; // Global callback to camera
    }
}
