namespace Sakk.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(string color, string position) : base(color, position)
        {
        }

        public override void Move(string newPosition)
        {
            // Egyszeru ellenorzes: a gyalog csak elore lephet (itt most csak a poziciot frissitjuk)
            Position = newPosition;
            System.Console.WriteLine("Move registered for Pawn: " + Position);
        }

        // Uj metodus a szabalyok ellenorzesehez
        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            int direction = (Color == "White") ? -1 : 1;

            // Alapeset: 1-et lep elore ugyanabban az oszlopban
            if (fromCol == toCol && toRow == fromRow + direction)
            {
                return true;
            }
            return false;
        }
    }
}