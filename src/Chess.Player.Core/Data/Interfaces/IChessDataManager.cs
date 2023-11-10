namespace Chess.Player.Data;

internal interface IChessDataManager
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetPlayerFullInfoAsync(SearchCriteria[] searchCriterias, bool useCache, CancellationToken cancellationToken);

    Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, DateTime? tournamentEndDate, int playerStartingRank, bool useCache, CancellationToken cancellationToken);
}