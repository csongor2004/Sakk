namespace Sakk
{
    public class Board
    {
        private Piece[,] grid;

        public Board()
        {
            // 8x8-as racs letrehozasa
            grid = new Piece[8, 8];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Teszt jelleggel felrakunk par gyalogot
            for (int i = 0; i < 8; i++)
            {
                grid[1, i] = new Pawn("Black", "B" + i);
                grid[6, i] = new Pawn("White", "W" + i);
            }
        }
        public bool MovePiece(string from, string to)
        {
            int fromCol = from[0] - 'a';
            int fromRow = 8 - (from[1] - '0');
            int toCol = to[0] - 'a';
            int toRow = 8 - (to[1] - '0');

            Piece piece = grid[fromRow, fromCol];

            if (piece != null)
            {
                // Itt hivjuk meg a specifikus szabalyokat
                if (piece is Pawn pawn)
                {
                    if (!pawn.IsValidMove(fromRow, fromCol, toRow, toCol))
                    {
                        System.Console.WriteLine("Invalid move for Pawn!");
                        return false;
                    }
                }

                grid[toRow, toCol] = piece;
                grid[fromRow, fromCol] = null;
                piece.Move(to);
                return true;
            }
            return false;
        }
        public void Display()
        {
            System.Console.WriteLine("  0 1 2 3 4 5 6 7");
            for (int row = 0; row < 8; row++)
            {
                System.Console.Write(row + " ");
                for (int col = 0; col < 8; col++)
                {
                    if (grid[row, col] == null)
                    {
                        System.Console.Write(". ");
                    }
                    else
                    {
                        // Kiirjuk a szin elso betujet (B vagy W)
                        System.Console.Write(grid[row, col].Color[0] + " ");
                    }
                }
                System.Console.WriteLine();
            }
        }
    }
}