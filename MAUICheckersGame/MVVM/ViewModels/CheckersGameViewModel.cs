using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUICheckersGame.MVVM.Models;

namespace MAUICheckersGame.MVVM.ViewModels
{
    public partial class CheckersGameViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "Checkers Game";

        [ObservableProperty]
        private Board _gameBoard;

        [ObservableProperty]
        private string _currentTurnDisplay;

        [ObservableProperty]
        private bool _isGameOver;

        [ObservableProperty]
        private string _gameOverMessage;


        // Track selected piece and square
        public Square? SelectedSquare { get; set; }
        public Game GameInstance { get; set; }

        public CheckersGameViewModel()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            GameInstance = new Game();
            GameBoard = new Board();
            IsGameOver = false;
            GameOverMessage = string.Empty;
            CurrentTurnDisplay = GameInstance.CurrentTurnDisplay;
            GameBoard.InitializeBoard(); // ensure Board is reset
            UpdateGameState();
            Console.WriteLine("Start a new game");
        }

        public void UpdateTurnDisplay()
        {
            CurrentTurnDisplay = GameInstance.CurrentTurnDisplay;
            UpdateGameState();
        }

        public void UpdateGameState()
        {
            GameInstance.CheckForEndGame();

            if (GameInstance.IsGameOver)
            {
                IsGameOver = true;
                GameOverMessage = GameInstance.GameOverMessage;
                CurrentTurnDisplay = GameOverMessage;
                Console.WriteLine("Game Over: {0}", GameOverMessage);
            }
            else
            {
                IsGameOver = false;
                GameOverMessage = string.Empty;
                CurrentTurnDisplay = GameInstance.CurrentTurnDisplay;
            }
            Console.WriteLine("IsGameOver Flag: {0}, GameOverMessage: {1}", IsGameOver, GameOverMessage);
            OnPropertyChanged(nameof(IsGameOver));
            OnPropertyChanged(nameof(GameOverMessage));
            OnPropertyChanged(nameof(GameBoard));
        }

        [RelayCommand]
        private void PlayAgain()
        {
            InitializeGame();
            GameBoard.InitializeBoard();
            // Trigger board refresh
            OnPropertyChanged(nameof(GameBoard));
        }
    }
}

// Game over is not implemented successfully in the CheckersGameViewModel.
