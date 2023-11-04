using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Emojiverse;

public sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        if (!EmojiSystem.emojisByAlias.TryGetValue(text, out var emoji)) {
            return new TextSnippet($":{text}:", baseColor);
        }
        
        return new EmojiSnippet(emoji) {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
