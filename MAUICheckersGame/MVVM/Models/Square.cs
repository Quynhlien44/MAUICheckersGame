using System;
namespace MAUICheckersGame.MVVM.Models
{
    public class Square
    {
        #region Properties
        public int Row { get; set; }
        public int Col { get; set; }
        public Piece? CurrentPiece { get; set; }
        public bool IsBlackSquare => (Row + Col) % 2 != 0;
        public bool IsSelected { get; set; } = false;
        public bool IsValidMoveSquare { get; set; } = false;
        public bool IsCaptureMoveSquare { get; set; } = false;
        #endregion

        #region Constructors
        public Square(int row, int col, Piece? piece)
        {
            Row = row;
            Col = col;
            CurrentPiece = piece;
        }
        #endregion
    }
}
