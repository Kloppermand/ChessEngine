using Microsoft.Xna.Framework;

namespace ChessEngine
{
    public class Move
    {
        public Vector2 SourceSquare { get; set; }
        public Vector2 TargetSquare { get; set; }
        public Move(Vector2 source, Vector2 target)
        {
            SourceSquare = source;
            TargetSquare = target;
        }
    }
}