using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Emojiverse.Common;

internal sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        if (!int.TryParse(text, out var id) || !EmojiSystem.TryGetEmoji(id, out var emoji)) {
            return new TextSnippet(text);
        }

        return new EmojiTagSnippet(emoji) {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
