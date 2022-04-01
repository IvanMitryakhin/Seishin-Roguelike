using System;
using System.Threading;
using System.IO;

namespace Roguelike
{
    public class Tile
    {
        protected Random r = new Random();
        public char ImageCharacter { get; set; }
        public ConsoleColor Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsFree;
        public Tile() { }

        public Tile(int x, int y, string t)
        {
            X = x;
            Y = y;
            ImageCharacter = Constants.TileImage;
            Color = Constants.TileColor;
            IsFree = true;
        }

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
        }

    }

    public class Floor : Tile
    {
        public Floor(int x, int y)
            : base(x, y, "floor")
        {
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

    abstract public class Creature : Tile
    {
        public int Hits { get; set; }
    }

    public class Player : Creature
    {
        public Player(Point p)
        {
            ImageCharacter = Constants.PlayerImage;
            Color = Constants.PlayerColor;
            X = p.X;
            Y = p.Y;
            Hits = 5;
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
            Hits = r.Next(1, 6);
        }
    }
    public class Bandage : Creature
    {
        public Bandage(Point p)
        {
            ImageCharacter = Constants.BandageImage;
            Color = Constants.BandageColor;
            X = p.X;
            Y = p.Y;
            Hits = r.Next(1, 3);
        }
    }

    abstract public class Building
    {
        public int length;
        public int height;
        public Building() { }

        public Building(int lengthOfBuilding, int heightOfBuildind)
        {
            length = lengthOfBuilding;
            height = heightOfBuildind;
        }


        public void SetBuildingtFromTextFile(Type type, Point startpoint, Tile[,] Tiles)
        {
            string nameOfBuilding = type.ToString().Remove(0, 10).ToLower();
            string path = $@"C:\PROJECTS!\Roguelike\Seishin-Roguelike\Roguelike\{nameOfBuilding}.txt";
            if (File.Exists(path) == false)
            {
                Console.WriteLine($"Файла для {nameOfBuilding} не существует");
                return;
            }

            using (StreamReader sr = File.OpenText(path))
            {
                int x = startpoint.X;
                int y = startpoint.Y;

                int symbol = sr.Read();
                while (symbol != -1)
                {
                    if (symbol != 10 && symbol != 13)
                    {
                        Tiles[x, y].ImageCharacter = Convert.ToChar(symbol);
                        if (symbol == 42)
                        {
                            Tiles[x, y].IsFree = false;
                        }
                        //Console.Write(Convert.ToChar(symbol) + " ");
                        //Thread.Sleep(70);
                        x++;
                    }
                    else
                    {
                        if (symbol == 10)
                        {
                            x = startpoint.X;
                            y--;
                        }
                    }
                    symbol = sr.Read();
                }
            }
        }
    }


    public class House : Building
    {
        public House(int length = 11, int height = 5)
            : base(length, height)
        {

        }

    }

    public class Warehouse : Building
    {
        public Warehouse(int length = 8, int height = 4)
            : base(length, height)
        {

        }

    }

    public class Palace : Building
    {
        public Palace(int length = 11, int height = 5)
            : base(length, height)
        {

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
