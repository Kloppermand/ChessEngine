using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine.Pieces
{
    class Pawn : Piece
    {
        public bool CanBeEnPassant { get; set; }
        public Pawn(bool isBlack, int x, int y) : base(isBlack, x, y)
        {
            Value = 1;
        }

        internal override List<Vector2> GetPossibleMoves(List<Piece> pieces, bool ignoreKing = false)
        {

            var list = new List<Vector2>();

            // Regular move
            var inFront = new Vector2(X, IsBlack ? Y + 1 : Y - 1);
            if (!pieces.Any(p => p.X == inFront.X && p.Y == inFront.Y))
                list.Add(inFront);

            // Take right
            var right = new Vector2(X + 1, IsBlack ? Y + 1 : Y - 1);
            if (pieces.Any(p => p.X == right.X && p.Y == right.Y && p.IsBlack != IsBlack))
                list.Add(right);
            // En Passant right
            var epRight = new Vector2(X + 1, Y);
            if (pieces.Where(p => p.X == epRight.X && p.Y == epRight.Y && p.IsBlack != IsBlack && p.GetType().Name.Equals(nameof(Pawn))).Select(p => (Pawn)p).Any(p => p.CanBeEnPassant))
                list.Add(right);

            // Take left
            var left = new Vector2(X - 1, IsBlack ? Y + 1 : Y - 1);
            if (pieces.Any(p => p.X == left.X && p.Y == left.Y && p.IsBlack != IsBlack))
                list.Add(left);
            // En Passant right
            var epLeft = new Vector2(X - 1, Y);
            if (pieces.Where(p => p.X == epLeft.X && p.Y == epLeft.Y && p.IsBlack != IsBlack && p.GetType().Name.Equals(nameof(Pawn))).Select(p => (Pawn)p).Any(p => p.CanBeEnPassant))
                list.Add(left);

            // Double move
            if (!HasMoved)
                list.Add(new Vector2(X, IsBlack ? Y + 2 : Y - 2));

            return list.Where(p => p.X < 9 && p.X > 0 && p.Y < 9 && p.Y > 0).ToList();
        }

        public override void Move(int x, int y)
        {
            CanBeEnPassant = !HasMoved;
            base.Move(x, y);
        }
    }
}
