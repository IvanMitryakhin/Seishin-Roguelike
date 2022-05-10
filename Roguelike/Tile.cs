using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace Roguelike
{
    public abstract class Tile
    {
        public char ImageCharacter { get; set; }
        public ConsoleColor Color { get; set; }
        public bool IsFree;
        public int X { get; set; }
        public int Y { get; set; }

        public Tile() 
        {
        }

        public Tile(int x, int y, char image, ConsoleColor color, bool isFree)
        {
            X = x;
            Y = y;
            ImageCharacter = image;
            Color = color;
            IsFree = isFree;
        }

    }

    public class Floor : Tile
    {
        public Floor(int x, int y)
            : base(x, y, Constants.FloorImage, Constants.FloorColor, true)
        {
        }
    }

    public class Wall : Tile
    {
        public Wall(int x, int y)
            : base(x, y, Constants.WallImage, Constants.WallColor, false)
        {
            ImageCharacter = Constants.WallImage;
            Color = Constants.WallColor;
        }
    }

    abstract public class Creature : Tile
    {
        public int Hits { get; set; }
        public Creature(Point p, char image, ConsoleColor color, bool isFree)
            : base(p.X, p.Y, image, color, isFree)
        {

        }
    }

    public class Player : Creature
    {
        public Inventory inventory = new Inventory();
        public Player(Point p)
            : base(p, Constants.PlayerImage, Constants.PlayerColor, false)
        {   
            Hits = 5;
        }
    }

    public class Monster : Creature
    {
        public Weapon firstgun { get; set; }
        public Weapon secondgun { get; set; }

        public Monster(Point p, char image, ConsoleColor color, int hits, Weapon firstGun, Weapon secondGun)
           : base(p, image, color, false)
        {
            firstgun = firstGun;
            secondgun = secondGun;
            Hits = hits;
            X = p.X;
            Y = p.Y;
        }
    }

    public class Inventory
    {
        //List<Thing> things = new List<Thing>();
        Thing[] things = new Thing[10];
        int header { get; set; }

        public void Pick(Thing thing)
        {
            things[header] = thing;
        }
        public Thing Push()
        {
            Thing thingToReturn = things[header];
            things[header] = null;
            return thingToReturn;
        }
        public void Choose(int number)
        {
            header = number;
        }
        public bool IsCellFree(int number)
        {
            if (number == 0 || things[number] is Thing)
                return false;
            return true;
        }
        public void GetInfoAboutThings()
        {

        }
    }

    abstract public class Thing : Tile
    {
        public Thing(Point p, char image, ConsoleColor color)
            : base(p.X, p.Y, image, color, false)
        {   
        }
        
    }

    //this is superclass
    abstract public class Potion : Thing
    {
        public Potion(Point p, char image, ConsoleColor color)
            : base(p, image, color)
        {

        }
        public abstract void Apply(Weapon gun);
    }

    public class Rage : Potion
    {
        public Rage(Point p)
            : base(p, Constants.RageImage, Constants.RageColor)
        {
        }
        //RiseDamagePoint
        public override void Apply(Weapon gun)
        {
            gun.DamagePoints *= 5;
        }
    }

    public class RadiusExpansion : Potion
    {
        public RadiusExpansion(Point p)
            : base(p, Constants.RadiusExpansionImage, Constants.RadiusExpansionColor)
        {
        }
        //RiseRadius
        public override void Apply(Weapon gun)
        {
            gun.RadiusOfApplication *= 3;
        }
    }

    //this is superclass
    public class Weapon : Thing
    {
        public int DamagePoints { get; set; }
        public int RadiusOfApplication { get; set; }

        public Weapon(Point p, int damagePoints, int radius, char image, ConsoleColor color)
            : base(p, image, color)
        {
            DamagePoints = damagePoints;
            RadiusOfApplication = radius;
        }

        public void DealDamage(Map map, Tile[,] Tiles, Player player, Monster monster, char direction, char image, ConsoleColor color)
        {
            Point startPoint = new Point(monster.X, monster.Y);
            while(startPoint.X != player.X || startPoint.Y != player.Y)
            {
                if(direction == 'X')
                {
                    if(startPoint.Y - player.Y > 0)
                    {
                        startPoint.Y--;
                    }
                    else
                    {
                        startPoint.Y++;
                    }
                }
                else if(direction == 'Y')
                {
                    if(startPoint.X - player.X > 0)
                    {
                        startPoint.X--;
                    }
                    else
                    {
                        startPoint.X++;
                    }
                }
                if(startPoint.X == player.X && startPoint.Y == player.Y)
                {
                    player.Hits -= DamagePoints;
                    break;
                }
                StrikeAnimation(Tiles, startPoint, image, color);
                map.Refresh();
                //Console.Beep();
                Thread.Sleep(1);
                Tiles[startPoint.X, startPoint.Y].ImageCharacter = Constants.FloorImage;
                Tiles[startPoint.X, startPoint.Y].Color = Constants.FloorColor;
            }
            
        }

        public void StrikeAnimation(Tile[,] Tiles, Point bulletPoint, char image, ConsoleColor color)
        {
            Tiles[bulletPoint.X, bulletPoint.Y].ImageCharacter = image;
            Tiles[bulletPoint.X, bulletPoint.Y].Color = color;
        }

    } 

    public class Bandage : Tile
    {
        public Bandage(Point p)
            : base(p.X, p.Y, Constants.BandageImage, Constants.BandageColor, false)
        {
        }
    }  

    public class Building
    {
        public int length;
        public int height;
        public string Type { get; set; }
        public Building() { }

        public Building(int lengthOfBuilding, int heightOfBuildind, string type)
        {
            length = lengthOfBuilding;
            height = heightOfBuildind;
            Type = type;
        }

        public void SetBuildingtFromTextFile(string nameOfBuilding, Point startpoint, Tile[,] Tiles)
        {
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
                        if (symbol != 46)
                        {
                            Tiles[x, y].IsFree = false;
                        }
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
