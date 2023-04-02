using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ChessEngine
{
    public class Chess : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Dictionary<string, Song> _sounds;
        private Dictionary<string, Texture2D> _sprites;
        private Texture2D _whiteSquare;
        private Texture2D _blackSquare;
        private Texture2D _selected;
        private SpriteFont _font;
        private Pieces.Piece _grabbed;
        private int _lastPickup;
        private int _lastMove;
        private MouseState _mouse;
        private Board _board;
        private IPlayer _player1;
        private IPlayer _player2;
        bool _hasRunWinSound;
        Thread _thread;

        public Chess()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.ApplyChanges();

            _whiteSquare = new Texture2D(GraphicsDevice, 100, 100);
            _blackSquare = new Texture2D(GraphicsDevice, 100, 100);
            _selected = new Texture2D(GraphicsDevice, 100, 100);

            Color[] whiteData = new Color[100 * 100];
            Color[] blackData = new Color[100 * 100];
            Color[] selectedData = new Color[100 * 100];
            for (int i = 0; i < whiteData.Length; i++)
            {
                whiteData[i] = Color.Beige;
                blackData[i] = Color.Sienna;
                var x = (i % 100) - 50;
                var y = (i / 100) - 50;
                if (x * x + y * y < 10 * 10)
                    selectedData[i] = Color.Red;
            }

            _whiteSquare.SetData(whiteData);
            _blackSquare.SetData(blackData);
            _selected.SetData(selectedData);

            _font = Content.Load<SpriteFont>("Font");

            _player1 = new Engines.HyperAgression();
            _player2 = new Human();


            _board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w"); // Start position
            _board.SaveOldBoads = false;
            _hasRunWinSound = false;

            _lastPickup = 100;
            _lastMove = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var player1SpriteFolder = _player1.PieceSpriteFolderName.ToString() ?? "Derp";
            var player2SpriteFolder = _player2.PieceSpriteFolderName.ToString() ?? "Derp";

            _sprites = new Dictionary<string, Texture2D>();
            _sprites.Add("blackPawnSprite", Content.Load<Texture2D>($"SpritePngs/{player2SpriteFolder}/BlackPawn"));
            _sprites.Add("whitePawnSprite", Content.Load<Texture2D>($"SpritePngs/{player1SpriteFolder}/WhitePawn"));
            _sprites.Add("blackRookSprite", Content.Load<Texture2D>($"SpritePngs/{player2SpriteFolder}/BlackRook"));
            _sprites.Add("whiteRookSprite", Content.Load<Texture2D>($"SpritePngs/{player1SpriteFolder}/WhiteRook"));
            _sprites.Add("blackKnightSprite", Content.Load<Texture2D>($"SpritePngs/{player2SpriteFolder}/BlackKnight"));
            _sprites.Add("whiteKnightSprite", Content.Load<Texture2D>($"SpritePngs/{player1SpriteFolder}/WhiteKnight"));
            _sprites.Add("blackBishopSprite", Content.Load<Texture2D>($"SpritePngs/{player2SpriteFolder}/BlackBishop"));
            _sprites.Add("whiteBishopSprite", Content.Load<Texture2D>($"SpritePngs/{player1SpriteFolder}/WhiteBishop"));
            _sprites.Add("blackQueenSprite", Content.Load<Texture2D>($"SpritePngs/{player2SpriteFolder}/BlackQueen"));
            _sprites.Add("whiteQueenSprite", Content.Load<Texture2D>($"SpritePngs/{player1SpriteFolder}/WhiteQueen"));
            _sprites.Add("blackKingSprite", Content.Load<Texture2D>($"SpritePngs/{player2SpriteFolder}/BlackKing"));
            _sprites.Add("whiteKingSprite", Content.Load<Texture2D>($"SpritePngs/{player1SpriteFolder}/WhiteKing"));

            var player1SoundFolder = _player1.SoundsFolderName.ToString() ?? "Normal";
            var player2SoundFolder = _player2.SoundsFolderName.ToString() ?? "Normal";

            _sounds = new Dictionary<string, Song>();
            _sounds.Add("whiteMove", Content.Load<Song>($"Sounds/{player1SoundFolder}/Move"));
            _sounds.Add("blackMove", Content.Load<Song>($"Sounds/{player2SoundFolder}/Move"));
            _sounds.Add("whiteWin", Content.Load<Song>($"Sounds/{player1SoundFolder}/Win"));
            _sounds.Add("blackWin", Content.Load<Song>($"Sounds/{player2SoundFolder}/Win"));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                Initialize();

            _lastMove++;
            if (!_board.GameIsOver && _lastMove > 30)
            {
                _lastPickup++;

                var currentPlayer = _board.IsBlackMove ? _player2 : _player1;

                if (currentPlayer.GetType().Name.Equals(nameof(Human)))
                {
                    _mouse = Mouse.GetState();
                    if (_mouse.LeftButton == ButtonState.Pressed)
                    {
                        int clickedX = _mouse.X / 100;
                        int clickedY = _mouse.Y / 100;

                        if (_lastPickup > 10)
                        {
                            // Grab piece
                            if (_grabbed is null)
                            {
                                var tmpGrabbed = _board.Pieces.Find(p => p.X == clickedX && p.Y == clickedY);
                                if (!(tmpGrabbed is null) && tmpGrabbed.IsBlack == _board.IsBlackMove)
                                {
                                    _grabbed = tmpGrabbed;
                                    _lastPickup = 0;
                                }

                            }
                            else
                            {
                                // Place piece back
                                if (_grabbed.X == clickedX && _grabbed.Y == clickedY)
                                {
                                    _grabbed = null;
                                    _lastPickup = 0;
                                }

                                // Move piece
                                else if (_grabbed.GetPossibleMoves(_board.Pieces).Contains(new Vector2(clickedX, clickedY)))
                                {
                                    var targetPiece = _board.Pieces.Find(p => p.X == clickedX && p.Y == clickedY);

                                    if (targetPiece is null || targetPiece.IsBlack != _grabbed.IsBlack)
                                    {
                                        MediaPlayer.Play(_sounds[_board.IsBlackMove ? "blackMove" : "whiteMove"]);
                                        _board.MovePiece(new Move(_grabbed.GetPosition(), new Vector2(clickedX, clickedY)));
                                        _grabbed = null;
                                        _lastPickup = 0;
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    MediaPlayer.Play(_sounds[_board.IsBlackMove ? "blackMove" : "whiteMove"]);
                    _board.MovePiece(currentPlayer.GetMove(new Board(_board)));
                    _lastMove = 0;
                }

            }
            
            if (_board.GameIsOver && !_hasRunWinSound)
            {
                _thread = new Thread(() => MediaPlayer.Play(_sounds[!_board.IsBlackMove ? "blackWin" : "whiteWin"]));
                _thread.Start();
                _hasRunWinSound = true;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BurlyWood);
            _spriteBatch.Begin();

            // Draw board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0)
                        _spriteBatch.Draw(_whiteSquare, new Vector2(100 + i * 100, 100 + j * 100), Color.White);
                    else
                        _spriteBatch.Draw(_blackSquare, new Vector2(100 + i * 100, 100 + j * 100), Color.White);
                }
                // Write Letters
                _spriteBatch.DrawString(_font, ((char)(i + 65)).ToString(), new Vector2(i * 100 + 150 - _font.MeasureString(((char)(i + 65)).ToString()).X / 2, 920), Color.Black);
                // Write Numbers
                _spriteBatch.DrawString(_font, (i + 1).ToString(), new Vector2(70, 850 - i * 100 - _font.MeasureString((i + 1).ToString()).Y / 2), Color.Black);

                // Write move count
                _spriteBatch.DrawString(_font, $"Move: {_board.MoveCount}", new Vector2(5, 5), Color.Black);
            }

            // Draw pieces
            foreach (var piece in _board.Pieces)
            {
                if (piece != _grabbed)
                    _spriteBatch.Draw(GetSprite(piece), new Rectangle(piece.X * 100, piece.Y * 100, 100, 100), Color.White);
            }

            // Draw grabbed 
            if (!(_grabbed is null))
            {
                _spriteBatch.Draw(GetSprite(_grabbed), new Vector2(_mouse.X - 50, _mouse.Y - 50), Color.White);
                var possibleMoves = _grabbed.GetPossibleMoves(_board.Pieces);
                foreach (var move in possibleMoves)
                {
                    _spriteBatch.Draw(_selected, move * 100, Color.Red);
                }
            }

            // Draw Winner
            if (_board.GameIsOver)
            {
                var winnerString = $"{_board.GetWinner()} won!";
                _spriteBatch.DrawString(_font, winnerString, new Vector2(500 - _font.MeasureString(winnerString).X / 2, 50), Color.Black);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private Texture2D GetSprite(Pieces.Piece piece)
        {
            if (piece.IsBlack)
            {
                switch (piece.GetType().Name)
                {
                    case nameof(Pieces.Pawn):
                        return _sprites["blackPawnSprite"];
                    case nameof(Pieces.Rook):
                        return _sprites["blackRookSprite"];
                    case nameof(Pieces.Bishop):
                        return _sprites["blackBishopSprite"];
                    case nameof(Pieces.Knight):
                        return _sprites["blackKnightSprite"];
                    case nameof(Pieces.Queen):
                        return _sprites["blackQueenSprite"];
                    case nameof(Pieces.King):
                        return _sprites["blackKingSprite"];
                    default:
                        return null;
                }
            }
            else
            {
                switch (piece.GetType().Name)
                {
                    case nameof(Pieces.Pawn):
                        return _sprites["whitePawnSprite"];
                    case nameof(Pieces.Rook):
                        return _sprites["whiteRookSprite"];
                    case nameof(Pieces.Bishop):
                        return _sprites["whiteBishopSprite"];
                    case nameof(Pieces.Knight):
                        return _sprites["whiteKnightSprite"];
                    case nameof(Pieces.Queen):
                        return _sprites["whiteQueenSprite"];
                    case nameof(Pieces.King):
                        return _sprites["whiteKingSprite"];
                    default:
                        return null;
                }
            }
        }
    }
}
