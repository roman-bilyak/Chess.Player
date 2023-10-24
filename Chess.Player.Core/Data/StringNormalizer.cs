using Chess.Player.Data.Interfaces;
using System.Globalization;

namespace Chess.Player.Data;

public class StringNormalizer : IStringNormalizer
{
    public string NormalizeName(string name)
    {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        string normalizedName = textInfo.ToTitleCase(name.ToLower());

        string[] nameParts = normalizedName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", nameParts);
    }
}
