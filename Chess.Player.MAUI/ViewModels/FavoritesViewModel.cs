using Chess.Player.Data;
using Chess.Player.MAUI.Services;
using Chess.Player.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class FavoritesViewModel : BaseViewModel
{
    private readonly IPlayerFavoriteService _playerFavoriteService;

    [ObservableProperty]
    private PlayerCardListViewModel _playerCardList;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private bool _forceRefresh;

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
    private Task StartAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;
        ForceRefresh = false;

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task RefreshAsync(CancellationToken cancellationToken)
    {
        ForceRefresh = true;
        IsLoading = true;

        return Task.CompletedTask;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            PlayerCardList.Players.Clear();

            foreach (PlayerFullInfo player in await _playerFavoriteService.GetAllAsync(ForceRefresh, cancellationToken))
            {
                PlayerCardViewModel playerCardViewModel = new()
                {
                    Names = new ObservableCollection<NameViewModel>(player.Names.Select(x=> new NameViewModel { LastName = x.LastName, FirstName = x.FirstName })),
                    Title = player.Title,
                    ClubCity = player.ClubCity,
                    YearOfBirth = player.YearOfBirth,
                };
                PlayerCardList.Players.Add(playerCardViewModel);
            }
        }
        finally
        { 
            IsLoading = false;
            ForceRefresh = false;
        }
    }
}