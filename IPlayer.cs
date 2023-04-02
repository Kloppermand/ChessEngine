using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChessEngine
{
    interface IPlayer
    {
        public AssetHelper.Sprites PieceSpriteFolderName { get; set; }
        public AssetHelper.Sounds SoundsFolderName { get; set; }
        public Move GetMove(Board board);
    }
}
