using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Chess.Player.Data;

internal class ChessDataNormalizer : IChessDataNormalizer
{
    [return: NotNullIfNotNull(nameof(name))]
    public string? NormalizeName(string? name)
    {
        if (name == null)
        {
            return null;
        }

        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        string normalizedName = textInfo.ToTitleCase(name.ToLower());

        IEnumerable<string> nameParts = normalizedName.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Take(2);
        return string.Join(" ", nameParts);
    }
}