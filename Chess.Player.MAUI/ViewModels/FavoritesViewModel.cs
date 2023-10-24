using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class FavoritesViewModel : BaseViewModel
{
    private readonly IFavoritePlayerService _favoritePlayerService;

    [ObservableProperty]
    private PlayerCardListViewModel _playerCardList;

    public FavoritesViewModel
    (
        IFavoritePlayerService favoritePlayerService,
        INavigationService navigationService
    )
    {
        ArgumentNullException.ThrowIfNull(favoritePlayerService);
        ArgumentNullException.ThrowIfNull(navigationService);

        _favoritePlayerService = favoritePlayerService;

        _playerCardList = new(navigationService);
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        PlayerCardList.Players.Clear();

        foreach (string player in await _favoritePlayerService.GetAllAsync(cancellationToken))
        {
            PlayerCardList.Players.Add(new PlayerCardViewModel 
            { 
                LastName = player.Split(" ").FirstOrDefault(),
                FirstName = player.Split(" ").Skip(1).FirstOrDefault(),
            });
        }
    }
}