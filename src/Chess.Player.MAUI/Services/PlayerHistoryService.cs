using Chess.Player.Cache;
using Chess.Player.Data;
using System.Diagnostics.CodeAnalysis;

namespace Chess.Player.MAUI.Services;

internal class PlayerHistoryService : IPlayerHistoryService
{
    private const int MaxCount = 5;

    private readonly IChessDataService _chessDataService;
    private readonly ICacheManager _cacheManager;

    private PlayerHistoryList? _playerHistoryList;

    public PlayerHistoryService
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

        _playerHistoryList.RemoveAll(x => x.Equals(name));
        _playerHistoryList.Insert(0, name);

        while (_playerHistoryList.Count > MaxCount)
        {
            _playerHistoryList.RemoveAt(_playerHistoryList.Count - 1);
        }

        await SaveAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PlayerFullInfo>> GetAllAsync(bool useCache, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        List<PlayerFullInfo> result = [];
        foreach (var player in _playerHistoryList)
        {
            PlayerFullInfo playerInfo = await _chessDataService.GetPlayerFullInfoAsync(player, useCache, cancellationToken);
            result.Add(playerInfo);
        }

        return result;
    }

    public async Task ClearAsync(CancellationToken cancellationToken)
    {
        _playerHistoryList = [];

        await SaveAsync(cancellationToken);
    }

    #region helper methods

    [MemberNotNull(nameof(_playerHistoryList))]
    private async Task EnsureLoadedAsync(CancellationToken cancellationToken)
    {
        _playerHistoryList ??= await _cacheManager.GetAsync<PlayerHistoryList>(includeExpired: false, cancellationToken) ?? [];
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.AddAsync(_playerHistoryList, expirationDate: null, cancellationToken);
    }

    #endregion
}
