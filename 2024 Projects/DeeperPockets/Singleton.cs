﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets
{
    public abstract class Singleton<T>
    where T : Singleton<T>, new()
    {
        private static T _instance = new T();
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
