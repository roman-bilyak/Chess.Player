using Chess.Player.Cache;

namespace Chess.Player.Data;

internal class ChessDataManager : IChessDataManager
{
    private const int PercentageStart = 1;
    private const int PercentageFinish = 100;

    private readonly IChessDataFetcher _chessDataFetcher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICacheManager _cacheManager;

    public event SearchProgressEventHandler? ProgressChanged;

    public ChessDataManager
    (
        IChessDataFetcher chessDataFetcher,
        IDateTimeProvider dateTimeProvider,
        ICacheManager cacheManager
    ) 
    {
        ArgumentNullException.ThrowIfNull(chessDataFetcher);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _chessDataFetcher = chessDataFetcher;
        _dateTimeProvider = dateTimeProvider;
        _cacheManager = cacheManager;
    }

    public async Task<PlayerFullInfo> GetPlayerFullInfoAsync(SearchCriteria[] searchCriterias, bool useCache, CancellationToken cancellationToken)
    {
        OnProgressChanged(PercentageStart);

        PlayerFullInfo playerFullInfo = new();

        List<PlayerTournament> playerTournaments = new();
        TimeSpan? cacheInvalidatePeriod = _cacheManager.GetCacheInvalidatePeriod(useCache);
        foreach (SearchCriteria searchCriteria in searchCriterias)
        {
            string[] nameParts = searchCriteria.Name.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string lastName = nameParts.First();
            string? firstName = string.Join(" ", nameParts.Skip(1));
            playerFullInfo.Names.Add(new NameInfo(lastName, firstName));

            PlayerTournamentList playerTournamentList = await _cacheManager.GetOrAddAsync($"{lastName}_{firstName}", cacheInvalidatePeriod, async () => await _chessDataFetcher.GetPlayerTournamentListAsync(lastName, firstName, cancellationToken), cancellationToken);
            playerTournaments.AddRange(playerTournamentList.Items);
        }

        int index = 0;
        List<PlayerTournamentInfo> playerTournamentInfos = new();
        foreach (PlayerTournament playerTournament in playerTournaments)
        {
            index++;

            PlayerTournamentInfo playerTournamentInfo = await GetPlayerTournamentInfoAsync(playerTournament.TournamentId, playerTournament.EndDate, playerTournament.PlayerStartingRank, useCache, cancellationToken);
            playerTournamentInfos.Add(playerTournamentInfo);

            int progressPercentage = index * (PercentageFinish - PercentageStart) / playerTournaments.Count + PercentageStart;
            OnProgressChanged(progressPercentage);
        }

        playerTournamentInfos = playerTournamentInfos.OrderByDescending(x => x.Tournament.EndDate).ThenByDescending(x => x.Tournament.StartDate).ToList();
        playerFullInfo.Tournaments.AddRange(playerTournamentInfos);

        OnProgressChanged(PercentageFinish);
        return playerFullInfo;
    }

    public async Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, DateTime? tournamentEndDate, int playerStartingRank, bool useCache, CancellationToken cancellationToken)
    {
        bool isArchive = tournamentEndDate is not null && tournamentEndDate < _dateTimeProvider.UtcNow.Date;
        TimeSpan? cacheInvalidatePeriod = _cacheManager.GetCacheInvalidatePeriod(useCache || isArchive);

        TournamentInfo tournamentInfo = await _cacheManager.GetOrAddAsync($"{tournamentId}", cacheInvalidatePeriod, async () => await _chessDataFetcher.GetTournamentInfoAsync(tournamentId, cancellationToken), cancellationToken);
        PlayerInfo playerInfo = await _cacheManager.GetOrAddAsync($"{tournamentId}_{playerStartingRank}", cacheInvalidatePeriod, async () => await _chessDataFetcher.GetPlayerInfoAsync(tournamentId, playerStartingRank, cancellationToken), cancellationToken);

        return new PlayerTournamentInfo(tournamentInfo, playerInfo);
    }

    protected virtual void OnProgressChanged(int progressPercentage)
    {
        ProgressChanged?.Invoke(this, new SearchProgressEventArgs(progressPercentage));
    }
}