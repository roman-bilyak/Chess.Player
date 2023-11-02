namespace Chess.Player.Data;

public interface IChessDataService
{
    event SearchProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetPlayerFullInfoAsync(string name, bool isForceRefresh, CancellationToken cancellationToken);

    Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, int playerStartingRank, bool isForceRefresh, CancellationToken cancellationToken);
}