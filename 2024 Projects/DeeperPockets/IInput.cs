using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DeeperPockets
{
    internal interface IInput
    {
        // Mouse Input
        public abstract KeyboardState prevKBState { get;  }
        public abstract KeyboardState curKBState { get; }

        // Mouse Input
        public abstract MouseState prevMouseState { get;  }
        public abstract MouseState curMouseState { get; }

    }
}
