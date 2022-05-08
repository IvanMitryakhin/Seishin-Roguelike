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
        public List<Thing> things;

        public Player player = new Player(new Point(15, 15));
        public Inventory inventory = new Inventory();

        private int xMaxScreen = 30;
        private int yMaxScreen = 30;
        private int xMax;
        private int yMax;
        private int cameraOffsetX = 0;
        private int cameraOffsetY = 0;

        private Dictionary<int, Func<Point, Thing>> _types =
        new Dictionary<int, Func<Point, Thing>>
        {
            { 0, CreateKatana },
            { 1, CreateNunchucks },
            { 2, CreateFireball },
            { 3, CreateShuriken },
            { 4, CreateRage },
            { 5, CreateRadiusExpansion },
        };

        private static Thing CreateKatana(Point p)
        {
            return new Weapon(p, 3, 3, Constants.KatanaImage, Constants.KatanaColor);
        }
        private static Thing CreateNunchucks(Point p)
        {
            return new Weapon(p, 2, 4, Constants.NunchucksImage, Constants.NunchucksColor);
        }
        private static Thing CreateFireball(Point p)
        {
            return new Weapon(p, 3, 5, Constants.FireballImage, Constants.FireballColor);
        }
        private static Thing CreateShuriken(Point p)
        {
            return new Weapon(p, 2, 7, Constants.ShurikenImage, Constants.ShurikenColor);
        }
        private static Thing CreateRage(Point p)
        {
            return new Rage(p);
        }
        private static Thing CreateRadiusExpansion(Point p)
        {
            return new RadiusExpansion(p);
        }


    public Map() { }

        public Map(int xMax, int yMax)
        {
            walls = new List<Wall>();
            monsters = new List<Monster>();
            bandages = new List<Bandage>();
            things = new List<Thing>();

            this.xMax = xMax;
            this.yMax = yMax;
            Tiles = new Tile[xMax, yMax];

            GenerateMap();
        }

        // All about generating objects, tiles, etc.
        private void GenerateMap()
        {
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
            SpawnThings();
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

        private void SpawnThings()
        {
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                Point startPoint = CreateRandomPoint(xMax, yMax);
                if (!Tiles[startPoint.X, startPoint.Y].IsFree)
                {
                    while (!Tiles[startPoint.X, startPoint.Y].IsFree)
                    {
                        startPoint = CreateRandomPoint(xMax, yMax);
                    }
                }
                
                things.Add(_types[r.Next(6)](startPoint));
            }
        }

        private void ChooseRandomPotion()
        {

        }

        private Point CreateRandomPoint(int xMaxValue, int yMaxValue)
        {
            Random r = new Random();
            Point randomPoint = new Point(r.Next(2, xMaxValue), r.Next(2, yMaxValue));
            return randomPoint;
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
            things.ForEach(t => Tiles[t.X, t.Y] = t);
        }


        private void SpawnBuildings()
        {
            Random r = new Random();
            int countOfBuildings = 0;
            int randomCountOfBuilding = r.Next(7, 15);
            int numberOfBuilding;
            Building[] buildings = new Building[3];
            buildings[0] = new Building(11, 5, "house");//House
            buildings[1] = new Building(8, 4, "warehouse");//Warehouse
            buildings[2] = new Building(11, 5, "palace");//Palace

            while (countOfBuildings < randomCountOfBuilding)
            {
                Point startPoint = CreateRandomPoint(xMax, yMax);
                numberOfBuilding = r.Next(3);

                if (CheckingForPossibilityOfConstruction(startPoint, buildings[numberOfBuilding].length, buildings[numberOfBuilding].height))
                {
                    buildings[numberOfBuilding].SetBuildingtFromTextFile(buildings[numberOfBuilding].Type, startPoint, Tiles);
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
            for (int i = 0; i < r.Next(10 + level, 12 + level); i++)
            {
                // Ensures if mob's spawning position occupied position
                Point startPoint = CreateRandomPoint(xMax, yMax);
                if (!Tiles[startPoint.X, startPoint.Y].IsFree)
                {
                    while (!Tiles[startPoint.X, startPoint.Y].IsFree)
                    {
                        startPoint = CreateRandomPoint(xMax, yMax);
                    }
                }

                if (r.Next(level + 5) > 1)
                {
                    char image;
                    ConsoleColor color;

                    if (r.Next(2) == 0)
                    {
                        image = Constants.CloseCombatMonsterImage;
                        color = Constants.CloseCombatMonsterColor;
                        //create close combat with Katana and Nunchucks
                        Monster closeCombatMonster = new Monster(startPoint, image, color, 3, (Weapon)_types[0](startPoint), (Weapon)_types[1](startPoint));
                        monsters.Add(closeCombatMonster);
                    }
                    else
                    {
                        image = Constants.DistantCombatMonsterImage;
                        color = Constants.DistantCombatMonsterColor;
                        //create distant combat with Fireball and Shuriken
                        Monster distantCombatMonster = new Monster(startPoint, image, color, 5, (Weapon)_types[2](startPoint), (Weapon)_types[3](startPoint));
                        monsters.Add(distantCombatMonster);
                    }

                }
                else
                {
                    Console.Beep();
                    Bandage b = new Bandage(startPoint);
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
                case ConsoleKey.Spacebar:
                    HitTheMonster();
                    break;
                case ConsoleKey.E:
                    Point coordinatesOfThing = new Point();
                    if(IsThingNear(player.X, player.Y, coordinatesOfThing))
                    {
                        inventory.Pick((Thing)Tiles[coordinatesOfThing.X, coordinatesOfThing.Y]);
                        RemoveThing(coordinatesOfThing);
                    }                   
                    break;
            }
            int nothing = -1;
            if (IsMobNear(player.X, player.Y, ref nothing))
            {
                MessageGenerator.WriteSomeMessage(1);
            }

        }

        private void RemoveThing(Point coordinates)
        {
            Tiles[coordinates.X, coordinates.Y] = new Floor(coordinates.X, coordinates.Y);
        }

        private bool IsThingNear(int x, int y, Point coordinates)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if(Tiles[i,j] is Thing)
                    {
                        coordinates.X = i;
                        coordinates.Y = j;
                        return true;
                    }
                }
            }
            return false;
        }
        // Player Movement
        private void GetNewLocation(ConsoleKeyInfo command, Point move)
        {
            MessageGenerator.WriteSomeMessage(0);
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
                Tiles[player.X, player.Y].ImageCharacter = Constants.FloorImage;
                Tiles[player.X, player.Y].Color = Constants.FloorColor;
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
                MessageGenerator.WriteSomeMessage(2);
            }
        }

        private void DestructionOfBandage(Creature monsterOrPlayer, int numberOfCurrentBandage)
        {
            Point coordinatesBandage = new Point(bandages[numberOfCurrentBandage].X, bandages[numberOfCurrentBandage].Y);
            monsterOrPlayer.Hits += 1;
            Tiles[coordinatesBandage.X, coordinatesBandage.Y].ImageCharacter = Constants.FloorImage;
            Tiles[coordinatesBandage.X, coordinatesBandage.Y].Color = Constants.FloorColor;
            Tiles[coordinatesBandage.X, coordinatesBandage.Y].IsFree = true;
            bandages.RemoveAt(numberOfCurrentBandage);
        }

        //Monsters movement
        public void MonstersActivity(Map map)
        {
            foreach (Monster m in monsters)
            {
                //MonstersApproach(m);
                MonstersKick(map, m);
                int positionOfCreature = -1;
                if (IsBandageNear(m.X, m.Y, ref positionOfCreature))
                {
                    DestructionOfBandage(m, positionOfCreature);
                    MessageGenerator.WriteSomeMessage(3);
                }
            }
            //Thread.Sleep(100);
        }

        private void MonstersKick(Map map, Monster monster)
        {
            int horizontalDistance = player.X - monster.X;
            int verticalDistance = player.Y - monster.Y;
            char directionOptionForFirstgun = IsDirectionAcceptable(monster, monster.firstgun.RadiusOfApplication, horizontalDistance, verticalDistance);
            char directionOptionForSecondgun = IsDirectionAcceptable(monster, monster.secondgun.RadiusOfApplication, horizontalDistance, verticalDistance);

            if(directionOptionForFirstgun == 'X' || directionOptionForFirstgun == 'Y')
            {
                char image = monster.firstgun.ImageCharacter;
                ConsoleColor color = monster.firstgun.Color;
                monster.firstgun.DealDamage(map, Tiles, player, monster, directionOptionForFirstgun, image, color);
            }
            else if (directionOptionForSecondgun == 'X' || directionOptionForSecondgun == 'Y')
            {
                char image = monster.secondgun.ImageCharacter;
                ConsoleColor color = monster.secondgun.Color;
                monster.secondgun.DealDamage(map, Tiles, player, monster, directionOptionForSecondgun, image, color);
            }
        }

        private char IsDirectionAcceptable(Monster monster, int radiusOfDamage, int horizontalDistance, int verticalDistance)
        {
            Point checkPoint = new Point(monster.X, monster.Y);
            if ((radiusOfDamage > Math.Abs(horizontalDistance)) && verticalDistance == 0)
            {
                while(checkPoint.X != player.X)
                {
                    checkPoint.X = checkPoint.X + Math.Sign(horizontalDistance) * 1;
                    horizontalDistance = horizontalDistance - Math.Sign(horizontalDistance) * 1;
                    if(Tiles[checkPoint.X,checkPoint.Y].ImageCharacter != Constants.FloorImage && Tiles[checkPoint.X, checkPoint.Y].ImageCharacter != Constants.PlayerImage)
                    {
                        return 'N';
                    }
                }
                return 'Y';
            }
            else if((radiusOfDamage > Math.Abs(verticalDistance)) && horizontalDistance == 0)
            {
                while (checkPoint.Y != player.Y)
                {
                    checkPoint.Y = checkPoint.Y + Math.Sign(verticalDistance) * 1;
                    verticalDistance = verticalDistance - Math.Sign(verticalDistance) * 1;
                    if (Tiles[checkPoint.X, checkPoint.Y].ImageCharacter != Constants.FloorImage && Tiles[checkPoint.X, checkPoint.Y].ImageCharacter != Constants.PlayerImage)
                    {
                        return 'N';
                    }
                }
                return 'X';
            }
            return 'N';
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
            int numberOfMonster = -1;
            if (IsMobNear(player.X, player.Y, ref numberOfMonster))
            {
                monsters[numberOfMonster].Hits -= 1;
                if (monsters[numberOfMonster].Hits == 0)
                {
                    RemoveMonster(numberOfMonster);
                    player.Hits += 1;
                    MessageGenerator.WriteSomeMessage(4);
                }
            }
            else
            {
                MessageGenerator.WriteSomeMessage(5);
            }
        }
        private void RemoveMonster(int number)
        {
            int x = monsters[number].X;
            int y = monsters[number].Y;
            Tiles[x, y] = new Floor(x, y);
            monsters.RemoveAt(number);
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
        public void CreateNewLevel(Map lastMap, int level)
        {
            Thread.Sleep(1000);
            Console.Clear();
            Console.SetCursorPosition(22, 0);
            Console.WriteLine("This is not the end...           ");
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
            //Console.SetCursorPosition(xTitle, yTitle);
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
