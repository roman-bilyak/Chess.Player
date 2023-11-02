namespace Chess.Player.Data;

internal interface IChessDataFetcher
{
    Task<List<PlayerTournament>> GetPlayerTournamentsAsync(string lastName, string firstName, CancellationToken cancellationToken);

    Task<TournamentInfo> GetTournamentInfoAsync(int tornamentId, CancellationToken cancellationToken);

    Task<PlayerInfo> GetPlayerInfoAsync(int tornamentId, int startingRank, CancellationToken cancellationToken);
}