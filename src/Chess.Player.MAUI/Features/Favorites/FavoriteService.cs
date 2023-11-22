using Chess.Player.Cache;
using Chess.Player.Data;
using System.Diagnostics.CodeAnalysis;

namespace Chess.Player.MAUI.Features.Favorites;

internal class PlayerFavoriteService : IPlayerFavoriteService
{
    private readonly IChessDataService _chessDataService;
    private readonly ICacheManager _cacheManager;

    private PlayerFavoriteList? _playerFavoriteList;

    public PlayerFavoriteService
    (
        IChessDataService chessDataService,
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataService);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _chessDataService = chessDataService;
        _cacheManager = cacheManager;
    }

    public async Task AddAsync(string name, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        _playerFavoriteList.Add(name);

        await SaveAsync(cancellationToken);
    }

    public async Task RemoveAsync(string name, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        _playerFavoriteList.Remove(name);

        await SaveAsync(cancellationToken);
    }

    public async Task<bool> ToggleAsync(string name, CancellationToken cancellationToken)
    {
        if (!await ContainsAsync(name, cancellationToken))
        {
            await AddAsync(name, cancellationToken);
            return true;
        }

        await RemoveAsync(name, cancellationToken);
        return false;
    }

    public async Task<bool> ContainsAsync(string name, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        return _playerFavoriteList.Contains(name);
    }

    public async Task<IReadOnlyList<PlayerFullInfo>> GetAllAsync(bool useCache, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        List<PlayerFullInfo> result = [];
        foreach (var player in _playerFavoriteList)
        {
            PlayerFullInfo playerInfo = await _chessDataService.GetPlayerFullInfoAsync(player, useCache, cancellationToken);
            result.Add(playerInfo);
        }

        return result.OrderBy(x => x.Names.FirstOrDefault()?.FullName).ToList();
    }

    public async Task ClearAsync(CancellationToken cancellationToken)
    {
        _playerFavoriteList = [];

        await SaveAsync(cancellationToken);
    }

    #region helper methods

    [MemberNotNull(nameof(_playerFavoriteList))]
    private async Task EnsureLoadedAsync(CancellationToken cancellationToken)
    {
        _playerFavoriteList ??= await _cacheManager.GetAsync<PlayerFavoriteList>(includeExpired: false, cancellationToken) ?? [];
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.AddAsync(_playerFavoriteList, expirationDate: null, cancellationToken);
    }

    #endregion
}