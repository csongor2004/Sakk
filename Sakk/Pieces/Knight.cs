namespace Sakk.Pieces
{
	public class Knight : Piece
	{
		public Knight(string color, string position) : base(color, position)
		{
		}

		public override void Move(string newPosition)
		{
			// A huszár "L" alakban ugrik, itt kesobb ellenorizzuk a szabalyt.
			Position = newPosition;
			System.Console.WriteLine("A huszar az uj helyere ugrott: " + Position);
		}
        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            int deltaRow = System.Math.Abs(fromRow - toRow);
            int deltaCol = System.Math.Abs(fromCol - toCol);
            return (deltaRow == 2 && deltaCol == 1) || (deltaRow == 1 && deltaCol == 2);
        }
    }
}