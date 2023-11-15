namespace Chess.Player.Data;

internal interface IChessDataManager
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetPlayerFullInfoAsync(SearchCriteria[] searchCriterias, bool useCache, CancellationToken cancellationToken);

    Task<TournamentInfo> GetTournamentInfoAsync(int tournamentId, bool useCache, CancellationToken cancellationToken);

    Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, int playerNo, bool useCache, CancellationToken cancellationToken);
}