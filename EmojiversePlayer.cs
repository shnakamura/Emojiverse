using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Emojiverse;

internal sealed class EmojiversePlayer : ModPlayer
{
    public override void OnEnterWorld() {
        if (!EmojiverseConfig.Instance.EnableWarningMessages || Emojiverse.HasAnyEmoji()) {
            return;
        }

        Main.NewText(Language.GetTextValue($"Mods.{nameof(Emojiverse)}.Messages.Undetected"), new Color(214, 41, 76));
    }
}
