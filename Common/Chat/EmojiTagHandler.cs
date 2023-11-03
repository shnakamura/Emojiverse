using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

public sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        return new EmojiSnippet() {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
