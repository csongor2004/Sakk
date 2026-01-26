using System;
using Sakk.Pieces; // Fontos: igy latja a Board a Pieces mappaban levo babukat

namespace Sakk
{
    public class Board
    {
        private Piece[,] grid = new Piece[8, 8];

        public Board()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Fekete tisztek (0. sor) es Gyalogok (1. sor)
            grid[0, 0] = new Rook("Black", "a8");
            grid[0, 1] = new Knight("Black", "b8");
            grid[0, 2] = new Bishop("Black", "c8");
            grid[0, 3] = new Queen("Black", "d8");
            grid[0, 4] = new King("Black", "e8");
            grid[0, 5] = new Bishop("Black", "f8");
            grid[0, 6] = new Knight("Black", "g8");
            grid[0, 7] = new Rook("Black", "h8");

            for (int i = 0; i < 8; i++)
                grid[1, i] = new Pawn("Black", ((char)('a' + i)).ToString() + "7");

            // Feher tisztek (7. sor) es Gyalogok (6. sor)
            grid[7, 0] = new Rook("White", "a1");
            grid[7, 1] = new Knight("White", "b1");
            grid[7, 2] = new Bishop("White", "c1");
            grid[7, 3] = new Queen("White", "d1");
            grid[7, 4] = new King("White", "e1");
            grid[7, 5] = new Bishop("White", "f1");
            grid[7, 6] = new Knight("White", "g1");
            grid[7, 7] = new Rook("White", "h1");

            for (int i = 0; i < 8; i++)
                grid[6, i] = new Pawn("White", ((char)('a' + i)).ToString() + "2");
        }

        public bool MovePiece(string from, string to)
        {
            // Koordinatak kiszamitasa (pl. a1 -> row 7, col 0)
            int fromCol = from[0] - 'a';
            int fromRow = 8 - (from[1] - '0');
            int toCol = to[0] - 'a';
            int toRow = 8 - (to[1] - '0');

            // Palya elhagyasanak ellenorzese
            if (fromRow < 0 || fromRow > 7 || fromCol < 0 || fromCol > 7 ||
                toRow < 0 || toRow > 7 || toCol < 0 || toCol > 7) return false;

            Piece piece = grid[fromRow, fromCol];

            // Polimorfizmus: barmilyen babu IsValidMove-jat meg tudjuk hivni
            if (piece != null && piece.IsValidMove(fromRow, fromCol, toRow, toCol))
            {
                // Lepes vegrehajtasa a racson
                grid[toRow, toCol] = piece;
                grid[fromRow, fromCol] = null;
                piece.Move(to);
                return true;
            }
            return false;
        }

        public void Display()
        {
            Console.WriteLine("  a b c d e f g h");
            for (int row = 0; row < 8; row++)
            {
                Console.Write((8 - row) + " ");
                for (int col = 0; col < 8; col++)
                {
                    if (grid[row, col] == null)
                    {
                        Console.Write(". ");
                    }
                    else
                    {
                        // Kiirjuk a babu tipusanak elso betujet
                        string typeName = grid[row, col].GetType().Name;
                        char letter = typeName[0];
                        // Knight eseten 'N'-t hasznalunk (sakkszabvany)
                        if (typeName == "Knight") letter = 'N';

                        Console.Write(letter + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}