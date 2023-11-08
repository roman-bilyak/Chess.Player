using Chess.Player.Cache;

namespace Chess.Player.Data;

internal class PlayerGroupService : IPlayerGroupService
{
    private readonly IChessDataNormalizer _chessDataNormalizer;
    private readonly ICacheManager _cacheManager;

    public PlayerGroupService
    (
        IChessDataNormalizer chessDataNormalizer,
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataNormalizer);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _chessDataNormalizer = chessDataNormalizer;
        _cacheManager = cacheManager;
    }

    public async Task AddToGroupAsync(string groupName, string name, CancellationToken cancellationToken)
    {
        PlayerGroupInfo playerGroupInfo = await GetGroupInfoAsync(groupName, cancellationToken);

        name = _chessDataNormalizer.NormalizeName(name);
        if (!string.IsNullOrEmpty(name) && !playerGroupInfo.Contains(name))
        {
            playerGroupInfo.Add(name);

            await _cacheManager.AddAsync(groupName, playerGroupInfo, cancellationToken);
        }
    }

    public async Task<PlayerGroupInfo> GetGroupInfoAsync(string name, CancellationToken cancellationToken)
    {
        name = _chessDataNormalizer.NormalizeName(name);

        return await _cacheManager.GetAsync<PlayerGroupInfo>(name, cancellationToken) ?? new PlayerGroupInfo(name);
    }
}