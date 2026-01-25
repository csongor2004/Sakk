namespace Sakk
{
	// Absztrakt alaposztály minden bábuhoz
	public abstract class Piece
	{
		public string Color { get; set; }
		public string Position { get; set; }

		protected Piece(string color, string position)
		{
			Color = color;
			Position = position;
		}

		public abstract void Move(string newPosition);
	}
}