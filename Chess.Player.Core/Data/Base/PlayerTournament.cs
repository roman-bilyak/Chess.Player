namespace Chess.Player.Data
{
    public class PlayerTournament
    {
        public int TournamentId { get; private set; }

        public int PlayerStartingRank { get; private set; }

        public PlayerTournament(int tournamentId, int playerStartingRank)
        {
            TournamentId = tournamentId;
            PlayerStartingRank = playerStartingRank;
        }
    }
}