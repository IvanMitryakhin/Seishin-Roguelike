using System;

namespace Roguelike
{
    static class GraphicsEngine
    {
        public static int xMaxScreen = 40;
        public static int yMaxScreen = 30;
        public static int cameraOffsetX ;
        public static int cameraOffsetY;

        public static void Refresh(Map map)
        {
            if (map.player.Hits < 2)
            {
                map.Tiles[map.player.X, map.player.Y].Color = ConsoleColor.Red;
            }
            else if (map.player.Hits > 1)
            {
                map.Tiles[map.player.X, map.player.Y].Color = Constants.PlayerColor;
            }

            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < yMaxScreen; i++)
            {
                for (int j = 0; j < xMaxScreen; j++)
                {
                    Console.ForegroundColor = map.Tiles[j + cameraOffsetX, i + cameraOffsetY].Color;
                    Console.Write(map.Tiles[j + cameraOffsetX, i + cameraOffsetY].ImageCharacter);

                }
                Console.WriteLine();
            }
        }

    }
}