namespace Chess.Player.Data;

public class PlayerShortInfo
{
    public List<NameInfo> Names { get; set; } = new List<NameInfo>();

    public string? Title { get; set; }

    public string? FideId { get; set; }

    public string? ClubCity { get; set; }

    public int? YearOfBirth { get; set; }
}