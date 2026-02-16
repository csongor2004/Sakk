using System;
using Sakk.Pieces;
namespace Sakk
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            string currentTurn = "White";

            while (true)
            {
                Console.Clear();
                board.Display();

                bool inCheck = board.IsInCheck(currentTurn);
                bool canMove = board.HasLegalMoves(currentTurn);

                if (inCheck && !canMove)
                {
                    Console.WriteLine($"CHECKMATE! {currentTurn} loses.");
                    board.SaveHistoryToFile();
                    break;
                }
                if (!inCheck && !canMove)
                {
                    Console.WriteLine("STALEMATE! It's a draw.");
                    board.SaveHistoryToFile();
                    break;
                }
                if (inCheck) Console.WriteLine($"WARNING: {currentTurn} is in CHECK!");

                Console.WriteLine($"\n{currentTurn}'s turn. Enter move (e2 e4), 'resign', 'save' or 'exit':");
                string input = Console.ReadLine()?.ToLower();

                if (input == "exit") break;
                if (input == "resign")
                {
                    Console.WriteLine($"{currentTurn} resigned. Game over.");
                    board.SaveHistoryToFile();
                    break;
                }
                if (input == "save") { board.SaveHistoryToFile(); continue; }

                string[] parts = input.Split(' ');
                if (parts.Length == 2 && board.MovePiece(parts[0], parts[1], currentTurn))
                {
                    currentTurn = (currentTurn == "White") ? "Black" : "White";
                }
                else
                {
                    Console.WriteLine("Invalid move! Press any key...");
                    Console.ReadKey();
                }
            }
        }
    }
}