using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

public sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        var asset = Main.Assets.Request<Texture2D>($"Emojis/{text}");
        var alias = text;

        return new EmojiSnippet(asset, alias) {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
