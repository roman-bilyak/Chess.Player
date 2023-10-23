using Chess.Player.Data;

namespace Chess.Player.MAUI.Services;

internal class PlayerHistoryService : IPlayerHistoryService
{
    private const int MaxCount = 5;

    private readonly ICacheManager _cacheManager;
    private List<string> _players;

    public PlayerHistoryService
    (
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(cacheManager);

        _cacheManager = cacheManager;
    }

    public async Task AddAsync(string name, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        _players.RemoveAll(x => x.Equals(name));
        _players.Insert(0, name);

        while (_players.Count > MaxCount)
        {
            _players.RemoveAt(_players.Count - 1);
        }

        await SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetAllAsync(CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        return _players.AsReadOnly();
    }

    #region helper methods

    private async Task EnsureLoadedAsync(CancellationToken cancellationToken)
    {
        _players ??= await _cacheManager.GetOrAddAsync("PlayerHistory", "Root", () => Task.FromResult(new List<string>()), false, cancellationToken);
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.GetOrAddAsync("PlayerHistory", "Root", () => Task.FromResult(_players), true, cancellationToken);
    }

    #endregion
}
