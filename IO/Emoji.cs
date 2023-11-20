using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.IO;

public sealed class Emoji
{
    public string Alias { get; }
    public string Name { get; }
    public string Path { get; }
    
    public int Id { get; }
    
    public bool Animated { get; }

    internal Emoji(string alias, string name, string path, int id, bool animated) {
        Alias = alias;
        Name = name;
        Path = path;
        Id = id;
        Animated = animated;
    }
}   