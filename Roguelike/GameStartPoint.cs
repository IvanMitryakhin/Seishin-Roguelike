using System;
using System.Threading;

namespace Roguelike
{
    class GameStartPoint
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

            while(true)
            {
                int level = 1;
                PlayTheGame(level);
                MessageGenerator.WriteEndLevel(16);
                Thread.Sleep(3000);
                Console.Clear();
            }
        }

        static void PlayTheGame(int level)
        {
            GraphicsEngine.cameraOffsetX = 0;
            GraphicsEngine.cameraOffsetY = 0;
            Console.Clear();
            MessageGenerator.WriteStartInfo();
    
            Map map = new Map(mapWidth, mapHeight);

            while (map.IsGameActive)
            {
                GraphicsEngine.Refresh(map);
                MessageGenerator.WriteInfoAboutPlayer(map, level);
                map.ExecuteCommand(Console.ReadKey());
                if (map.monsters.Count > 0)
                {
                    map.MonstersActivity(map);
                }
                else if (map.bandages.Count == 0)
                {
                    MessageGenerator.WriteEndLevel(15);
                    Thread.Sleep(3000);
                    PlayTheGame(level + 1);
                }
            }
        }
    }

}
