using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ChessEngine.Pieces
{
    abstract class Piece
    {
        public readonly Texture2D Sprite;
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsBlack { get; }
        public bool HasMoved { get; private set; } = false;
        public int Value { get; set; }


        public Piece(bool isBlack, int x, int y)
        {
            X = x;
            Y = y;
            IsBlack = isBlack;
        }
        virtual public void Move(int x, int y)
        {
            X = x;
            Y = y;
            HasMoved = true;
        }
        virtual public void Move(Move move)
        {
            X = (int)move.TargetSquare.X;
            Y = (int)move.TargetSquare.Y;
            HasMoved = true;
        }

        public Vector2 GetPosition()
        {
            return new Vector2(X, Y);
        }

        internal void AddDirectionalMoves(List<Vector2> moves, List<Piece> pieces, Vector2 offset, bool ignoreKing = false)
        {
            Vector2 potentialMove = GetPosition()+offset;
            while (potentialMove.X >= 1 && potentialMove.X <= 8 && potentialMove.Y >= 1 && potentialMove.Y <= 8)
            {
                var target = pieces.Find(p => p.GetPosition() == potentialMove);
                if (target is null || (target.GetType().Name == nameof(King) && target.IsBlack != IsBlack && ignoreKing))
                {
                    moves.Add(potentialMove);
                }
                else
                {
                    if (target.IsBlack != IsBlack || ignoreKing)
                        moves.Add(potentialMove);
                    return;
                }

                potentialMove += offset;
            } 
        }

        abstract internal List<Vector2> GetPossibleMoves(List<Piece> pieces, bool ignoreKing = false);
    }
}
