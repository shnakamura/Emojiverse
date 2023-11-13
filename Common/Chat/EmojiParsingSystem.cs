using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

public sealed class EmojiParsingSystem : ModSystem
{
    private static readonly Regex EscapeRegex = new Regex(@"\\:(\w+):", RegexOptions.Compiled);
    private static readonly Regex ParseRegex = new Regex(@":(\w+):", RegexOptions.Compiled);

    public override void OnModLoad() {
        On_ChatManager.ParseMessage += ParseMessageHook;

        ChatManager.Register<EmojiTagHandler>("e", "emoji", "emote");
    }

    private static List<TextSnippet> ParseMessageHook(On_ChatManager.orig_ParseMessage orig, string text, Color baseColor) {
        if (Main.gameMenu) {
            return orig(text, baseColor);
        }

        const string replacePattern = @"[e:$1]";

        text = text.Replace("\\:", "\x01");
        var parsed = ParseRegex.Replace(text, replacePattern);
        parsed = parsed.Replace("\x01", ":");

        return orig(parsed, baseColor);
    }
}
