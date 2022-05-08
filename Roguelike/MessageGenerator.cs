using System;
using System.Collections.Generic;

namespace Roguelike
{
    public static class MessageGenerator
    {
        private static int xMessagePosition = 41;
        private static int yMessagePosition = 4;

        private static Dictionary<int, string> _messages =
        new Dictionary<int, string>
        {
            {0, "                        " },
            {1, "Press the space to fight" },
            {2, "Health points increased!" },
            {3, "The bandage is lost" },
            {4, "Great job!" },
            {5, "There's no one around!" },
            {6, "This is not the end..." },

        };

        public static void WriteStartInfo()
        {
            Console.SetCursorPosition(44, 14);
            Console.Write("Motion: W, S, D, A");
            Console.SetCursorPosition(48, 15);
            Console.Write("Hit: space");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(30, 0);
            Console.Write("   Сhoose the name of your samurai: ");
        }

        public static void WriteInfoAboutPlayer(Map map, int level)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(33, 0);
            Console.Write("Kill all the enemies and collect bandages");
            Console.SetCursorPosition(48, 2);
            Console.Write("Level: {0}", level);
            Console.SetCursorPosition(24, 34);
            Console.Write("HP: {0} ", map.player.Hits);
            Console.SetCursorPosition(50, 34);
            Console.Write("Monsters: {0}, Bandages: {1}", map.monsters.Count, map.bandages.Count);
            Console.SetCursorPosition(0, 34);
            Console.Write("Key input > ");
        }
        public static void WriteSomeMessage(int numberOfMessage)
        {
            Console.SetCursorPosition(xMessagePosition, yMessagePosition);
            Console.Write(_messages[0]);
            Console.SetCursorPosition(xMessagePosition, yMessagePosition);
            Console.Write(_messages[numberOfMessage]);
        }
        public static void WriteEndLevel()
        {

        }
    }
}
