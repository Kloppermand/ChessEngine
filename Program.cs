using System;

namespace ChessEngine
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Chess())
                game.Run();
        }
    }
}
