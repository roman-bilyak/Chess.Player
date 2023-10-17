using Chess.Player.MAUI.ViewModels;
using Chess.Player.MAUI.Views;

namespace Chess.Player.MAUI
{
    public partial class App : Application
    {
        public App(SearchViewModel searchViewModel)
        {
            InitializeComponent();

            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Kravtsiv", FirstName = "Martyn", Title = "GM", ClubCity = "Urnaine, Львів", YearOfBirth = 1990 });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Кравців", FirstName = "Мартин", Title = "GM", YearOfBirth = 1990 });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Dubnevych", FirstName = "Maksym", Title = "FM", ClubCity = "КЗ ДЮСШ Дебют (Грабінський В.)", YearOfBirth = 2009 });
            searchViewModel.RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Коць", FirstName = "Святослав", Title = "3", ClubCity = "Городоцька ДЮСШ (Мелешко В.)", YearOfBirth = 2016 });

            MainPage = new NavigationPage(new SearchView { BindingContext = searchViewModel });
        }
    }
}