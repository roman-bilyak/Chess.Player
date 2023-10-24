using Chess.Player.Data;
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

        foreach (PlayerShortInfo player in await _playerFavoriteService.GetAllAsync(cancellationToken))
        {
            PlayerCardViewModel playerCardViewModel = new()
            {
                Title = player.Title,
                ClubCity = player.ClubCity,
                YearOfBirth = player.YearOfBirth,
            };
            foreach (NameInfo nameInfo in player.Names)
            {
                playerCardViewModel.Names.Add(new NameViewModel
                {
                    LastName = nameInfo.LastName,
                    FirstName = nameInfo.FirstName
                });
            }
            PlayerCardList.Players.Add(playerCardViewModel);
        }
    }
}