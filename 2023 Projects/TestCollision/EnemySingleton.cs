using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCollision
{
    internal class EnemySingleton
    {
        public static EnemySingleton instance = new EnemySingleton();

        private int count;

        public static int Count
        {
            get { return instance.count; }
            set { instance.count = value; }
        }

        private EnemySingleton()
        {
            this.count = 0;
        }
    }
}
