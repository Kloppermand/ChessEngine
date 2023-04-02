using System;
using System.Collections.Generic;
using System.Text;

namespace ChessEngine
{
    interface IPlayer
    {
        public Move GetMove(Board board);
    }
}
