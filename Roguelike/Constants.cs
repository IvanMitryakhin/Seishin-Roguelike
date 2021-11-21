using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    public static class Constants
    {
        public readonly static char TileImage = '.';
        public readonly static char WallImage = '#';
        public readonly static char PlayerImage = '@';
        public readonly static char MonsterImage = 'M';

        public readonly static ConsoleColor PlayerColor = ConsoleColor.White;
        public readonly static ConsoleColor WallColor = ConsoleColor.Yellow;
        public readonly static ConsoleColor TileColor = ConsoleColor.Green;
        public readonly static ConsoleColor MonsterColor = ConsoleColor.Red;
    }
}
