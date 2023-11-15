namespace Chess.Player.Data;

public class PlayerTournament
{
    public int TournamentId { get; private set; }

    public int PlayerNo { get; private set; }

    public DateTime EndDate { get; private set; }

    public PlayerTournament(int tournamentId, int playerNo, DateTime endDate)
    {
        TournamentId = tournamentId;
        PlayerNo = playerNo;
        EndDate = endDate;
    }
}