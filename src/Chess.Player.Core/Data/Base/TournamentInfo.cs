﻿namespace Chess.Player.Data;

public class TournamentInfo
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public string? Organizers { get; set; }

    public string? Federation { get; set; }

    public string? TournamentDirector { get; set; }

    public string? ChiefArbiter { get; set; }

    public string? DeputyChiefArbiter { get; set; }

    public string? Arbiter { get; set; }

    public string? TimeControl { get; set; }

    public TimeControlType TimeControlType { get; set; }

    public string? Location { get; set; }

    public int? NumberOfRounds { get; set; }

    public string? TournamentType { get; set; }

    public string? RatingCalculation { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? Rating { get; set; }

    public List<PlayerScoreInfo> Players { get; set; } = new List<PlayerScoreInfo>();

    public bool IsTeamTournament { get; set; }

    public bool IsOnline(DateTime currentDate)
    {
        return (!StartDate.HasValue || currentDate >= StartDate.Value) && (!EndDate.HasValue || currentDate <= EndDate.Value);
    }

    public bool IsFuture(DateTime currentDate)
    {
        return StartDate.HasValue && currentDate < StartDate.Value;
    }
}