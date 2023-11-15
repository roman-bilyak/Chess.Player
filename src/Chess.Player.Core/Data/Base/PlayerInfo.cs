namespace Chess.Player.Data;

public class PlayerInfo
{
    public string? Name { get; set; }

    public string? Title { get; set; }

    public int? No { get; set; }

    public int? Rating { get; set; }

    public int? RatingNational { get; set; }

    public int? RatingInternational { get; set; }

    public int? PerformanceRating { get; set; }

    public double? FideRtg { get; set; }

    public double? Points { get; set; }

    public int? Rank { get; set; }

    public string? Federation { get; set; }

    public string? ClubCity { get; set; }

    public string? IdentNumber { get; set; }

    public string? FideId { get; set; }

    public int? YearOfBirth { get; set; }

    public List<GameInfo> Games { get; set; } = new List<GameInfo>();
}