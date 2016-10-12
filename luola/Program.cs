using System;

namespace luola
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            LuolaGame luolaGame = new LuolaGame();
            luolaGame.Run();
            luolaGame.Dispose();
        }
    }
}
