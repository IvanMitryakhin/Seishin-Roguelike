using System;
using System.Collections.Generic;
using System.Threading;

namespace Roguelike
{
    public class Map
    {
        public Tile[,] Tiles;
        public List<Wall> walls;
        public List<Monster> monsters;
        public List<Bandage> bandages;

        public Player player = new Player(new Point(15, 15));


        private int xMaxScreen = 30;
        private int yMaxScreen = 30;
        private int xMax;
        private int yMax;
        private int cameraOffsetX = 0;
        private int cameraOffsetY = 0;
        private int xTitle = 41;
        private int yTitle = 4;

        public Map() { }

        public Map(int xMax, int yMax)
        {
            walls = new List<Wall>();
            monsters = new List<Monster>();
            bandages = new List<Bandage>();

            this.xMax = xMax;
            this.yMax = yMax;
            Tiles = new Tile[xMax, yMax];
            SpawnMobsAndBandages(1);
            GenerateMap();
            SetMapTiles();
        }
        public bool IsGameActive
        {
            get
            {
                return player.Hits > 0;
            }
        }

        // Converting to tiles
        private void SetMapTiles()
        {
            SetAllMapSquaresToTiles();
            SetAllMapObjectsToTiles();
        }
        private void SetAllMapSquaresToTiles()
        {
            for (int i = 0; i < yMax; i++)
            {
                for (int j = 0; j < xMax; j++)
                {
                    Tiles[j, i] = new Tile(j, i);
                }
            }
        }
        private void SetAllMapObjectsToTiles()
        {
            walls.ForEach(w => Tiles[w.X, w.Y] = w);
            monsters.ForEach(m => Tiles[m.X, m.Y] = m);
            bandages.ForEach(b => Tiles[b.X, b.Y] = b);
            Tiles[player.X, player.Y] = player;
        }
        // All about generating objects, tiles, etc.
        private void GenerateMap()
        {
            Random r = new Random();
            for (int i = 0; i < xMax; i++)
            {
                Wall top = new Wall(i, 0);
                walls.Add(top);
                Wall bottom = new Wall(i, yMax - 1);
                walls.Add(bottom);
            }

            for (int i = 0; i < yMax; i++)
            {
                Wall left = new Wall(0, i);
                walls.Add(left);
                Wall right = new Wall(xMax - 1, i);
                walls.Add(right);
            }
        }
        protected void SpawnMobsAndBandages(int level)
        {
            Random r = new Random();
            for (int i = 0; i < r.Next(2 + level, 12 + level); i++)
            {
                // Ensures if mob's spawning position != player's spawning position
                int x = r.Next(1, xMax);
                int y = r.Next(1, yMax);
                if (x == player.X)
                {
                    while (x == player.X)
                    {
                        x = r.Next(1, xMax);
                    }
                }
                if (y == player.Y)
                {
                    while (y == player.X)
                    {
                        y = r.Next(1, yMax);
                    }
                }
                if(r.Next(level + 2) > 1)
                {
                    Monster m = new Monster(new Point(x, y));
                    monsters.Add(m);
                }
                else
                {
                    Console.Beep();
                    Bandage b = new Bandage(new Point(x, y));
                    bandages.Add(b);
                }
            }
        }

        public void MonstersActivity()
        {
            foreach(Monster m in monsters)
            {
                MonstersApproach(m);
                int positionOfCreature = -1;
                if (IsBandageNear(m.X, m.Y, ref positionOfCreature))
                {
                    m.Hits += bandages[positionOfCreature].Hits;
                    bandages.RemoveAt(positionOfCreature);
                    Console.SetCursorPosition(xTitle, yTitle);
                    Console.Write("The bandage is lost(         ");
                }
            }
            //Thread.Sleep(100);
        }

        private void MonstersApproach(Monster m)
        {
           if(Math.Abs(m.X + 1 - player.X) < Math.Abs(m.X - player.X) && Math.Abs(m.X + 1 - player.X) > 0)
            {
                m.X += 1;
            }
            else if (Math.Abs(m.X - 1 - player.X) < Math.Abs(m.X - player.X) && Math.Abs(m.X - 1 - player.X) > 0)
            {
                m.X -= 1;
            }
            if (Math.Abs(m.Y + 1 - player.Y) < Math.Abs(m.Y - player.Y) && Math.Abs(m.Y + 1 - player.Y) > 0)
            {
                m.Y += 1;
            }
            else if (Math.Abs(m.Y - 1 - player.Y) < Math.Abs(m.Y - player.Y) && Math.Abs(m.Y - 1 - player.Y) > 0)
            {
                m.Y -= 1;
            }
        }

        // Reads console input
        public void ExecuteCommand(ConsoleKeyInfo command)
        {
            switch (command.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.S:
                case ConsoleKey.A:
                case ConsoleKey.D:
                    GetNewLocation(command, new Point(player.X, player.Y));
                    break;
                case ConsoleKey.Spacebar:
                    HitTheMonster();
                    break;
            }
            int nothing = -1;
            if (IsMobNear(player.X, player.Y, ref nothing))
            {
                Console.SetCursorPosition(xTitle, yTitle);
                Console.WriteLine("Press the space to fight     ");
            }
            if (IsMobNear(player.X, player.Y, ref nothing))
            {
                Console.SetCursorPosition(xTitle, yTitle);
                Console.WriteLine("Press the space to fight     ");
            }
            SetMapTiles();
        }

