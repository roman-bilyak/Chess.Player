using Chess.Player.MAUI.Pages;
using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class FavoritesViewModel : BaseViewModel
{
    private readonly IFavoritePlayerService _favoritePlayerService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<RecentPlayerViewModel> _recentPlayers = new();

    [ObservableProperty]
    private RecentPlayerViewModel _selectedPlayer;

    public FavoritesViewModel
    (
        IFavoritePlayerService favoritePlayerService,
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(favoritePlayerService);
        ArgumentNullException.ThrowIfNull(navigationService);

        _favoritePlayerService = favoritePlayerService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        RecentPlayers.Clear();

        foreach (string name in await _favoritePlayerService.GetAllAsync(cancellationToken))
        {
            RecentPlayers.Add(new RecentPlayerViewModel { LastName = name });
        }
    }

    [RelayCommand]
    private async Task ItemSelectedAsync(RecentPlayerViewModel selectedPlayer)
    {
        if (selectedPlayer == null)
        {
            return;
        }
        await NavigateToPlayerViewAsync($"{selectedPlayer.FullName}");
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