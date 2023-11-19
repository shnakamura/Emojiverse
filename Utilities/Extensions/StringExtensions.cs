namespace Emojiverse.Utilities.Extensions;

public static class StringExtensions
{
    public static string SurroundWith(this string value, char character) {
        return $"{character}{value}{character}";
    }
    
    public static string SurroundWith(this string value, string character) {
        return $"{character}{value}{character}";
    }
}
