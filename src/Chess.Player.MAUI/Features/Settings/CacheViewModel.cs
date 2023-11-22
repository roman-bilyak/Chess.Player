using Chess.Player.Cache;
using Chess.Player.Data;
using Chess.Player.MAUI.Features.Favorites;
using Chess.Player.MAUI.Features.Home;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Chess.Player.MAUI.Features.Settings;

[INotifyPropertyChanged]
public partial class CacheViewModel : BaseViewModel
{
    private readonly IFavoriteService _favoriteService;
    private readonly IHistoryService _historyService;
    private readonly ICacheManager _cacheManager;

    public CacheViewModel
    (
        IFavoriteService favoriteService,
        IHistoryService historyService,
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(favoriteService);
        ArgumentNullException.ThrowIfNull(historyService);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _favoriteService = favoriteService;
        _historyService = historyService;
        _cacheManager = cacheManager;
    }

    [RelayCommand]
    private async Task ClearFavoritesAsync(CancellationToken cancellationToken)
    {
        await _favoriteService.ClearAsync(cancellationToken);
    }

    [RelayCommand]
    private async Task ClearHistoryAsync(CancellationToken cancellationToken)
    {
        await _historyService.ClearAsync(cancellationToken);
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