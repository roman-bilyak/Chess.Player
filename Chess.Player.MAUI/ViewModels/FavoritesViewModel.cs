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
    private ObservableCollection<PlayerCardViewModel> _players = new();

    [ObservableProperty]
    private PlayerCardViewModel _selectedPlayer;

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
        Players.Clear();

        foreach (string player in await _favoritePlayerService.GetAllAsync(cancellationToken))
        {
            Players.Add(new PlayerCardViewModel { LastName = player });
        }
    }

    [RelayCommand]
    private async Task ItemSelectedAsync(PlayerCardViewModel selectedPlayer)
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