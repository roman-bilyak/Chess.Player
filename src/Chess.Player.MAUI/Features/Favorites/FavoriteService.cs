using Chess.Player.Cache;
using Chess.Player.Data;
using System.Diagnostics.CodeAnalysis;

namespace Chess.Player.MAUI.Features.Favorites;

internal class FavoriteService : IFavoriteService
{
    private readonly IChessDataService _chessDataService;
    private readonly ICacheManager _cacheManager;

    private FavoriteList? _favoriteList;

    public event ProgressEventHandler? ProgressChanged;

    public FavoriteService
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

        _favoriteList.Add(name);

        await SaveAsync(cancellationToken);
    }

    public async Task RemoveAsync(string name, CancellationToken cancellationToken)
    {
        await EnsureLoadedAsync(cancellationToken);

        _favoriteList.Remove(name);

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

        return _favoriteList.Contains(name);
    }

    public async Task<IReadOnlyList<PlayerFullInfo>> GetAllAsync(bool useCache, CancellationToken cancellationToken)
    {
        OnProgressChanged(ProgressHelper.Start);

        await EnsureLoadedAsync(cancellationToken);

        int index = 1;
        List<PlayerFullInfo> result = [];
        foreach (var player in _favoriteList)
        {
            PlayerFullInfo playerInfo = await _chessDataService.GetPlayerFullInfoAsync(player, useCache, cancellationToken);
            result.Add(playerInfo);

            double progressPercentage = ProgressHelper.GetProgress(index++, _favoriteList.Count);
            OnProgressChanged(progressPercentage);
        }

        OnProgressChanged(ProgressHelper.Finish);

        return result.OrderBy(x => x.Names.FirstOrDefault()?.FullName).ToList();
    }

    public async Task ClearAsync(CancellationToken cancellationToken)
    {
        _favoriteList = [];

        await SaveAsync(cancellationToken);
    }

    #region helper methods

    [MemberNotNull(nameof(_favoriteList))]
    private async Task EnsureLoadedAsync(CancellationToken cancellationToken)
    {
        _favoriteList ??= await _cacheManager.GetAsync<FavoriteList>(includeExpired: false, cancellationToken) ?? [];
    }

    protected virtual void OnProgressChanged(double percentage)
    {
        ProgressChanged?.Invoke(this, new ProgressEventArgs(percentage));
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _cacheManager.AddAsync(_favoriteList, expirationDate: null, cancellationToken);
    }

    #endregion
}