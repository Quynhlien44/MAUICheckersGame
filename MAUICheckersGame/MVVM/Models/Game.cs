namespace MAUICheckersGame.MVVM.Models
{
    public class Game
    {
        #region Properties
        public Board Board { get; set; }
        public Player BlackPlayer { get; set; }
        public Player WhitePlayer { get; set; }
        public Player CurrentTurn { get; private set; }
        public string CurrentTurnDisplay => $"{CurrentTurn.Color}'s Turn";
        public string GameOverMessage { get; private set; }
        public bool IsGameOver { get; private set; }
        #endregion

        #region Constructors
        public Game()
        {
            Board = new Board();
            BlackPlayer = new Player(PieceColor.Black);
            WhitePlayer = new Player(PieceColor.White);
            CurrentTurn = WhitePlayer;
            GameOverMessage = string.Empty;
            IsGameOver = false;
        }
        #endregion

        #region Methods
        // Check if the current turn is valid
        public bool IsValidTurn(PieceColor pieceColor)
        {
            return CurrentTurn.Color == pieceColor;
        }

        // Switch the turn to another player
        public void SwitchTurn()
        {
            CurrentTurn = (CurrentTurn == WhitePlayer) ? BlackPlayer : WhitePlayer;
        }

        // Check if the game has ended
        public void CheckForEndGame()
        {
            int blackPieces = 0, whitePieces = 0;
            bool blackHasValidMoves = false, whiteHasValidMoves = false;

            foreach (var row in Board.Squares)
            {
                foreach (var square in row)
                {
                    var piece = square.CurrentPiece;
                    if (piece != null)
                    {
                        // Count pieces by color
                        if (piece.Color == PieceColor.Black) blackPieces++;
                        else if (piece.Color == PieceColor.White) whitePieces++;
                    }
                }
            }

            // Now check for valid moves after counting pieces
            foreach (var row in Board.Squares)
            {
                foreach (var square in row)
                {
                    var piece = square.CurrentPiece;
                    if (piece != null)
                    {
                        // Check for valid moves
                        if (piece.Color == PieceColor.Black && !blackHasValidMoves)
                        {
                            blackHasValidMoves = HasValidMoves(square);
                        }
                        else if (piece.Color == PieceColor.White && !whiteHasValidMoves)
                        {
                            whiteHasValidMoves = HasValidMoves(square);
                        }
                    }
                }
            }

            // Set game-over conditions based on piece count and valid moves
            if (blackPieces == 0 || !blackHasValidMoves)
            {
                IsGameOver = true;
                GameOverMessage = "White Wins!";
            }
            else if (whitePieces == 0 || !whiteHasValidMoves)
            {
                IsGameOver = true;
                GameOverMessage = "Black Wins!";
            }
            else
            {
                IsGameOver = false;
                GameOverMessage = string.Empty;
            }
        }


        private bool HasValidMoves(Square pieceSquare)
        {
            var piece = pieceSquare.CurrentPiece;
            if (piece == null) return false;

            int[][] directions = piece.IsKing
                ? new[] { new[] { -1, -1 }, new[] { -1, 1 }, new[] { 1, -1 }, new[] { 1, 1 } } // King moves
                : piece.Color == PieceColor.Black
                    ? new[] { new[] { 1, -1 }, new[] { 1, 1 } } // Black moves down
                    : new[] { new[] { -1, -1 }, new[] { -1, 1 } }; // White moves up

            foreach (var direction in directions)
            {
                int targetRow = pieceSquare.Row + direction[0];
                int targetCol = pieceSquare.Col + direction[1];

                if (targetRow >= 0 && targetRow < 8 && targetCol >= 0 && targetCol < 8)
                {
                    var targetSquare = Board.Squares[targetRow][targetCol];
                    if (piece.IsValidMove(pieceSquare, targetSquare, Board))
                        return true;
                }
            }
            return false;
        }

    }
    #endregion
}
