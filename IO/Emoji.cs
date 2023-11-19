using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.IO;

public sealed class Emoji
{
    public string Path;
    public bool Animated;
    
    public string Alias { get; }
    public string Name { get; }
    
    public int Id { get; }

    internal Emoji(string alias, string name, int id) {
        Alias = alias;
        Name = name;
        Id = id;
    }
}   