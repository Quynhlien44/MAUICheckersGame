using MAUICheckersGame.MVVM.Views;

namespace MAUICheckersGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new CheckersGame();
        }
    }
}
