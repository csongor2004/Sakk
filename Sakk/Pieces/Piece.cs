namespace Sakk.Pieces
{
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

        // Ez a metodus kenyszeriti ki a babuk egyedi lepesszabalyait
        public abstract bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol);
    }
}