using Chess.Player.MAUI.ViewModels;
using Chess.Player.MAUI.Views;

namespace Chess.Player.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SearchViewModel viewModel = new();
            viewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Poroshenko", FirstName = "Mykola" });
            viewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Stehniy", FirstName = "Oleg" });
            viewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Prystay", FirstName = "Olena" });
            viewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Novak", FirstName = "Iryna" });
            viewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Furta", FirstName = "Mykola" });

            MainPage = new NavigationPage(new SearchView { BindingContext = viewModel });
        }
    }
}