namespace Chess.Player.Cache;

internal interface ICacheItem<T>
{
    T Value { get; }
    DateTime? ExpirationDate { get; }
}