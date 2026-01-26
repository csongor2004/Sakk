namespace Sakk.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(string color, string position) : base(color, position)
        {
        }

        public override void Move(string newPosition)
        {
            Position = newPosition;
            System.Console.WriteLine("Bishop moved to: " + Position);
        }

        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            // Atlos mozgas: a sor- es oszlopkulonbseg megegyezik
            return System.Math.Abs(fromRow - toRow) == System.Math.Abs(fromCol - toCol);
        }
    }
}