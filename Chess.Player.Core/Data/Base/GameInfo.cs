namespace Chess.Player.Data;

public class GameInfo
{
    public int? Round { get; set; }

    public string? OponentName { get; set; }

    public bool? IsWhite { get; set; }

    public double? Result { get; set; }
}