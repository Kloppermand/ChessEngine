using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine.Pieces
{
    class King : Piece
    {
        public King(bool isBlack, int x, int y) : base(isBlack, x, y)
        {
            Value = 1000;
        }

        internal override List<Vector2> GetPossibleMoves(List<Piece> pieces, bool ignoreKing = false)
        {
            if (ignoreKing)
                return (Neighbours());

            var attackedSquares = new HashSet<Vector2>();

            foreach (var piece in pieces.Where(p => p.IsBlack != IsBlack))
            {
                foreach (var square in piece.GetPossibleMoves(pieces, true))
                {
                    attackedSquares.Add(square);
                }
            }

            var list = Neighbours().Where(p => p.X < 9 && p.X > 0 && p.Y < 9 && p.Y > 0)
                .Where(m => !attackedSquares.Contains(m))
                .ToList(); ;

            // Castle
            if (!HasMoved)
            {
                var rook1CanCastle = true;
                var rook1 = pieces.Find(p => p.GetPosition() == new Vector2(1, Y) && p.GetType().Name.Equals(nameof(Pieces.Rook)));
                if (rook1 is object && !rook1.HasMoved)
                { // Check that squares inbetween are empty.
                    for (int i = 2; i < X; i++)
                    {
                        if (pieces.Any(p => p.GetPosition() == new Vector2(i, Y)))
                            rook1CanCastle = false;
                    }
                }

                var rook2CanCastle = true;
                var rook2 = pieces.Find(p => p.GetPosition() == new Vector2(8, Y) && p.GetType().Name.Equals(nameof(Pieces.Rook)));
                if (rook2 is object && !rook2.HasMoved)
                { // Check that squares inbetween are empty.
                    for (int i = 7; i > X; i--)
                    {
                        if (pieces.Any(p => p.GetPosition() == new Vector2(i, Y)))
                            rook2CanCastle = false;
                    }
                }

                if (rook1CanCastle)
                    list.Add(new Vector2(X - 2, Y));
                if (rook2CanCastle)
                    list.Add(new Vector2(X + 2, Y));
            }

            return list;
        }

        private List<Vector2> Neighbours()
        {
            var list = new List<Vector2>();

            list.Add(new Vector2(X + 1, Y + 1));
            list.Add(new Vector2(X + 1, Y));
            list.Add(new Vector2(X + 1, Y - 1));
            list.Add(new Vector2(X, Y + 1));
            list.Add(new Vector2(X, Y - 1));
            list.Add(new Vector2(X - 1, Y + 1));
            list.Add(new Vector2(X - 1, Y));
            list.Add(new Vector2(X - 1, Y - 1));

            return list;
        }
    }
}
