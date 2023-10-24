using Chess.Player.Data;

namespace Chess.Player.MAUI.Services;

internal class PlayerFavoriteService : IPlayerFavoriteService
{
    private readonly IChessDataService _chessDataService;
    private readonly ICacheManager _cacheManager;

    private List<string> _players;

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

        _players.Add(name);

        await SaveAsync(cancellationToken);
    }

    public async Task RemoveAsync(string name, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        if (_players.Contains(name))
        {
            _players.Remove(name);
        }

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

        return _players.Contains(name);
    }

    public async Task<IReadOnlyList<PlayerShortInfo>> GetAllAsync(bool forceRefresh, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        List<PlayerShortInfo> result = new();
        foreach(var player in _players)
        {
            PlayerFullInfo playerInfo = await _chessDataService.GetFullPlayerInfoAsync(player, forceRefresh, cancellationToken);
            result.Add(playerInfo);
        }

        return result.OrderBy(x => x.Names.FirstOrDefault()?.FullName).ToList();
    }

    #region helper methods

    private async Task EnsureLoadedAsync(CancellationToken cancellationToken)
    {
        _players ??= await _cacheManager.GetOrAddAsync("Favorites", "Root", () => Task.FromResult(new List<string>()), false, cancellationToken);
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.GetOrAddAsync("Favorites", "Root", () => Task.FromResult(_players), true, cancellationToken);
    }

    #endregion
}