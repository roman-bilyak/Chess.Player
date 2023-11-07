using System.Diagnostics.CodeAnalysis;

namespace Chess.Player.Data;

public interface IChessDataNormalizer
{
    [return: NotNullIfNotNull(nameof(name))]
    string? NormalizeName(string? name);
}