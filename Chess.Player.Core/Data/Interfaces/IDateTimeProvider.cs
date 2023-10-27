namespace Chess.Player.Data;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}