using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChessEngine
{
    interface IPlayer
    {
        public Move GetMove(Board board);
    }
}
