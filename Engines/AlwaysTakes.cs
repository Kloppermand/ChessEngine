﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChessEngine.Engines
{
    class AlwaysTakes : IPlayer
    {
        private bool _playerIsBlack;
        public Move GetMove(Board board)
        {
            _playerIsBlack = board.IsBlackMove;
            Move bestMove = board.AllMoves[0];
            int bestValue = -2000;

            foreach (var move in board.AllMoves)
            {
                Board tmpBoard = new Board(board);
                tmpBoard.MovePiece(move);
                int value = GetBoardValue(tmpBoard);
                if(value > bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }
            }

            return bestMove;
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
