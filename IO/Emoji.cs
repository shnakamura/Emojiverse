using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.IO;

public sealed class Emoji
{
    public string Name { get; }
    public string Alias { get; }
    public string Path { get; }
    public string Pack { get; }
    
    public int Id { get; }
    
    public bool Animated { get; }

    internal Emoji(string name, string alias, string path, string pack, int id, bool animated) {
        Name = name;
        Alias = alias;
        Path = path;
        Pack = pack;
        Id = id;
        Animated = animated;
    }
}   