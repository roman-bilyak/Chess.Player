namespace Chess.Player.Data;

public interface IChessDataService
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetPlayerFullInfoAsync(string name, bool useCache, CancellationToken cancellationToken);

    Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, DateTime? tournamentEndDate, int playerStartingRank, bool useCache, CancellationToken cancellationToken);
}