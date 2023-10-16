using Chess.Player.MAUI.ViewModels;
using Chess.Player.MAUI.Views;

namespace Chess.Player.MAUI
{
    public partial class App : Application
    {
        public App(SearchViewModel searchViewModel)
        {
            InitializeComponent();

            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Kravtsiv", FirstName = "Martyn" });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Кравців", FirstName = "Мартин" });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Dubnevych", FirstName = "Maksym" });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Коць", FirstName = "Святослав" });

            MainPage = new NavigationPage(new SearchView { BindingContext = searchViewModel });
        }
    }
}