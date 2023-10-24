using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class FavoritesViewModel : BaseViewModel
{
    private readonly IPlayerFavoriteService _playerFavoriteService;

    [ObservableProperty]
    private PlayerCardListViewModel _playerCardList;

    public FavoritesViewModel
    (
        IPlayerFavoriteService playerFavoriteService,
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(playerFavoriteService);
        ArgumentNullException.ThrowIfNull(navigationService);

        _playerFavoriteService = playerFavoriteService;

        _playerCardList = new(navigationService);
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        PlayerCardList.Players.Clear();

        foreach (string player in await _playerFavoriteService.GetAllAsync(cancellationToken))
        {
            PlayerCardList.Players.Add(new PlayerCardViewModel 
            { 
                LastName = player.Split(" ").FirstOrDefault(),
                FirstName = player.Split(" ").Skip(1).FirstOrDefault(),
            });
        }
    }
}