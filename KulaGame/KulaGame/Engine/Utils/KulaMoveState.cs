using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KulaGame.Engine.Utils
{
    public enum KulaMoveState
    {
        Stop=1, //don't move
        Forward=2, // When the next move has collision with one block
        Up=3, // When the next move has collision with 2 blocks
        Down=4, // When the next move has no collision with blocks - to figure out
    }
}
