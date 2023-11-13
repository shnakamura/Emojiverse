using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Emojiverse.Common.IO;

public sealed class EmojiWarningPlayer : ModPlayer
{
    public override void OnEnterWorld() {
        if (!ClientConfig.Instance.EnableWarningMessages) {
            return;
        }
        
        var emojis = EmojiCacheSystem.ReadEmojis();

        if (emojis.Length > 0) {
            return;
        }

        Main.NewText(Language.GetTextValue($"Mods.{nameof(Emojiverse)}.Messages.Undetected"), new Color(214, 41, 76));
    }
}
