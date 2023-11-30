namespace Chess.Player.Data;

public interface IChessDataService
{
    event ProgressEventHandler? ProgressChanged;

    Task<PlayerFullInfo> GetPlayerFullInfoAsync(string name, bool useCache, CancellationToken cancellationToken);

    Task<TournamentInfo> GetTournamentInfoAsync(int tournamentId, bool useCache, CancellationToken cancellationToken);

    Task<PlayerTournamentInfo> GetPlayerTournamentInfoAsync(int tournamentId, int playerNo, bool useCache, CancellationToken cancellationToken);
}