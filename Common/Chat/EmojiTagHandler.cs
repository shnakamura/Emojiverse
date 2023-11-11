using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

public sealed class EmojiTagHandler : ITagHandler
{
    public TextSnippet Parse(string text, Color baseColor = default, string options = null) {
        var path = $"Emojis/{text}";
        var hasAsset = false;

        foreach (var resourcePack in Main.AssetSourceController.ActiveResourcePackList.EnabledPacks) {
            if (resourcePack.GetContentSource().HasAsset(path)) {
                hasAsset = true;
                break;
            }
        }

        if (!hasAsset) {
            return new TextSnippet(text);
        }

        var asset = Main.Assets.Request<Texture2D>(path);
        var alias = text;

        return new EmojiSnippet(asset, alias) {
            CheckForHover = true,
            DeleteWhole = true
        };
    }
}
