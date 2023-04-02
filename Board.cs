using ChessEngine.Pieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    class Board
    {
        public List<Piece> Pieces { get; set; }
        public bool IsBlackMove { get; set; }
        public bool GameIsOver { get; private set; } = false;
        public int MoveCount { get; set; } = 0;
        private int _50MoveCounter = 0;
        private List<Move> _allMoves;
        private List<string> _oldBoards = new List<string>();
        public bool SaveOldBoads { get; set; } = false;
        public List<Move> AllMoves { get {
                if (_allMoves is null)
                    _allMoves = GetAllMoves();
                return _allMoves;
            } 
        }
        public Board()
        {

        }
        public Board(string startPosistion)
        {
            SetPieces(startPosistion);
        }
        public void MovePiece(Move move)
        {
            if (GameIsOver)
                throw new GameOverException($"Game is already done, {GetWinner()} won!");

            if (SaveOldBoads)
                _oldBoards.Add(GetFen());

            var pieceToMove = Pieces.Find(p => p.GetPosition() == move.SourceSquare);
            var targetPiece = Pieces.Find(p => p.GetPosition() == move.TargetSquare);
            var originalPosistion = pieceToMove.GetPosition();
            pieceToMove.Move(move);

            // Special pawn rules
            if (pieceToMove.GetType().Name.Equals(nameof(Pawn)))
            {
                // En Passant
                if (targetPiece is null)
                {
                    Pieces.Remove(Pieces.Find(p => p.GetPosition() == pieceToMove.GetPosition() + (pieceToMove.IsBlack ? new Vector2(0, -1) : new Vector2(0, 1))));
                }

                // White promotion
                if (pieceToMove.Y == 1)
                {
                    Pieces.Remove(pieceToMove);
                    SetPieces($"{pieceToMove.X - 1}Q");
                }

                // Black promotion
                if (pieceToMove.Y == 8)
                {
                    Pieces.Remove(pieceToMove);
                    SetPieces($"///////{pieceToMove.X - 1}q");
                }

                _50MoveCounter++;
            }

            // Castle
            if (pieceToMove.GetType().Name.Equals(nameof(King)))
            {
                // Right
                if (pieceToMove.X - originalPosistion.X == 2)
                {
                    Pieces.Find(p => p.GetPosition() == new Vector2(8, pieceToMove.Y) && p.GetType().Name.Equals(nameof(Rook))).Move(6, pieceToMove.Y);
                }
                // Left
                if (pieceToMove.X - originalPosistion.X == -2)
                {
                    Pieces.Find(p => p.GetPosition() == new Vector2(1, pieceToMove.Y) && p.GetType().Name.Equals(nameof(Rook))).Move(4, pieceToMove.Y);
                }
            }

            pieceToMove = null;

            IsBlackMove = !IsBlackMove;

            if (!(targetPiece is null))
            {
                if (targetPiece.GetType().Name.Equals(nameof(King)))
                    GameIsOver = true;
                Pieces.Remove(targetPiece);
                _50MoveCounter = 0;
            }

            MoveCount++;
            _50MoveCounter++;

            if (_50MoveCounter > 50)
                GameIsOver = true;

            _allMoves = null;
            // Stalemate
            if (AllMoves.Count < 1)
                GameIsOver = true;
        }

        private List<Move> GetAllMoves()
        {
            var allMoves = new List<Move>();
            foreach (var piece in Pieces)
            {
                if (piece.IsBlack != IsBlackMove)
                    continue;

                var pieceMoves = piece.GetPossibleMoves(Pieces);
                foreach (var move in pieceMoves)
                {
                    allMoves.Add(new Move(piece.GetPosition(), move));
                }
            }

            return allMoves;
        }

        public string GetFen()
        {
            string fen = "";
            for (int i = 1; i <= 8; i++)
            {
                int blanks = 0;
                for (int j = 1; j <=8; j++)
                {
                    var current = Pieces.Find(p => p.GetPosition() == new Vector2(j, i));
                    if (current is null)
                    {
                        blanks++;
                        continue;
                    }

                    if (blanks > 0)
                        fen += blanks.ToString();

                    switch (current.GetType().Name)
                    {
                        case nameof(Pawn):
                            if (current.IsBlack)
                                fen += "p";
                            else
                                fen += "P";
                            break;
                        case nameof(Rook):
                            if (current.IsBlack)
                                fen += "r";
                            else
                                fen += "R";
                            break;
                        case nameof(Knight):
                            if (current.IsBlack)
                                fen += "n";
                            else
                                fen += "N";
                            break;
                        case nameof(Bishop):
                            if (current.IsBlack)
                                fen += "b";
                            else
                                fen += "B";
                            break;
                        case nameof(Queen):
                            if (current.IsBlack)
                                fen += "q";
                            else
                                fen += "Q";
                            break;
                        case nameof(King):
                            if (current.IsBlack)
                                fen += "k";
                            else
                                fen += "K";
                            break;
                        default:
                            break;
                    }

                    blanks = 0;
                }
                if (blanks > 0)
                    fen += (blanks).ToString();
                if(i < 8)
                    fen += "/";
            }
            return fen;
        }

        public void SetPieces(string posistion)
        {
            if (Pieces is null) Pieces = new List<Pieces.Piece>();

            int x = 1;
            int y = 1;
            foreach (var c in posistion)
            {
                switch (c)
                {
                    case ('p'):
                        Pieces.Add(new Pieces.Pawn(true, x, y));
                        break;
                    case ('P'):
                        Pieces.Add(new Pieces.Pawn(false, x, y));
                        break;
                    case ('r'):
                        Pieces.Add(new Pieces.Rook(true, x, y));
                        break;
                    case ('R'):
                        Pieces.Add(new Pieces.Rook(false, x, y));
                        break;
                    case ('n'):
                        Pieces.Add(new Pieces.Knight(true, x, y));
                        break;
                    case ('N'):
                        Pieces.Add(new Pieces.Knight(false, x, y));
                        break;
                    case ('b'):
                        Pieces.Add(new Pieces.Bishop(true, x, y));
                        break;
                    case ('B'):
                        Pieces.Add(new Pieces.Bishop(false, x, y));
                        break;
                    case ('q'):
                        Pieces.Add(new Pieces.Queen(true, x, y));
                        break;
                    case ('Q'):
                        Pieces.Add(new Pieces.Queen(false, x, y));
                        break;
                    case ('k'):
                        Pieces.Add(new Pieces.King(true, x, y));
                        break;
                    case ('K'):
                        Pieces.Add(new Pieces.King(false, x, y));
                        break;
                    case ('/'):
                        x = 0;
                        y++;
                        break;
                    default:
                        x += int.Parse(c.ToString()) - 1;
                        break;
                }
                x++;
            }
        }
        public Board Copy()
        {
            return new Board
            {
                Pieces = this.Pieces,
                IsBlackMove = this.IsBlackMove
            };
        }

        public string GetWinner()
        {
            if (!GameIsOver)
                return "";

            var Kings = Pieces.Where(p => p.GetType().Name.Equals(nameof(King)));

            var whiteKingAlive = Kings.Any(k => !k.IsBlack);
            var blackKingAlive = Kings.Any(k => k.IsBlack);

            if (whiteKingAlive && !blackKingAlive)
                return "White";
            if (!whiteKingAlive && blackKingAlive)
                return "Black";

            return "Neither";
        }
    }
}
