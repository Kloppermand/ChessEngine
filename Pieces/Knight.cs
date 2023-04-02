using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ChessEngine.Pieces
{
    class Knight : Piece
    {
        public Knight(bool isBlack, int x, int y ) : base(isBlack, x, y)
        {
            Value = 3;
        }

        internal override List<Vector2> GetPossibleMoves(List<Piece> pieces, bool ignoreKing = false)
        {
            var list = new List<Vector2>();
            list.Add(new Vector2(X + 1, Y + 2));
            list.Add(new Vector2(X + 2, Y + 1));
            list.Add(new Vector2(X - 1, Y + 2));
            list.Add(new Vector2(X - 2, Y + 1));
            list.Add(new Vector2(X + 1, Y - 2));
            list.Add(new Vector2(X + 2, Y - 1));
            list.Add(new Vector2(X - 1, Y - 2));
            list.Add(new Vector2(X - 2, Y - 1));

            return list.Where(m => m.X < 9 && m.X > 0 && m.Y < 9 && m.Y > 0) // Filter out of board
                .Where(m => !pieces.Any(p => new Vector2(p.X, p.Y) == m && p.IsBlack == IsBlack)) // Filter friendly pieces
                .ToList();
        }
    }
}
