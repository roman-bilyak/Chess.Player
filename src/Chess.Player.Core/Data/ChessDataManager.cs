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

    public async Task<PlayerFullInfo> GetPlayerFullInfoAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken)
    {
        OnProgressChanged(PercentageStart);

        PlayerFullInfo playerFullInfo = new();

        List<PlayerTournament> playerTournaments = new();
        foreach (SearchCriteria searchCriteria in searchCriterias)
        {
            string[] nameParts = searchCriteria.Name.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string lastName = nameParts.First();
            string? firstName = string.Join(" ", nameParts.Skip(1));
            playerFullInfo.Names.Add(new NameInfo(lastName, firstName));

            List<PlayerTournament> tournaments = await _chessDataFetcher.GetPlayerTournamentsAsync(lastName, firstName, cancellationToken);
            playerTournaments.AddRange(tournaments);          
        }

        int index = 0;
        List<PlayerTournamentInfo> playerTournamentInfos = new();
        foreach (PlayerTournament playerTournament in playerTournaments)
        {
            index++;

            bool isForceRefresh = playerTournament.EndDate >= _dateTimeProvider.UtcNow.Date;

            PlayerTournamentInfo playerTournamentInfo = await GetPlayerTournamentInfoAsync(playerTournament.TournamentId, playerTournament.PlayerStartingRank, isForceRefresh, cancellationToken);
            playerTournamentInfos.Add(playerTournamentInfo);

            int progressPercentage = index * (PercentageFinish - PercentageStart) / playerTournaments.Count + PercentageStart;
            OnProgressChanged(progressPercentage);
        }

        playerTournamentInfos = playerTournamentInfos.OrderByDescending(x => x.Tournament.EndDate).ToList();
        playerFullInfo.Tournaments.AddRange(playerTournamentInfos);

        OnProgressChanged(PercentageFinish);
        return playerFullInfo;
    }

    public async Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, int playerStartingRank, bool isForceRefresh, CancellationToken cancellationToken)
    {
        TournamentInfo tournamentInfo = await _cacheManager.GetOrAddAsync
        (
            $"{tournamentId}",
            () => _chessDataFetcher.GetTournamentInfoAsync(tournamentId, cancellationToken),
            isForceRefresh, cancellationToken
        );

        PlayerInfo playerInfo = await _cacheManager.GetOrAddAsync
        (
            $"{tournamentId}_{playerStartingRank}",
            () => _chessDataFetcher.GetPlayerInfoAsync(tournamentId, playerStartingRank, cancellationToken),
            isForceRefresh, cancellationToken
        );

        return new PlayerTournamentInfo(tournamentInfo, playerInfo);
    }

    protected virtual void OnProgressChanged(int progressPercentage)
    {
        ProgressChanged?.Invoke(this, new SearchProgressEventArgs(progressPercentage));
    }
}