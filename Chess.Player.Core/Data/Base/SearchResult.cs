namespace Chess.Player.Data;

public class SearchResult
{
    public List<string> Names { get; set; } = new List<string>();

    public List<PlayerTournamentInfo> Data { get; set; } = new List<PlayerTournamentInfo>();

    public string? Title
    {
        get
        {
            return this.Data.FirstOrDefault(x => x.Player.Title is not null)?.Player.Title;
        }
    }

    public string? FideId
    {
        get
        {
            return this.Data.FirstOrDefault(x => x.Player.FideId is not null)?.Player.FideId;
        }
    }

    public string? ClubCity
    {
        get
        {
            return this.Data.FirstOrDefault(x => x.Player.ClubCity is not null)?.Player.ClubCity;
        }
    }

    public int? YearOfBirth
    {
        get
        {
            return this.Data.FirstOrDefault(x => x.Player.YearOfBirth is not null)?.Player.YearOfBirth;
        }
    }
}