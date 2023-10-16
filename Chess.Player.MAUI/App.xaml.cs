using Chess.Player.MAUI.ViewModels;
using Chess.Player.MAUI.Views;

namespace Chess.Player.MAUI
{
    public partial class App : Application
    {
        public App(SearchViewModel searchViewModel)
        {
            InitializeComponent();

            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Poroshenko", FirstName = "Mykola" });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Stehniy", FirstName = "Oleg" });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Prystay", FirstName = "Olena" });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Novak", FirstName = "Iryna" });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Furta", FirstName = "Mykola" });

            MainPage = new NavigationPage(new SearchView { BindingContext = searchViewModel });
        }
    }
}