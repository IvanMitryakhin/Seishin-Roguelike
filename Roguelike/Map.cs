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

            GenerateMap();
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

            SetAllMapSquaresToTiles();
            SpawnPlayer();
            SpawnBuildings();
            SpawnMobsAndBandages(1);
            SetAllMapObjectsToTiles();
        }

        private void SetAllMapSquaresToTiles()
        {
            for (int i = 0; i < yMax; i++)
            {
                for (int j = 0; j < xMax; j++)
                {
                    Tiles[j, i] = new Floor(j, i);
                }
            }

        }

        private void SpawnPlayer()
        {
            Tiles[player.X, player.Y] = player;
            Tiles[player.X, player.Y].IsFree = false;
        }

        private void SetAllMapObjectsToTiles()
        {
            walls.ForEach(w => Tiles[w.X, w.Y] = w);
            monsters.ForEach(m => Tiles[m.X, m.Y] = m);
            bandages.ForEach(b => Tiles[b.X, b.Y] = b);
        }


        private void SpawnBuildings()
        {
            Random r = new Random();
            int countOfBuildings = 0;
            int randomCountOfBuilding = r.Next(7, 15);
            int numberOfBuilding;
            Building[] buildings = new Building[3];
            buildings[0] = new House();
            buildings[1] = new Warehouse();
            buildings[2] = new Palace();

            while (countOfBuildings < randomCountOfBuilding)
            {
                Point startPoint = new Point(r.Next(1, xMax), r.Next(1, yMax));
                numberOfBuilding = r.Next(3);

                if (CheckingForPossibilityOfConstruction(startPoint, buildings[numberOfBuilding].length, buildings[numberOfBuilding].height))
                {
                    buildings[numberOfBuilding].SetBuildingtFromTextFile(buildings[numberOfBuilding].GetType(), startPoint, Tiles);
                    countOfBuildings++;
                }

            }

        }

        private bool CheckingForPossibilityOfConstruction(Point p, int length, int height)
        {
            Point breakPoint = new Point(0, 0);
            for (int i = p.X; i < p.X + length; i++)
            {
                breakPoint.Y = 0;
                for (int j = p.Y; j > p.Y - height; j--)
                {
                    breakPoint.Y++;
                    if (i >= xMax - 1 || j <= 1 || !Tiles[i, j].IsFree)
                    {
                        return false;
                    }

                }
                breakPoint.X++;

            }
            if (breakPoint.X == length && breakPoint.Y == height)
            {
                return true;
            }
            return false;
        }

        private void SpawnMobsAndBandages(int level)
        {
            Random r = new Random();
            for (int i = 0; i < r.Next(2 + level, 12 + level); i++)
            {
                // Ensures if mob's spawning position occupied position
                int x = r.Next(1, xMax);
                int y = r.Next(1, yMax);
                if (!Tiles[x, y].IsFree)
                {
                    while (!Tiles[x, y].IsFree)
                    {
                        x = r.Next(1, xMax);
                        y = r.Next(1, yMax);
                    }
                }

                if (r.Next(level + 2) > 1)
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
            }
            int nothing = -1;
            if (IsMobNear(player.X, player.Y, ref nothing))
            {
                Console.SetCursorPosition(xTitle, yTitle);
                Console.WriteLine("Press the space to fight     ");
            }

        }

        // Player Movement
        private void GetNewLocation(ConsoleKeyInfo command, Point move)
        {
            Console.SetCursorPosition(xTitle, yTitle);
            Console.Write("                         ");
            switch (command.Key)
            {
                case ConsoleKey.W:
                    move.Y -= 1;
                    if (cameraOffsetY - 1 >= 0 && !IsInvalidMove(move.X, move.Y))
                        if (yMax - move.Y > 15)
                            cameraOffsetY -= 1;
                    break;
                case ConsoleKey.S:
                    move.Y += 1;
                    if (cameraOffsetY + 1 <= yMaxScreen && !IsInvalidMove(move.X, move.Y))
                        if (move.Y > 15)
                            cameraOffsetY += 1;
                    break;
                case ConsoleKey.A:
                    move.X -= 1;
                    if (cameraOffsetX - 1 >= 0 && !IsInvalidMove(move.X, move.Y))
                        if (xMax - move.X > 15)
                            cameraOffsetX -= 1;
                    break;
                case ConsoleKey.D:
                    move.X += 1;
                    if (cameraOffsetX + 1 <= xMaxScreen && !IsInvalidMove(move.X, move.Y))
                        if (move.X > 15)
                            cameraOffsetX += 1;
                    break;
            }
            if (!IsInvalidMove(move.X, move.Y))
            {
                Tiles[player.X, player.Y].IsFree = true;
                Tiles[player.X, player.Y].ImageCharacter = Constants.TileImage;
                Tiles[player.X, player.Y].Color = Constants.TileColor;
                player.X = move.X;
                player.Y = move.Y;
                Tiles[player.X, player.Y].IsFree = false;
                Tiles[player.X, player.Y].ImageCharacter = Constants.PlayerImage;
                Tiles[player.X, player.Y].Color = Constants.PlayerColor;
            }

            int numberInList = 0;
            if (IsMobNear(player.X, player.Y, ref numberInList))
            {
                player.Hits -= 1;
            }

            if (IsBandageNear(player.X, player.Y, ref numberInList))
            {
                DestructionOfBandage(player, numberInList);
                Console.SetCursorPosition(xTitle, yTitle);
                Console.Write("Health points increased!                    ");
            }
        }

        private void DestructionOfBandage(Creature monsterOrPlayer, int numberOfCurrentBandage)
        {
            Point coordinatesBandage = new Point(bandages[numberOfCurrentBandage].X, bandages[numberOfCurrentBandage].Y);
            monsterOrPlayer.Hits += bandages[numberOfCurrentBandage].Hits;
            Tiles[coordinatesBandage.X, coordinatesBandage.Y].ImageCharacter = Constants.TileImage;
            Tiles[coordinatesBandage.X, coordinatesBandage.Y].Color = Constants.TileColor;
            bandages.RemoveAt(numberOfCurrentBandage);
        }

        // Monsters movement
        public void MonstersActivity()
        {
            foreach (Monster m in monsters)
            {
                MonstersApproach(m);
                int positionOfCreature = -1;
                if (IsBandageNear(m.X, m.Y, ref positionOfCreature))
                {
                    DestructionOfBandage(m, positionOfCreature);
                    Console.SetCursorPosition(xTitle, yTitle);
                    Console.Write("The bandage is lost(         ");
                }
            }
            //Thread.Sleep(100);
        }

        private void MonstersApproach(Monster m)
        {
            if (Math.Abs(m.X + 1 - player.X) < Math.Abs(m.X - player.X) && Math.Abs(m.X + 1 - player.X) > 0)
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

        private void HitTheMonster()
        {
            Console.SetCursorPosition(xTitle, yTitle);
            int numberOfMonster = -1;
            if (IsMobNear(player.X, player.Y, ref numberOfMonster))
            {
                monsters[numberOfMonster].Hits -= 1;
                if (monsters[numberOfMonster].Hits == 0)
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

        // All checks
        private bool IsInvalidMove(int x, int y)
        {
            bool mobsOrBuidingNearby = false;
            for (int i = 0; i < monsters.Count; i++)
            {
                mobsOrBuidingNearby = monsters[i].X == x && monsters[i].Y == y || !Tiles[x, y].IsFree;
                if (mobsOrBuidingNearby)
                {
                    break;
                }
            }
            return (x == 0 || x == xMax - 1 || y == yMax - 1 || y == 0) || mobsOrBuidingNearby;
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

        // All updates
        public void CreateNewLevel(Map lastMap, string name, int level)
        {
            Console.SetCursorPosition(xTitle, yTitle);
            Thread.Sleep(1000);
            Console.Clear();
            Console.SetCursorPosition(22, 0);
            Console.WriteLine("{0}, this is not the end... a", name);
            player.Hits = 5;
            Thread.Sleep(3000);
            Console.Clear();
            SpawnMobsAndBandages(level);
        }

        public void Refresh()
        {
            if (player.Hits < 2)
            {
                Tiles[player.X, player.Y].Color = ConsoleColor.Red;
            }
            else if (player.Hits > 1)
            {
                Tiles[player.X, player.Y].Color = Constants.PlayerColor;
            }

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

        public bool IsGameActive
        {
            get
            {
                return player.Hits > 0;
            }
        }
    }

}
