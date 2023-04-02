using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine.Pieces
{
    class Queen : Piece
    {
        public Queen(bool isBlack, int x, int y ) : base(isBlack, x, y)
        {
            Value = 9;
        }

        internal override List<Vector2> GetPossibleMoves(List<Piece> pieces, bool ignoreKing = false)
        {
            var list = new List<Vector2>();

            // Orthogonal
            AddDirectionalMoves(list, pieces, new Vector2(1, 0), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(-1, 0), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(0, 1), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(0, -1), ignoreKing);

            // Diagonal
            AddDirectionalMoves(list, pieces, new Vector2(1, 1), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(1, -1), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(-1, 1), ignoreKing);
            AddDirectionalMoves(list, pieces, new Vector2(-1, -1), ignoreKing);

            return list.Where(p => p.X < 9 && p.X > 0 && p.Y < 9 && p.Y > 0).ToList();

        }
    }
}
