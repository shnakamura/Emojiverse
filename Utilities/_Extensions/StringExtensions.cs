namespace Emojiverse.Utilities;

public static class StringExtensions
{
    /// <summary>
    ///     Surrounds the given string with the specified character.
    /// </summary>
    /// <param name="value">The original string.</param>
    /// <param name="character">The character to surround the original string with.</param>
    /// <returns>A new string surrounded by the specified character.</returns>
    public static string SurroundWith(this string value, char character) {
        return $"{character}{value}{character}";
    }

    /// <summary>
    ///     Surrounds the given string with the specified string.
    /// </summary>
    /// <param name="value">The original string.</param>
    /// <param name="character">The string to surround the original string with.</param>
    /// <returns>A new string surrounded by the specified string.</returns>
    public static string SurroundWith(this string value, string character) {
        return $"{character}{value}{character}";
    }
}