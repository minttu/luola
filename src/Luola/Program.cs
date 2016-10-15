using System;

namespace Luola
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var luolaGame = new LuolaGame();
            luolaGame.Run();
            luolaGame.Dispose();
        }
    }
}