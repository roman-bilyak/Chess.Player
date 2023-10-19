using Chess.Player.MAUI.Pages;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class SearchViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _searchText;

    [ObservableProperty]
    private ObservableCollection<RecentPlayerViewModel> _recentPlayers = new();

    [ObservableProperty]
    private RecentPlayerViewModel _selectedPlayer;

    public SearchViewModel
    (
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(navigationService);

        _navigationService = navigationService;
    }

    [RelayCommand]
    private void Load()
    {
        RecentPlayers.Clear();
        RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Kravtsiv", FirstName = "Martyn", Title = "GM", ClubCity = "Ukraine, Львів", YearOfBirth = 1990 });
        RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Кравців", FirstName = "Мартин", Title = "GM", YearOfBirth = 1990 });
        RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Dubnevych", FirstName = "Maksym", Title = "FM", ClubCity = "КЗ ДЮСШ Дебют (Грабінський В.)", YearOfBirth = 2009 });
        RecentPlayers.Add(new RecentPlayerViewModel { LastName = "Коць", FirstName = "Святослав", Title = "3", ClubCity = "Городоцька ДЮСШ (Мелешко В.)", YearOfBirth = 2016 });
    }

    [RelayCommand]
    private async Task GoAsync()
    {
        await NavigateToPlayerViewAsync(SearchText);
    }

    [RelayCommand]
    private async Task ItemSelectedAsync(RecentPlayerViewModel selectedPlayer)
    {
        if (selectedPlayer == null)
        {
            return;
        }
        await NavigateToPlayerViewAsync($"{selectedPlayer.LastName} {selectedPlayer.FirstName}");
    }

    private async Task NavigateToPlayerViewAsync(string name)
    {
        await _navigationService.PushAsync<PlayerPage, PlayerViewModel>(x =>
        {
            x.SearchCriterias.Add(name);
        });

        SelectedPlayer = null;
    }
}