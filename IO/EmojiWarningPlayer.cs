using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Emojiverse.IO;

public sealed class EmojiWarningPlayer : ModPlayer
{
    public override void OnEnterWorld() {
        if (!EmojiverseConfig.Instance.EnableWarningMessages) {
            return;
        }

        var emojis = EmojiCacheSystem.Emojis;

        if (emojis.Count > 0) {
            return;
        }

        Main.NewText(Language.GetTextValue($"Mods.{nameof(Emojiverse)}.Messages.Undetected"), new Color(214, 41, 76));
    }
}
