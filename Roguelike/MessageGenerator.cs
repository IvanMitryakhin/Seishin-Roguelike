using System;
using System.Collections.Generic;

namespace Roguelike
{
    public static class MessageGenerator
    {
        private static int xMessagePosition = 91;
        private static int yMessagePosition = 4;

        private static Dictionary<int, string> _messages =
        new Dictionary<int, string>
        {
            {0, "                                              " },
            {1, "Press the space to fight" },
            {2, "Health points increased!" },
            {3, "The bandage is lost" },
            {4, "Great job!" },
            {5, "There's no one around!" },
            {6, "This is not the end..." },
            {7, "Your hand is busy, release it" },
            {8, "Choose cell with thing, 0-rejection: " },
            {9, "This cell is occupied, want to exchange - Y: " },
            {10, "Choose cell to push in hand: " },
            {11, "Choose an occupied cell, 0-rejection: " },
            {12, "The hand is busy, want to exchange - Y: " },
            {13, "Your hand is free" },
            {14, "There are no thing around!" },
            {15, "This is not the end...           " },
            {16, "You have lost, now the game will restart" }
        };

        public static void WriteStartInfo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(94, 13);
            Console.Write("Motion: W, S, D, A");
            Console.SetCursorPosition(98, 14);
            Console.Write("Hit: space");
            Console.SetCursorPosition(93, 16);
            Console.Write("Work with inventory:");
            Console.SetCursorPosition(93, 18);
            Console.Write("Get inventory state: I");
            Console.SetCursorPosition(93, 19);
            Console.Write("Catch thing in hand: C");
            Console.SetCursorPosition(93, 20);
            Console.Write("Push thing from hand in inventory: P");
            Console.SetCursorPosition(93, 21);
            Console.Write("Take thing in hand from inventory: T");
            Console.SetCursorPosition(93, 22);
            Console.Write("Eject thing on map from hand: E");
            Console.SetCursorPosition(84, 0);
            Console.Write("Сhoose the name of your samurai: ");
        }

        public static void WriteInfoAboutPlayer(Map map, int level)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(83, 0);
            Console.Write("Kill all the enemies and collect bandages");
            Console.SetCursorPosition(98, 2);
            Console.Write("Level: {0}", level);
            Console.SetCursorPosition(90, 34);
            Console.Write("HP: {0} ", map.player.Hits);
            Console.SetCursorPosition(90, 34);
            Console.Write("", map.player.inventory);
            Console.SetCursorPosition(100, 34);
            Console.Write("Monsters: {0}, Bandages: {1}", map.monsters.Count, map.bandages.Count);
            Console.SetCursorPosition(0, 34);
            Console.Write("Key input > ");
        }
        public static void WriteSomeMessage(int numberOfMessage)
        {
            Console.SetCursorPosition(xMessagePosition, yMessagePosition);
            Console.Write(_messages[0]);
            Console.SetCursorPosition(xMessagePosition, yMessagePosition + 1);
            Console.Write(_messages[0]);
            Console.SetCursorPosition(xMessagePosition, yMessagePosition);
            Console.Write(_messages[numberOfMessage]);
        }
        public static void WriteEndLevel(int number)
        {
            Console.Clear();
            Console.SetCursorPosition(60, 15);
            Console.WriteLine(_messages[number]);
        }
    }
}
