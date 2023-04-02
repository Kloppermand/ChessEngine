using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChessEngine
{
    interface IPlayer
    {
        public string PieceSpriteFolderName { get; set; }
        public Move GetMove(Board board);
    }
}
