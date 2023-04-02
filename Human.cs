using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessEngine
{
    class Human : IPlayer
    {
        public string PieceSpriteFolderName { get; set; } = "Normal";

        public Move GetMove(Board board)
        {
            throw new NotImplementedException();
        }

    }
}
