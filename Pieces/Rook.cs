using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ChessEngine.Pieces
{
    class Rook : Piece
    {
        public Rook(bool isBlack, int x, int y) : base(isBlack, x, y)
        {
            Value = 5;
        }
        private Rook(bool isBlack, int x, int y, bool hasMoved) : base(isBlack, x, y, hasMoved)
        {
            Value = 5;
        }
        public override Piece Copy()
        {
            return new Rook(IsBlack, X, Y, HasMoved);
        }

        internal override List<Vector2> GetPossibleMoves(List<Piece> pieces, bool ignoreKing = false)
        {
            var list = new List<Vector2>();

            AddDirectionalMoves(list, pieces, new Vector2(1, 0), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(-1, 0), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(0, 1), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(0, -1), ignoreKing);

            return list;

        }
    }
}