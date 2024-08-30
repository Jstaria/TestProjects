using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets
{
    internal interface ICollision
    {
        public delegate void OnCollision();
        public event OnCollision OnCollisionEnter;

        public HitBox HitBox { get; }

        public void OnCollisionDetection();
    }
}
