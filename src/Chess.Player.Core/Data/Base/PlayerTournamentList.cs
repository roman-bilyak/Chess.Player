namespace Chess.Player.Data;

public class PlayerTournamentList
{
    public List<PlayerTournament> Items { get; protected set; } = new List<PlayerTournament>();

    public DateTime? LastUpdateTime { get; set; }
}