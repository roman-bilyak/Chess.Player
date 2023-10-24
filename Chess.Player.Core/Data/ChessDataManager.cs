namespace Chess.Player.Data;

internal class ChessDataManager : IChessDataManager
{
    private readonly IChessDataFetcher _chessDataFetcher;
    private readonly ICacheManager _cacheManager;

    public event SearchProgressEventHandler? ProgressChanged;

    public ChessDataManager(IChessDataFetcher chessDataFetcher, ICacheManager cacheManager) 
    {
        ArgumentNullException.ThrowIfNull(chessDataFetcher);
        ArgumentNullException.ThrowIfNull(cacheManager);

        _chessDataFetcher = chessDataFetcher;
        _cacheManager = cacheManager;
    }

    public async Task<PlayerFullInfo> SearchAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken)
    {
        OnProgressChanged(1);

        PlayerFullInfo playerFullInfo = new();

        List<PlayerTournament> playerTournaments = new();
        foreach (SearchCriteria searchCriteria in searchCriterias)
        {
            string[] nameParts = searchCriteria.Name.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string? lastName = nameParts.FirstOrDefault();
            if (lastName is null)
            {
                continue;
            }
            string? firstName = string.Join(" ", nameParts.Skip(1));

            List<PlayerTournament>? tournaments = await _chessDataFetcher.GetPlayerTournamentsAsync(lastName, firstName, cancellationToken);
            if (tournaments is not null && tournaments.Any())
            {
                playerFullInfo.Names.Add(new NameInfo(lastName, firstName));

                playerTournaments.AddRange(tournaments);
            }
        }

        int i = 0;
        List<PlayerTournamentInfo> playerTournamentInfos = new();
        foreach (PlayerTournament playerTournament in playerTournaments)
        {
            TournamentInfo? tournamentInfo = await _cacheManager.GetOrAddAsync(nameof(TournamentInfo), $"{playerTournament.TournamentId}",
                () => _chessDataFetcher.GetTournamentInfoAsync(playerTournament.TournamentId, cancellationToken),
                false, cancellationToken
            );

            PlayerInfo? playerInfo = await _cacheManager.GetOrAddAsync(nameof(PlayerInfo), $"{playerTournament.TournamentId}_{playerTournament.PlayerStartingRank}",
                () => _chessDataFetcher.GetPlayerInfoAsync(playerTournament.TournamentId, playerTournament.PlayerStartingRank, cancellationToken),
                false, cancellationToken
            );

            if (tournamentInfo is not null && playerInfo is not null)
            {
                playerTournamentInfos.Add(new PlayerTournamentInfo(tournamentInfo, playerInfo));
            }

            int progressPercentage = i++ * 100 / playerTournaments.Count;
            OnProgressChanged(progressPercentage);
        }

        playerTournamentInfos = playerTournamentInfos.OrderByDescending(x => x.Tournament.EndDate).ToList();
        playerFullInfo.Tournaments.AddRange(playerTournamentInfos);

        playerFullInfo.Title = playerFullInfo.Tournaments.FirstOrDefault(x => x.Player.Title is not null)?.Player.Title;
        playerFullInfo.FideId = playerFullInfo.Tournaments.FirstOrDefault(x => x.Player.FideId is not null)?.Player.FideId;
        playerFullInfo.ClubCity = playerFullInfo.Tournaments.FirstOrDefault(x => x.Player.ClubCity is not null)?.Player.ClubCity;
        playerFullInfo.YearOfBirth = playerFullInfo.Tournaments.FirstOrDefault(x => x.Player.YearOfBirth is not null)?.Player.YearOfBirth;

        OnProgressChanged(100);
        return playerFullInfo;
    }

    protected virtual void OnProgressChanged(int progressPercentage)
    {
        ProgressChanged?.Invoke(this, new SearchProgressEventArgs(progressPercentage));
    }
}