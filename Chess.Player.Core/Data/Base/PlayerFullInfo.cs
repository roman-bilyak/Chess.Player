namespace Chess.Player.Data;

public class PlayerFullInfo : PlayerShortInfo
{
    public List<PlayerTournamentInfo> Tournaments { get; set; } = new List<PlayerTournamentInfo>();
}