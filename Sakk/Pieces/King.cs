namespace Sakk.Pieces
{
    public class King : Piece
    {
        public King(string color, string position) : base(color, position)
        {
        }

        public override void Move(string newPosition)
        {
            Position = newPosition;
            System.Console.WriteLine("King moved to: " + Position);
        }

        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            // Max 1 mezo tavolsag minden iranyban
            return System.Math.Abs(fromRow - toRow) <= 1 && System.Math.Abs(fromCol - toCol) <= 1;
        }
    }
}