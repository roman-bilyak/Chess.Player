using System.ComponentModel;

namespace Chess.Player.Cache;

public class CacheItem<T>
{
    public T Value { get; protected set; }

    [DefaultValue(null)]
    public DateTime? ExpirationDate { get; protected set; }

    public CacheItem(T value, DateTime? expirationDate = null)
    {
        Value = value;
        ExpirationDate = expirationDate;
    }
}