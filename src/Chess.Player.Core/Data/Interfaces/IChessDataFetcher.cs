namespace Chess.Player.Data;

internal interface IChessDataFetcher
{
    Task<PlayerTournamentList> GetPlayerTournamentListAsync(string lastName, string firstName, CancellationToken cancellationToken);

    Task<TournamentInfo> GetTournamentInfoAsync(int tornamentId, CancellationToken cancellationToken);

    Task<PlayerInfo> GetPlayerInfoAsync(int tornamentId, int startingRank, CancellationToken cancellationToken);
}