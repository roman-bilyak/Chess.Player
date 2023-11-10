using Chess.Player.Cache.Interfaces;

namespace Chess.Player.Data;

public class PlayerFullInfo: ICacheItem
{
    public string? Name => Names.FirstOrDefault()?.FullName;

    public string? Title => this.Tournaments.FirstOrDefault(x => x.Player.Title is not null)?.Player.Title;

    public string? FideId => this.Tournaments.FirstOrDefault(x => x.Player.FideId is not null)?.Player.FideId;

    public string? ClubCity => this.Tournaments.FirstOrDefault(x => x.Player.ClubCity is not null)?.Player.ClubCity;

    public int? YearOfBirth => this.Tournaments.FirstOrDefault(x => x.Player.YearOfBirth is not null)?.Player.YearOfBirth;

    public List<NameInfo> Names { get; set; } = new List<NameInfo>();

    public List<PlayerTournamentInfo> Tournaments { get; set; } = new List<PlayerTournamentInfo>();

    public DateTime? LastUpdateTime { get; set; }
}