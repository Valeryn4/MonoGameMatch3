using System;

namespace Match3MonoGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Match3PrototypeGame())
                game.Run();
        }
    }
}
