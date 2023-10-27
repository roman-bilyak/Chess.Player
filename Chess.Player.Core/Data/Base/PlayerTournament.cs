namespace Chess.Player.Data;

public class PlayerTournament
{
    public int TournamentId { get; private set; }

    public int PlayerStartingRank { get; private set; }

    public DateTime EndDate { get; private set; }

    public PlayerTournament(int tournamentId, int playerStartingRank, DateTime endDate)
    {
        TournamentId = tournamentId;
        PlayerStartingRank = playerStartingRank;
        EndDate = endDate;
    }
}