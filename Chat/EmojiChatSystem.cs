using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse.Chat;

public sealed class EmojiChatSystem : ModSystem
{
    private static readonly Regex MatchRegex = new(@":(\w+):", RegexOptions.Compiled);

    public override void OnModLoad() {
        ChatManager.Register<EmojiTagHandler>("e", "emoji", "emote");
    }
}
