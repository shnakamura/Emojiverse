using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.IO;

public sealed class Emoji
{
    public bool Animated { get; }
    
    public string Alias { get; }
    public string Name { get; }
    public string Path { get; }
    
    public int Id { get; }

    internal Emoji(string alias, string name, string path, int id) {
        Alias = alias;
        Name = name;
        Path = path;
        Id = id;
    }
}   