using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    public class Tile
    {
        public string name { get; set; }
        public char ImageCharacter { get; set; }
        public ConsoleColor Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Tile() { }

        public Tile(int x, int y)
            : base()
        {
            X = x;
            Y = y;
            ImageCharacter = Constants.TileImage;
            Color = Constants.TileColor;
        }
    }

    public class Wall : Tile
    {
        public Wall(int x, int y)
            : base(x, y)
        {
            ImageCharacter = Constants.WallImage;
            Color = Constants.WallColor;
        }
    }

    public class Creature : Tile
    {
        public int Hits { get; set; }
        public void Die()
        {
            Hits = 0;
        }
    }

    public class Player : Creature
    {
        public Player(Point p)
        {
            ImageCharacter = Constants.PlayerImage;
            Color = Constants.PlayerColor;
            X = p.X;
            Y = p.Y;
            Hits = 10;
        }
    }

    public class Monster : Creature
    {
        public Monster(Point p)
        {
            ImageCharacter = Constants.MonsterImage;
            Color = Constants.MonsterColor;
            X = p.X;
            Y = p.Y;
            Hits = 5;
        }
    }

    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
