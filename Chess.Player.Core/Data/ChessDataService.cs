﻿namespace Chess.Player.Data;

internal class ChessDataService : IChessDataService
{
    private readonly IChessDataManager _chessDataManager;
    private readonly IPlayerGroupService _playerGroupService;
    private readonly ICacheManager _cacheManager;

    public event SearchProgressEventHandler? ProgressChanged
    {
        add
        {
            _chessDataManager.ProgressChanged += value;
        }
        remove
        {
            _chessDataManager.ProgressChanged -= value;
        }
    }

    public ChessDataService
    (
        IChessDataManager chessDataManager,
        IPlayerGroupService playerGroupService,
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataManager);
        ArgumentNullException.ThrowIfNull(chessDataManager);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _chessDataManager = chessDataManager;
        _playerGroupService = playerGroupService;
        _cacheManager = cacheManager;
    }

    public async Task<PlayerFullInfo> GetPlayerFullInfoAsync(string name, bool forceRefresh, CancellationToken cancellationToken)
    {
        PlayerGroupInfo playerGroupInfo = await _playerGroupService.GetGroupInfoAsync(name, cancellationToken);
        SearchCriteria[] searchCriterias = playerGroupInfo.Select(x => new SearchCriteria(x)).ToArray();

        return await _cacheManager.GetOrAddAsync(nameof(PlayerFullInfo),
                string.Join("_", searchCriterias.Select(x => x.Name)),
                async () => await _chessDataManager.GetPlayerFullInfoAsync(searchCriterias, cancellationToken),
                forceRefresh,
                cancellationToken);
    }
}