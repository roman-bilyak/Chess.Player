namespace Chess.Player.Data;

internal interface IChessDataManager
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetPlayerFullInfoAsync(SearchCriteria[] searchCriterias, CancellationToken cancellationToken);

    Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, int playerStartingRank, bool isForceRefresh, CancellationToken cancellationToken);
}