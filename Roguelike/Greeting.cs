using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Roguelike
{
    public class Greeting
    {
        private string nameOfGame = "Рогалик за день";
        private string description = "Когда-то давно 4 расы рогаликов жили в мире. Но народ rogue развязал войну";
        public string nameOfPlayer;

        public Greeting()
        {
            Start();
            Console.SetCursorPosition(40, 14);
            Console.WriteLine("Передвижение: W, S, D, A");
            Console.SetCursorPosition(47, 15);
            Console.WriteLine("Удар: space");
        }

        private void Naming(string name)
        {
            nameOfPlayer = name;
            Console.WriteLine("Давай начнём, {0}...", nameOfPlayer);
            Thread.Sleep(2000);
            Console.WriteLine(description);
            Thread.Sleep(3000);
            Console.Clear();
        }

        private void Start()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(30, 0);
            Console.WriteLine(nameOfGame);
            Console.Write("Выберите имя своему рогалику:  ");
            Naming(Console.ReadLine());
        }

    }
}
