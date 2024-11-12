using System.Diagnostics.CodeAnalysis;

namespace Chess.Player;

public static class StringExtensions
{
    [return: NotNullIfNotNull(nameof(input))]
    public static string? Replace(this string? input, char[] oldChars, char newChar = '\0')
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        string newCharString = newChar == '\0' ? string.Empty : newChar.ToString();
        foreach (char oldChar in oldChars)
        {
            input = input.Replace(oldChar.ToString(), newCharString);
        }

        return input;
    }
}