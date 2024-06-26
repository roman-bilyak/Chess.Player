﻿namespace Chess.Player.Data;

public class GameInfo
{
    public int? Round { get; set; }

    public int? Board { get; set; }

    public int? No { get; set; }

    public string? Name { get; set; }

    public int? Rating { get; set; }

    public string? ClubCity { get; set; }

    public bool? IsWhite { get; set; }

    public double? Result { get; set; }
}