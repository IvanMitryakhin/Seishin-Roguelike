using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike
{
    public class Map
    {
        public Tile[,] Tiles;
        public List<Wall> walls;
        public List<Monster> monsters;

        public Player player = new Player(new Point(15, 15));


        private int xMaxScreen = 30;
        private int yMaxScreen = 30;
        private int xMax;
        private int yMax;
        private int cameraOffsetX = 0;
        private int cameraOffsetY = 0;

        public Map() { }

        public Map(int xMax, int yMax)
        {
            walls = new List<Wall>();
            monsters = new List<Monster>();

            this.xMax = xMax;
            this.yMax = yMax;
            Tiles = new Tile[xMax, yMax];
            SpawnMobs();
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
        private void SpawnMobs()
        {
            Random r = new Random();
            for (int i = 0; i < r.Next(1, 6); i++)
            {
                // Ensures if mob's spawning position != player's spawning position
                int x = r.Next(0, xMax);
                int y = r.Next(0, yMax);
                if (x == player.X)
                {
                    while (x == player.X)
                    {
                        x = r.Next(0, xMax);
                    }
                }
                if (y == player.Y)
                {
                    while (y == player.X)
                    {
                        y = r.Next(0, yMax);
                    }
                }

                Monster m = new Monster(new Point(x, y));
                monsters.Add(m);
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
            SetMapTiles();
        }
        // Player Movement
        public void GetNewLocation(ConsoleKeyInfo command, Point move)
        {
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
          
        }
    }
}
