using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse;

public sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        ModContent.GetInstance<Emojiverse>().Logger.Debug(text);

        return new EmojiSnippet(text) {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
