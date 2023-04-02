using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine.Pieces
{
    class Bishop : Piece
    {
        public Bishop(bool isBlack, int x, int y) : base(isBlack, x, y)
        {
            Value = 3;
        }

        public override Piece Copy()
        {
            return new Bishop(IsBlack, X, Y);
        }

        internal override List<Vector2> GetPossibleMoves(List<Piece> pieces, bool ignoreKing = false)
        {
            var list = new List<Vector2>();

            AddDirectionalMoves(list, pieces, new Vector2(1, 1), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(1, -1), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(-1, 1), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(-1, -1), ignoreKing);

            return list.Where(p => p.X < 9 && p.X > 0 && p.Y < 9 && p.Y > 0).ToList();
        }
    }
}
