using System;

namespace Sakk
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BitChess v1.0 - Fejleszto: Csongor");
            Board board = new Board();

            while (true)
            {
                Console.Clear();
                board.Display();

                Console.WriteLine("\nAdja meg a lepest (pl. b2 b4) vagy 'exit' a kilepeshez:");
                string input = Console.ReadLine();

                if (input == "exit") break;

                string[] parts = input.Split(' ');
                if (parts.Length == 2)
                {
                    if (board.MovePiece(parts[0], parts[1]))
                    {
                        Console.WriteLine("Sikeres lepes!");
                    }
                    else
                    {
                        Console.WriteLine("Ures mezo vagy ervenytelen koordinata!");
                    }
                }
            }
        }
    }
}