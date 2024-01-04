using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Common;

internal sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        if (!EmojiSystem.TryGetEmoji(text, out var emoji)) {
            return new TextSnippet(text);
        }

        return new EmojiTagSnippet(emoji) {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
