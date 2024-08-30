using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets
{
    internal interface IMove
    {
        public abstract int Speed { get; set; }

        void Move();

    }
}
