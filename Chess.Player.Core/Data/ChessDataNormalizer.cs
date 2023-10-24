using System.Globalization;

namespace Chess.Player.Data;

public class ChessDataNormalizer : IChessDataNormalizer
{
    public string NormalizeName(string name)
    {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        string normalizedName = textInfo.ToTitleCase(name.ToLower());

        string[] nameParts = normalizedName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", nameParts);
    }
}
