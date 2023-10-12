namespace Chess.Player.Data
{
    internal interface IChessDataFetcher
    {
        Task<List<PlayerTournament>?> GetPlayerTournamentsAsync(string lastName, string firstName);

        Task<TournamentInfo?> GetTournamentInfoAsync(int tornamentId);

        Task<PlayerInfo?> GetPlayerInfoAsync(int tornamentId, int startingRank);
    }
}