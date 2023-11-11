using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

public sealed class EmojiParsingSystem : ModSystem
{
    private static readonly Regex MatchPattern = new Regex(@":(\w+):", RegexOptions.Compiled);
    
    public override void OnModLoad() {
        On_ChatManager.ParseMessage += ParseMessageHook;

        ChatManager.Register<EmojiTagHandler>("e", "emoji", "emote");
    }

    private static List<TextSnippet> ParseMessageHook(On_ChatManager.orig_ParseMessage orig, string text, Color baseColor) {
        if (Main.gameMenu) {
            return orig(text, baseColor);
        }

        var parsed = MatchPattern.Replace(text, @"[e:$1]");

        return orig(parsed, baseColor);
    }
}