        // Player Movement
        public void GetNewLocation(ConsoleKeyInfo command, Point move)
        {
            Console.SetCursorPosition(xTitle, yTitle);
            Console.Write("                         ");
            switch (command.Key)
            {
                case ConsoleKey.W:
                    move.Y -= 1;
                    if (cameraOffsetY - 1 >= 0 && !IsInvalidMove(move.X, move.Y))
                        cameraOffsetY -= 1;
                    break;
                case ConsoleKey.S:
                    move.Y += 1;
                    if (cameraOffsetY + 1 <= yMaxScreen && !IsInvalidMove(move.X, move.Y))
                        cameraOffsetY += 1;
                    break;
                case ConsoleKey.A:
                    move.X -= 1;
                    if (cameraOffsetX - 1 >= 0 && !IsInvalidMove(move.X, move.Y))
                        cameraOffsetX -= 1;
                    break;
                case ConsoleKey.D:
                    move.X += 1;
                    if (cameraOffsetX + 1 <= xMaxScreen && !IsInvalidMove(move.X, move.Y))
                        cameraOffsetX += 1;
                    break;
            }
            if (!IsInvalidMove(move.X, move.Y))
            {
                player.X = move.X;
                player.Y = move.Y;
            }
            int positionOfCreature = 0;
            if(IsMobNear(player.X, player.Y, ref positionOfCreature))
            {
                player.Hits -= 1;
            }
            if (IsBandageNear(player.X, player.Y, ref positionOfCreature))
            {
                player.Hits += bandages[positionOfCreature].Hits;
                bandages.RemoveAt(positionOfCreature);
                Console.SetCursorPosition(xTitle, yTitle);
                Console.Write("Health points increased!                    ");
            }
        }

        private bool IsInvalidMove(int x, int y)
        {
            bool mobsNearby = false;
            for (int i = 0; i < monsters.Count; i++)
            {
                mobsNearby = monsters[i].X == x && monsters[i].Y == y;
                if (mobsNearby)
                {
                    break;
                }
            }
            return (x == 0 || x == xMax - 1 || y == yMax - 1 || y == 0) || mobsNearby;
        }

        private bool IsMobNear(int x, int y, ref int numberOfCurrentMob)
        {
            bool mobNear = false;
            for (int i = 0; i < monsters.Count; i++)
            {
                mobNear = (Math.Abs(monsters[i].X - x) <= 1 && Math.Abs(monsters[i].Y - y) <= 1);
                //mobNear = (monsters[i].X == x + 1 || monsters[i].X == x - 1) && (monsters[i].Y == y) || (monsters[i].Y == y - 1 || monsters[i].Y == y + 1) && (monsters[i].X == x);
                if (mobNear)
                {
                    numberOfCurrentMob = i;
                    break;
                }
            }
            return mobNear;
        }

        private bool IsBandageNear(int x, int y, ref int numberOfCurrentBandage)
        {
            bool bandageNear = false;
            for (int i = 0; i < bandages.Count; i++)
            {
                bandageNear = (bandages[i].X == x + 1 || bandages[i].X == x - 1) && (bandages[i].Y == y) || (bandages[i].Y == y - 1 || bandages[i].Y == y + 1) && (bandages[i].X == x);
                if (bandageNear)
                {
                    numberOfCurrentBandage = i;
                    break;
                }
            }
            return bandageNear;
        }

        private void HitTheMonster()
        {
            Console.SetCursorPosition(xTitle, yTitle);
            int numberOfMonster = -1;
            if(IsMobNear(player.X, player.Y, ref numberOfMonster))
            {
                monsters[numberOfMonster].Hits -= 1;
                if(monsters[numberOfMonster].Hits == 0)
                {
                    monsters.RemoveAt(numberOfMonster);
                    player.Hits += 1;
                    Console.Write("      Great job!                 ");
                }
            }
            else
            {
                Console.Write("There's no one around!               ");
            }
        }

        public void CreateNewLevel(Map lastMap, string name, int level)
        {
            Console.SetCursorPosition(xTitle, yTitle);
            Thread.Sleep(1000);
            Console.Clear();
            Console.SetCursorPosition(22, 0);
            Console.WriteLine("Хорош, {0}, но это не всё...", name);
            player.Hits = 5;
            Thread.Sleep(3000);
            Console.Clear();
            SpawnMobsAndBandages(level);
        }

        public void Refresh()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < yMaxScreen; i++)
            {
                for (int j = 0; j < xMaxScreen; j++)
                {

                    Console.ForegroundColor = Tiles[j + cameraOffsetX, i + cameraOffsetY].Color;
                    Console.Write(Tiles[j + cameraOffsetX, i + cameraOffsetY].ImageCharacter);
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(xTitle, yTitle);
        }
    }
}
