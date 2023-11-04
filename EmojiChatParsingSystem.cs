using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse;

internal sealed class EmojiChatParsingSystem : ModSystem
{
    private static Regex matchRegex = new(@":(\w+):", RegexOptions.Compiled);
    public override void OnModLoad() {
        On_ChatManager.ParseMessage += ParseMessageHook;
    }

    public override void PostSetupContent() {
        ChatManager.Register<EmojiTagHandler>("e", "emoji");
    }

    private static List<TextSnippet> ParseMessageHook(On_ChatManager.orig_ParseMessage orig, string text, Color baseColor) {
        if (Main.gameMenu) {
            return orig(text, baseColor);
        }

        var parsed = matchRegex.Replace(text, static match => $"[e{match.Value}]");

        return orig(parsed, baseColor);
    }
}
