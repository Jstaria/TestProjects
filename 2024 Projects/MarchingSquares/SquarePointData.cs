using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarchingSquares
{
    internal class SquarePointData
    {
        private Vector2 position;
        private float data;

        public Vector2 Position { get { return position; }}
        public float Data { get { return data; } set { data = value; } }

        public SquarePointData(Vector2 position)
        {
            this.position = position;
        }
    }
}
