namespace Sakk
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
	}
}