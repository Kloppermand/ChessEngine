using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessEngine
{
    class Human : IPlayer
    {
        public AssetHelper.Sprites PieceSpriteFolderName { get; set; } = AssetHelper.Sprites.Normal;
        public AssetHelper.Sounds SoundsFolderName { get; set; } = AssetHelper.Sounds.Normal;

        public Move GetMove(Board board)
        {
            throw new NotImplementedException();
        }

    }
}
