using Emojiverse.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Chat;

public sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        if (!int.TryParse(text, out var id) || !EmojiLoader.TryGet(id, out var emoji)) {
            return new TextSnippet(text);
        };
        
        return new EmojiTagSnippet(emoji) {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
