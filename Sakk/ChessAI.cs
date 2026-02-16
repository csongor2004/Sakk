using Sakk.Pieces;
using System;
using System.Collections.Generic;

namespace Sakk
{
    public class ChessAI
    {
        public Board board;
        public string color;

        public ChessAI(Board board, string color)
        {
            this.board = board;
            this.color = color;
        }

        public (string from, string to) GetBestMove()
        {
            int bestVal = (color == "White") ? int.MinValue : int.MaxValue;
            (string f, string t) bestMove = ("", "");

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (board.grid[r, c]?.Color == color)
                    {
                        for (int tr = 0; tr < 8; tr++)
                        {
                            for (int tc = 0; tc < 8; tc++)
                            {
                                string from = $"{(char)('a' + c)}{8 - r}";
                                string to = $"{(char)('a' + tc)}{8 - tr}";

                                Piece tempTarget = board.grid[tr, tc];
                                Piece attacker = board.grid[r, c];

                                if (board.MovePiece(from, to, color))
                                {
                                    int val = board.Evaluate();
                                    // Undo move manually for simulation
                                    board.grid[r, c] = attacker;
                                    board.grid[tr, tc] = tempTarget;
                                    board.MoveHistory.RemoveAt(board.MoveHistory.Count - 1);

                                    if ((color == "White" && val > bestVal) || (color == "Black" && val < bestVal))
                                    {
                                        bestVal = val;
                                        bestMove = (from, to);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return bestMove;
        }
    }
}