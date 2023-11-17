using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Chat;

public sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        return new TextSnippet(text) {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
