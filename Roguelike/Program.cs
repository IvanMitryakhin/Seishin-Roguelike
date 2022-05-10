using System;
using System.Threading;

namespace Roguelike
{
    class Program
    {
        private const int ScreenWidth = 150;
        private const int ScreenHeight = 35;
        private const int mapWidth = 100;
        private const int mapHeight = 100;

        static void Main()
        {
            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            MessageGenerator.WriteStartInfo();
            while (true)
            {
                int level = 1;
                PlayTheGame(level);
                Console.Clear();
            }
        }

        static void PlayTheGame(int level)
        {
            Map map = new Map(mapWidth, mapHeight);
            while (map.IsGameActive)
            {
                map.Refresh();
                MessageGenerator.WriteInfoAboutPlayer(map, level);
                map.ExecuteCommand(Console.ReadKey());
                if (map.monsters.Count > 0)
                {
                    map.MonstersActivity(map);
                }
                else if (map.bandages.Count == 0)
                {
                    level++;
                    map.CreateNewLevel(map, level);
                }
            }
        }
    }

}
