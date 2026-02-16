namespace Sakk.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(string color, string position) : base(color, position)
        {
        }

        public override void Move(string newPosition)
        {
            
            Position = newPosition;
            System.Console.WriteLine("Move registered for Pawn: " + Position);
        }


        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            int direction = (Color == "White") ? -1 : 1;
            int startRow = (Color == "White") ? 6 : 1;
            int deltaRow = toRow - fromRow;
            int deltaCol = Math.Abs(toCol - fromCol);

            if (deltaCol == 0)
            {
                if (deltaRow == direction) return true;
                if (fromRow == startRow && deltaRow == 2 * direction) return true;
            }
            else if (deltaCol == 1 && deltaRow == direction)
            {
                return true;
            }

            return false;
        }
    }
}