namespace MAUICheckersGame.MVVM.Models
{
    public class Piece
    {
        #region Properties
        public PieceType Type { get; set; }
        public PieceColor Color { get; set; }
        public bool IsKing => Type == PieceType.King;
        #endregion

        #region Constructors
        public Piece(PieceType type, PieceColor color)
        {
            Type = type;
            Color = color;
        }
        #endregion

        #region Methods
        // Move() - Method to Move a Piece
        public bool Move(Square startSquare, Square endSquare, Board board)
        {
            if (IsValidMove(startSquare, endSquare, board))
            {
                bool isCapture = Math.Abs(endSquare.Row - startSquare.Row) == 2;

                // Check if the move is a capture
                if (isCapture)
                {
                    // Find the piece that was captured
                    int capturedRow = (startSquare.Row + endSquare.Row) / 2;
                    int capturedCol = (startSquare.Col + endSquare.Col) / 2;
                    Square capturedSquare = board.Squares[capturedRow][capturedCol];

                    // Remove the captured piece from the board
                    capturedSquare.CurrentPiece = null;
                }

                // Move the piece to the new square
                endSquare.CurrentPiece = this;
                startSquare.CurrentPiece = null;

                // Promote to King if reaching the opposite end
                if ((Color == PieceColor.Black && endSquare.Row == 7) ||
                    (Color == PieceColor.White && endSquare.Row == 0))
                {
                    PromoteToKing();
                }

                // Check for further captures if this was a capture move
                if (isCapture && HasFurtherCaptures(endSquare, board))
                {
                    // Allow the player to continue capturing with the same piece
                    return false; // Return false to indicate the move isn't finalized for turn switching
                }

                return true;
            }
            return false;
        }


        // IsValidMove() - Method to Check if Move is Valid
        public bool IsValidMove(Square startSquare, Square endSquare, Board board)
        {
            // Only allow moves to the empty squares
            if (endSquare.CurrentPiece != null)
                return false;

            int rowDifference = endSquare.Row - startSquare.Row;
            int colDifferent = endSquare.Col - startSquare.Col;

            // Regular pieces can only move forward one square diagonally
            if (Type == PieceType.Regular)
            {
                // White pieces move up (negative row difference)
                // Black pieces move down (positive row difference)
                if ((Color == PieceColor.White && rowDifference >= 0) ||
                (Color == PieceColor.Black && rowDifference <= 0))
                {
                    return false;
                }
            }

            // Regular moves (one square diagonally)
            if (Math.Abs(rowDifference) == 1 && Math.Abs(colDifferent) == 1)
            {
                return true;
            }
            // Capture moves (two squares diagonally with an opponent's piece in between)
            else if (Math.Abs(rowDifference) == 2 && Math.Abs(colDifferent) == 2)
            {
                int midRow = (startSquare.Row + endSquare.Row) / 2;
                int midCol = (startSquare.Col + endSquare.Col) / 2;
                var middleSquare = board.Squares[midRow][midCol];

                // Check if there is an opponent's piece to capture
                if (middleSquare.CurrentPiece != null && middleSquare.CurrentPiece.Color != Color)
                {
                    return true;
                }
            }

            // Kings can move one square in any diagonal direction
            return colDifferent == 1 && Math.Abs(rowDifference) == 1;
        }

        // PromoteToKing() - Method to Promote to King
        public void PromoteToKing()
        {
            Type = PieceType.King;
        }

        // Helper method to check for further captures
        private bool HasFurtherCaptures(Square currentSquare, Board board)
        {
            // Check all possible capture directions for more captures
            int[][] directions = { new[] { 2 , 2 }, new[] { 2, -2 }, new[] { -2, 2 }, new[] { -2, -2 } };

            foreach (var direction in directions)
            {
                int newRow = currentSquare.Row + direction[0];
                int newCol = currentSquare.Col + direction[1];
                if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                {
                    var targetSquare = board.Squares[newRow][newCol];
                    if (IsValidMove(currentSquare, targetSquare, board))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
