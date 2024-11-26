using System.Collections.ObjectModel;
namespace MAUICheckersGame.MVVM.Models
{
    public class Board
    {
        #region Properties
        public ObservableCollection<ObservableCollection<Square>> Squares { get; set; }
        #endregion

        #region Constructors
        public Board()
        {
            Squares = new ObservableCollection<ObservableCollection<Square>>();
            InitializeBoard();
        }
        #endregion

        #region Methods
        public void InitializeBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                var newRow = new ObservableCollection<Square>();
                for (int col = 0; col < 8; col++)
                {
                    Piece? piece = null;

                    // Place black pieces in the first three rows on dark squares
                    if (row < 3 && (row + col) % 2 != 0)
                    {
                        piece = new Piece(PieceType.Regular, PieceColor.Black);
                    }
                    // Place white pieces in the last three rows on dark squares
                    else if (row > 4 && (row + col) % 2 != 0)
                    {
                        piece = new Piece(PieceType.Regular, PieceColor.White);
                    }

                    newRow.Add(new Square(row, col, piece));
                }
                Squares.Add(newRow);
            }
        }
        #endregion
    }
}