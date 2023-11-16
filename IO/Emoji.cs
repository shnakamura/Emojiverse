namespace Emojiverse.IO;

public sealed class Emoji : IEmoji
{
    public string Pack { get; private set; }
    public string Name { get; private set; }
    
    public Emoji(string pack, string name) {
        Pack = pack;
        Name = name;
    }
}
