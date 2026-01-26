namespace Sakk.Pieces
{
    public class Rook : Piece
    {
        public Rook(string color, string position) : base(color, position)
        {
        }

        public override void Move(string newPosition)
        {
            Position = newPosition;
            System.Console.WriteLine("Rook moved to: " + Position);
        }

         public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            // A bastya csak akkor lephet, ha vagy a sor, vagy az oszlop megegyezik
            return fromRow == toRow || fromCol == toCol;
        }
    }
}