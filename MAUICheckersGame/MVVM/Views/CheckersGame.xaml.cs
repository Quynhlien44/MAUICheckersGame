using MAUICheckersGame.MVVM.Models;
using MAUICheckersGame.MVVM.ViewModels;
namespace MAUICheckersGame.MVVM.Views
{
    public partial class CheckersGame : ContentPage
    {
        private Square? selectedSquare = null;
        private Square? destinationSquare = null;

        public CheckersGame()
        {
            InitializeComponent();
            PopulateBoard();
        }

        private void PopulateBoard()
        {
            var viewModel = (CheckersGameViewModel)BindingContext;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var square = viewModel.GameBoard.Squares[row][col];

                    // Create square background
                    var squareFrame = new Frame
                    {
                        Padding = 0,
                        HasShadow = false,
                        BorderColor = Colors.Transparent,
                        BackgroundColor = square.IsSelected ? Colors.Yellow :
                                        square.IsCaptureMoveSquare ? Colors.Red :
                                        square.IsValidMoveSquare ? Colors.LightGreen :
                                        (square.IsBlackSquare ? Colors.DarkGray : Colors.LightGray)
                    };

                    if (square.CurrentPiece != null)
                    {
                        // Create piece
                        var pieceFrame = new Frame
                        {
                            HeightRequest = 40,
                            WidthRequest = 40,
                            CornerRadius = 20,
                            HasShadow = false,
                            Padding = 0,
                            BackgroundColor = square.CurrentPiece.Color == PieceColor.Black ?
                                Colors.Black : Colors.White,
                            BorderColor = Colors.DarkGray,
                            Content = square.CurrentPiece.Type == PieceType.King ?
                                new Label
                                {
                                    Text = "♔",
                                    TextColor = square.CurrentPiece.Color == PieceColor.Black ?
                                        Colors.White : Colors.Black,
                                    HorizontalOptions = LayoutOptions.Center,
                                    VerticalOptions = LayoutOptions.Center
                                } : null,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        };

                        squareFrame.Content = pieceFrame;
                    }

                    // Add TapGestureRecognizer for each square
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => OnSquareTapped(square);
                    squareFrame.GestureRecognizers.Add(tapGestureRecognizer);

                    // Add to grid
                    BoardGrid.Add(squareFrame, col, row);
                }
            }
        }

        private void OnSquareTapped(Square tappedSquare)
        {
            var viewModel = (CheckersGameViewModel)BindingContext;

            // First tap - select a piece
            if (selectedSquare == null && tappedSquare.CurrentPiece != null)
            {
                // Only allow selection of pieces that match the current player's turn
                if (!viewModel.GameInstance.IsValidTurn(tappedSquare.CurrentPiece.Color))
                    return;

                // Ensure only the correct color can be selected (alternate turns, if needed)
                selectedSquare = tappedSquare;
                tappedSquare.IsSelected = true;
                HighlightValidMoves(selectedSquare);
            }
            // Second tap - attempt to move
            else if (selectedSquare != null && tappedSquare != selectedSquare)
            {
                destinationSquare = tappedSquare;

                if (selectedSquare.CurrentPiece?.Move(selectedSquare, destinationSquare, viewModel.GameBoard) == true)
                {
                    // Switch turns after a valid move
                    viewModel.GameInstance.CheckForEndGame();
                    viewModel.UpdateGameState();
                    if (!viewModel.GameInstance.IsGameOver)
                    {
                        viewModel.GameInstance.SwitchTurn();
                        viewModel.UpdateTurnDisplay();
                    }
                    PopulateBoard();
                }

                // Reset selection and valid move highlights
                ResetHighLights();
            }

        }

        private void HighlightValidMoves(Square selectedSquare)
        {
            var viewModel = (CheckersGameViewModel)BindingContext;
            foreach (var row in viewModel.GameBoard.Squares)
            {
                foreach (var square in row)
                {
                    if (selectedSquare.CurrentPiece?.IsValidMove(selectedSquare, square, viewModel.GameBoard) == true)
                    {
                        // Check if it is a capture move
                        int rowDiff = Math.Abs(square.Row - selectedSquare.Row);
                        square.IsValidMoveSquare = true;
                        square.IsCaptureMoveSquare = rowDiff == 2;
                    }
                }
            }
            PopulateBoard();
        }

        private void ResetHighLights()
        {
            var viewModel = (CheckersGameViewModel)BindingContext;
            foreach (var row in viewModel.GameBoard.Squares)
            {
                foreach (var square in row)
                {
                    square.IsSelected = false;
                    square.IsValidMoveSquare = false;
                    square.IsCaptureMoveSquare = false;
                }
            }
            selectedSquare = null;
            destinationSquare = null;
            PopulateBoard();
        }
    }
}