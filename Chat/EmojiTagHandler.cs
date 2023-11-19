using Emojiverse.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Chat;

public sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        var id = int.Parse(text);
        
        if (!EmojiLoader.Has(id)) {
            return new TextSnippet(text);
        };
        
        return new EmojiTagSnippet(EmojiLoader.Get(id)) {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
