namespace Emojiverse.Common.IO;

public readonly struct Emoji
{
    public readonly string Path;
    public readonly string Alias;

    public Emoji(string path, string alias) {
        Path = path;
        Alias = alias;
    }
}
