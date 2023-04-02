using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessEngine.Engines
{
    class HyperAgression : IPlayer
    {
        public string PieceSpriteFolderName { get; set; } = "AgressiveDerp";
        private bool _playerIsBlack;
        private Random rng = new Random();
        public Move GetMove(Board board)
        {
            _playerIsBlack = board.IsBlackMove;
            List<Move> bestMoves = new List<Move>();
            double bestValue = -2000;

            foreach (var move in board.AllMoves)
            {
                Board tmpBoard = new Board(board);
                bool isPawn = tmpBoard.Pieces.Find(p => p.GetPosition() == move.SourceSquare).GetType().Name.Equals(nameof(Pieces.Pawn));
                tmpBoard.MovePiece(move);
                double value = (double)GetBoardValue(tmpBoard) + (isPawn ? 0.5 : 0);
                if(value > bestValue)
                {
                    bestValue = value;
                    bestMoves = new List<Move>();
                }
                if(value == bestValue)
                {
                    bestMoves.Add(move);
                }

            }

            return bestMoves[rng.Next(0,bestMoves.Count-1)];
        }

        private int GetBoardValue(Board board)
        {
            int value = 0;
            foreach (var piece in board.Pieces)
            {
                bool isSameColor = piece.IsBlack == _playerIsBlack;
                value += piece.Value * (isSameColor ? 1 : -1);
            }

            return value;
        }
    }
}
