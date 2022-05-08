using System;

namespace Roguelike
{
    public static class Constants
    {
        public readonly static char FloorImage = '.';
        public readonly static char WallImage = '#';
        public readonly static char PlayerImage = '@';
        public readonly static char BandageImage = '+';
        public readonly static char CloseCombatMonsterImage = 'M';
        public readonly static char DistantCombatMonsterImage = 'W';
        public readonly static char ShurikenImage = '*';
        public readonly static char FireballImage = 'o';
        public readonly static char KatanaImage = '/';
        public readonly static char NunchucksImage = '=';
        public readonly static char RageImage = 'R';
        public readonly static char RadiusExpansionImage = 'D';

        public readonly static ConsoleColor PlayerColor = ConsoleColor.White;
        public readonly static ConsoleColor WallColor = ConsoleColor.Yellow;
        public readonly static ConsoleColor FloorColor = ConsoleColor.Gray;
        public readonly static ConsoleColor BandageColor = ConsoleColor.Green;
        public readonly static ConsoleColor CloseCombatMonsterColor = ConsoleColor.Red;
        public readonly static ConsoleColor DistantCombatMonsterColor = ConsoleColor.DarkRed;
        public readonly static ConsoleColor ShurikenColor = ConsoleColor.DarkGray;
        public readonly static ConsoleColor FireballColor = ConsoleColor.DarkYellow;
        public readonly static ConsoleColor KatanaColor = ConsoleColor.Blue;
        public readonly static ConsoleColor NunchucksColor = ConsoleColor.DarkYellow;
        public readonly static ConsoleColor RageColor = ConsoleColor.Red;
        public readonly static ConsoleColor RadiusExpansionColor = ConsoleColor.Blue;
    }
}
