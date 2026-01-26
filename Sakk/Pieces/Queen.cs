namespace Sakk.Pieces
{
    public class Queen : Piece
    {
        public Queen(string color, string position) : base(color, position)
        {
        }

        public override void Move(string newPosition)
        {
            Position = newPosition;
            System.Console.WriteLine("Queen moved to: " + Position);
        }

        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            // A vezer vagy ugy mozog mint egy bastya, vagy mint egy futo
            bool rookLike = (fromRow == toRow || fromCol == toCol);
            bool bishopLike = (System.Math.Abs(fromRow - toRow) == System.Math.Abs(fromCol - toCol));

            return rookLike || bishopLike;
        }
    }
}