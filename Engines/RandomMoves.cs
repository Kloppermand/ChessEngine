using System;
using System.Collections.Generic;
using System.Text;

namespace ChessEngine.Engines
{
    class RandomMoves : IPlayer
    {
        public AssetHelper.Sprites PieceSpriteFolderName { get; set; } = AssetHelper.Sprites.Derp;
        public AssetHelper.Sounds SoundsFolderName { get; set; } = AssetHelper.Sounds.Derp;
        private Random rng = new Random();

        public Move GetMove(Board board)
        {
            return board.AllMoves[rng.Next(0, board.AllMoves.Count - 1)];
        }
    }
}
