namespace MAUICheckersGame.MVVM.Models
{
    public class Player
    {
        #region Properties
        public PieceColor Color { get; set; }
        public List<Piece> Pieces { get; set; }
        #endregion

        #region Constructors
        public Player(PieceColor color)
        {
            Color = color;
            Pieces = new List<Piece>();
        }
        #endregion
    }
}