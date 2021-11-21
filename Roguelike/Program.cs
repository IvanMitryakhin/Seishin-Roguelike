using System;

namespace Roguelike
{
    class Program
    {
        private const int ScreenWidth = 75;
        private const int ScreenHeight = 40;
        private const int mapWidth = 60;
        private const int mapHeight = 60;

        static void Main()
        {
            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            Map map = new Map(mapWidth, mapHeight);

            while (map.IsGameActive)
            {
                map.Refresh();
                Console.SetCursorPosition(0, 39);
                Console.Write("Key input > ");
                map.ExecuteCommand(Console.ReadKey());
            }
        }
    }

}
