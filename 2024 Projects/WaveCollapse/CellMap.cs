using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WaveCollapse
{
    internal class CellMap
    {   
        // Cell Map values
        private Cell[,] cellArray;
        private Vector2 position;

        private int width;
        private int height;

        private int scale;


        public Cell[,] CellArray { get { return cellArray; } }
        public Vector2 Position { get { return position; } }

        public CellMap(Vector2 position, int width, int height, int scale)
        {
            this.cellArray = new Cell[width, height];
            this.position = position;
            this.width = width;
            this.height = height;
            this.scale = scale;
        }


    }
}
