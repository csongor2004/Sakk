using Sakk.Pieces;

namespace sakk_test
{
    public class UnitTest1
    {
        [Fact]
        public void Rook_CannotMoveDiagonally()
        {
            var rook = new Rook("White", "a1");
            bool isValid = rook.IsValidMove(7, 0, 5, 2);
            Assert.False(isValid);
        }
    }
}
