using System;
using System.Threading;

namespace Roguelike
{
    class Program
    {
        private const int ScreenWidth = 75;
        private const int ScreenHeight = 35;
        private const int mapWidth = 60;
        private const int mapHeight = 60;

        static void Main()
        {
            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            Greeting hello = new Greeting();
            while(true)
            {
                int level = 1;
                PlayTheGame(hello.nameOfPlayer, level);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("В этот раз народ Rogue оказался сильнее, но у тебя есть ещё один шанс...");
                Thread.Sleep(3000);
                Console.Clear();
            }
        }

        static void PlayTheGame(string nameOfPlayer, int level)
        {
            Map map = new Map(mapWidth, mapHeight);
            while (map.IsGameActive)
            {
                map.Refresh();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(33, 0);
                Console.Write("Kill all the enemies and collect bandages");
                Console.SetCursorPosition(48, 2);
                Console.Write("Level: {0}", level);
                Console.SetCursorPosition(24, 34);
                Console.Write("HP: {0} ", map.player.Hits);
                Console.SetCursorPosition(50, 34);
                Console.Write("Monsters: {0}, Bandages: {1}", map.monsters.Count, map.bandages.Count);
                if (map.monsters.Count > 0)
                {
                    map.MonstersActivity();
                }
                else if(map.bandages.Count == 0)
                {
                    level++;
                    map.CreateNewLevel(map, nameOfPlayer, level);
                }
                Console.SetCursorPosition(0, 34);
                Console.Write("Key input > ");
                map.ExecuteCommand(Console.ReadKey());
            }
        }
    }

}
