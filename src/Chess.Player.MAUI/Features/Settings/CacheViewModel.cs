using Chess.Player.Cache;
using Chess.Player.Data;
using Chess.Player.MAUI.Features;
using Chess.Player.MAUI.Features.Favorites;
using Chess.Player.MAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Chess.Player.MAUI.ViewModels;

[INotifyPropertyChanged]
public partial class CacheViewModel : BaseViewModel
{
    private readonly IPlayerFavoriteService _playerFavoriteService;
    private readonly IPlayerHistoryService _playerHistoryService;
    private readonly ICacheManager _cacheManager;

    public CacheViewModel
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
    private async Task ClearHistoryAsync(CancellationToken cancellationToken)
    {
        await _playerHistoryService.ClearAsync(cancellationToken);
    }

    [RelayCommand]
    private async Task ClearUserDataAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.ClearAllAsync<PlayerGroupInfo>(cancellationToken);
    }

    [RelayCommand]
    private async Task ClearSyncDataAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.ClearAllAsync<PlayerFullInfo>(cancellationToken);
        await _cacheManager.ClearAllAsync<PlayerTournamentList>(cancellationToken);
        await _cacheManager.ClearAllAsync<TournamentInfo>(cancellationToken);
        await _cacheManager.ClearAllAsync<PlayerInfo>(cancellationToken);
    }

    [RelayCommand]
    private async Task ClearAllAsync(CancellationToken cancellationToken)
    {
        await ClearFavoritesAsync(cancellationToken);
        await ClearHistoryAsync(cancellationToken);
        await ClearUserDataAsync(cancellationToken);
        await ClearSyncDataAsync(cancellationToken);
    }
}