using Chess.Player.Cache;

namespace Chess.Player.Data;

internal class ChessDataService : IChessDataService
{
    private readonly IChessDataManager _chessDataManager;
    private readonly IPlayerGroupService _playerGroupService;
    private readonly IDateTimeProvider _dateTimeProvider;
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
        IDateTimeProvider dateTimeProvider,
        ICacheManager cacheManager
    )
    {
        ArgumentNullException.ThrowIfNull(chessDataManager);
        ArgumentNullException.ThrowIfNull(playerGroupService);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _chessDataManager = chessDataManager;
        _playerGroupService = playerGroupService;
        _dateTimeProvider = dateTimeProvider;
        _cacheManager = cacheManager;
    }

    public async Task<PlayerFullInfo> GetPlayerFullInfoAsync(string name, bool useCache, CancellationToken cancellationToken)
    {
        PlayerGroupInfo playerGroupInfo = await _playerGroupService.GetGroupInfoAsync(name, cancellationToken);
        SearchCriteria[] searchCriterias = playerGroupInfo.Select(x => new SearchCriteria(x)).ToArray();

        string cacheKey = string.Join("_", searchCriterias.Select(x => x.Name));
        return await _cacheManager.GetOrAddAsync(cacheKey, 
            useCache, 
            () => _chessDataManager.GetPlayerFullInfoAsync(searchCriterias, useCache, cancellationToken), 
            x => _dateTimeProvider.UtcNow.AddMinutes(1), 
            cancellationToken);
    }

    public async Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, int playerStartingRank, bool useCache, CancellationToken cancellationToken)
    {
        return await _chessDataManager.GetPlayerTournamentInfoAsync(tournamentId, playerStartingRank, useCache, cancellationToken);
    }
}