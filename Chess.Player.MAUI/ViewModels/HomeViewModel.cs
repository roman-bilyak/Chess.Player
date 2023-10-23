using Chess.Player.MAUI.Pages;
using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class HomeViewModel : BaseViewModel
{
    private readonly IPlayerHistoryService _playerHistoryService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _searchText;

    [ObservableProperty]
    private ObservableCollection<RecentPlayerViewModel> _recentPlayers = new();

    [ObservableProperty]
    private RecentPlayerViewModel _selectedPlayer;

    public HomeViewModel
    (
        IPlayerHistoryService playerHistoryService,
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(playerHistoryService);
        ArgumentNullException.ThrowIfNull(navigationService);

        _playerHistoryService = playerHistoryService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        RecentPlayers.Clear();

        foreach(var player in await _playerHistoryService.GetAllAsync(cancellationToken))
        {
            RecentPlayers.Add(new RecentPlayerViewModel { LastName = player });
        }
    }

    [RelayCommand]
    private async Task SearchAsync()
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