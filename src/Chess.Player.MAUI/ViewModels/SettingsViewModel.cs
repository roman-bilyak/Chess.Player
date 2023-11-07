using Chess.Player.Cache;
using Chess.Player.Data;
using Chess.Player.MAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class SettingsViewModel : BaseViewModel
{
    private readonly IPlayerFavoriteService _playerFavoriteService;
    private readonly IPlayerHistoryService _playerHistoryService;
    private readonly ICacheManager _cacheManager;

    [ObservableProperty]
    private PlayerListViewModel _playerCardList;

    public SettingsViewModel
    (
        IPlayerFavoriteService playerFavoriteService,
        IPlayerHistoryService playerHistoryService,
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(playerFavoriteService);
        ArgumentNullException.ThrowIfNull(playerHistoryService);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _playerFavoriteService = playerFavoriteService;
        _playerHistoryService = playerHistoryService;
        _cacheManager = cacheManager;
    }

    [RelayCommand]
    private async Task ClearFavoritesAsync(CancellationToken cancellationToken)
    {
        await _playerFavoriteService.ClearAsync(cancellationToken);
    }

    [RelayCommand]
    private async Task ClearViewHistoryAsync(CancellationToken cancellationToken)
    {
        await _playerHistoryService.ClearAsync(cancellationToken);
    }

    [RelayCommand]
    private async Task ClearUserDataAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.ClearAsync<PlayerGroupInfo>(cancellationToken);
    }

    [RelayCommand]
    private async Task ClearCacheDataAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.ClearAsync<PlayerFullInfo>(cancellationToken);
        await _cacheManager.ClearAsync<TournamentInfo>(cancellationToken);
        await _cacheManager.ClearAsync<PlayerInfo>(cancellationToken);
    }

    [RelayCommand]
    private async Task ClearAllAsync(CancellationToken cancellationToken)
    {
        await ClearFavoritesAsync(cancellationToken);
        await ClearViewHistoryAsync(cancellationToken);
        await ClearUserDataAsync(cancellationToken);
        await ClearCacheDataAsync(cancellationToken);
    }
}