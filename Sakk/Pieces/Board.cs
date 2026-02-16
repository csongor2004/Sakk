using System;
using System.Collections.Generic;
using System.IO;
using Sakk.Pieces;

namespace Sakk.Pieces
{
    public class Board
    {
        public Piece[,] grid = new Piece[8, 8];
        
        public List<string> MoveHistory { get; private set; } = new List<string>();

        public Board()
        {
            InitializeBoard();
        }

        public void InitializeBoard()
        {
            string[] colors = { "Black", "White" };
            foreach (string color in colors)
            {
                int r = (color == "Black") ? 0 : 7;
                int p = (color == "Black") ? 1 : 6;

                grid[r, 0] = new Rook(color, "a" + (8 - r));
                grid[r, 7] = new Rook(color, "h" + (8 - r));
                grid[r, 1] = new Knight(color, "b" + (8 - r));
                grid[r, 6] = new Knight(color, "g" + (8 - r));
                grid[r, 2] = new Bishop(color, "c" + (8 - r));
                grid[r, 5] = new Bishop(color, "f" + (8 - r));
                grid[r, 3] = new Queen(color, "d" + (8 - r));
                grid[r, 4] = new King(color, "e" + (8 - r));

                for (int i = 0; i < 8; i++)
                    grid[p, i] = new Pawn(color, ((char)('a' + i)).ToString() + (8 - p));
            }
        }

        public bool MovePiece(string from, string to, string playerColor)
        {
            int fromCol = from[0] - 'a';
            int fromRow = 8 - (from[1] - '0');
            int toCol = to[0] - 'a';
            int toRow = 8 - (to[1] - '0');

            if (fromRow < 0 || fromRow > 7 || fromCol < 0 || fromCol > 7 ||
                toRow < 0 || toRow > 7 || toCol < 0 || toCol > 7) return false;

            Piece attacker = grid[fromRow, fromCol];
            Piece target = grid[toRow, toCol];

            if (attacker == null || attacker.Color != playerColor) return false;
            if (target != null && target.Color == attacker.Color) return false;

            // Pawn specific logic for capture and movement
            if (attacker is Pawn)
            {
                int deltaCol = Math.Abs(toCol - fromCol);
                if (deltaCol == 0 && target != null) return false;
                if (deltaCol == 1 && target == null) return false;
            }

            if (!(attacker is Knight) && !IsPathClear(fromRow, fromCol, toRow, toCol)) return false;

            if (attacker.IsValidMove(fromRow, fromCol, toRow, toCol))
            {
                Piece originalTarget = grid[toRow, toCol];
                grid[toRow, toCol] = attacker;
                grid[fromRow, fromCol] = null;

                // Do not allow moves that leave the king in check
                if (IsInCheck(playerColor))
                {
                    grid[fromRow, fromCol] = attacker;
                    grid[toRow, toCol] = originalTarget;
                    return false;
                }

                attacker.Move(to);

                // Record the move in history
                MoveHistory.Add($"{playerColor}: {from} -> {to}");

                

                return true;
            }
            return false;
        }

        

        // Save history to a text file
        public void SaveHistoryToFile()
        {
            try
            {
                File.WriteAllLines("move_history.txt", MoveHistory);
                Console.WriteLine("History saved to move_history.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving history: " + e.Message);
            }
        }
        public void PromotePawn(int row, int col, string pieceType)
        {
            string color = grid[row, col].Color;
            string pos = grid[row, col].Position;
            switch (pieceType)
            {
                case "Rook": grid[row, col] = new Rook(color, pos); break;
                case "Bishop": grid[row, col] = new Bishop(color, pos); break;
                case "Knight": grid[row, col] = new Knight(color, pos); break;
                default: grid[row, col] = new Queen(color, pos); break;
            }
        }
        public bool IsInCheck(string color)
        {
            int kingRow = -1, kingCol = -1;
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (grid[r, c] is King && grid[r, c].Color == color)
                    {
                        kingRow = r; kingCol = c;
                    }

            string enemyColor = (color == "White") ? "Black" : "White";
            return IsSquareAttacked(kingRow, kingCol, enemyColor);
        }

        public bool IsSquareAttacked(int row, int col, string attackerColor)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece p = grid[r, c];
                    if (p != null && p.Color == attackerColor)
                    {
                        if (p is Pawn)
                        {
                            int direction = (p.Color == "White") ? -1 : 1;
                            if (Math.Abs(c - col) == 1 && row == r + direction) return true;
                        }
                        else if (p.IsValidMove(r, c, row, col))
                        {
                            if (p is Knight || IsPathClear(r, c, row, col)) return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsCheckmate(string color)
        {
            if (!IsInCheck(color)) return false;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece p = grid[r, c];
                    if (p != null && p.Color == color)
                    {
                        for (int tr = 0; tr < 8; tr++)
                        {
                            for (int tc = 0; tc < 8; tc++)
                            {
                                string fromPos = $"{(char)('a' + c)}{8 - r}";
                                string toPos = $"{(char)('a' + tc)}{8 - tr}";

                                // Simplified simulation of a move
                                if (p.IsValidMove(r, c, tr, tc))
                                {
                                    Piece tempTarget = grid[tr, tc];
                                    grid[tr, tc] = p;
                                    grid[r, c] = null;
                                    bool stillInCheck = IsInCheck(color);
                                    grid[r, c] = p;
                                    grid[tr, tc] = tempTarget;

                                    if (!stillInCheck) return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool IsPathClear(int fromRow, int fromCol, int toRow, int toCol)
        {
            int rowStep = Math.Sign(toRow - fromRow);
            int colStep = Math.Sign(toCol - fromCol);
            int currR = fromRow + rowStep;
            int currC = fromCol + colStep;
            while (currR != toRow || currC != toCol)
            {
                if (grid[currR, currC] != null) return false;
                currR += rowStep;
                currC += colStep;
            }
            return true;
        }
        public bool HasLegalMoves(string color)
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                {
                    Piece p = grid[r, c];
                    if (p != null && p.Color == color)
                    {
                        for (int tr = 0; tr < 8; tr++)
                            for (int tc = 0; tc < 8; tc++)
                            {
                                string from = $"{(char)('a' + c)}{8 - r}";
                                string to = $"{(char)('a' + tc)}{8 - tr}";
                                Piece temp = grid[tr, tc];
                                if (MovePiece(from, to, color))
                                {
                                    grid[r, c] = grid[tr, tc];
                                    grid[tr, tc] = temp;
                                    MoveHistory.RemoveAt(MoveHistory.Count - 1);
                                    return true;
                                }
                            }
                    }
                }
            return false;
        }
        public int Evaluate()
        {
            int score = 0;
            foreach (var p in grid)
            {
                if (p == null) continue;
                int val = p is Pawn ? 10 : p is Knight || p is Bishop ? 30 : p is Rook ? 50 : p is Queen ? 90 : 900;
                score += (p.Color == "White" ? val : -val);
            }
            return score;
        }
        public void Display()
        {
            Console.WriteLine("  a b c d e f g h");
            for (int r = 0; r < 8; r++)
            {
                Console.Write((8 - r) + " ");
                for (int c = 0; c < 8; c++)
                {
                    if (grid[r, c] == null) Console.Write(". ");
                    else
                    {
                        string name = grid[r, c].GetType().Name;
                        char letter = (name == "Knight") ? 'N' : name[0];
                        Console.Write(letter + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}