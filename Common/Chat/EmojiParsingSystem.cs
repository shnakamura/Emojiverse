using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

public sealed class EmojiParsingSystem : ModSystem
{
    private static readonly Regex MatchRegex = new(@":(\w+):", RegexOptions.Compiled);

    public override void OnModLoad() {
        On_ChatManager.ParseMessage += ParseMessageHook;

        ChatManager.Register<EmojiTagHandler>("e", "emoji", "emote");
    }

    private static List<TextSnippet> ParseMessageHook(On_ChatManager.orig_ParseMessage orig, string text, Color baseColor) {
        if (Main.gameMenu) {
            return orig(text, baseColor);
        }

        const string replacePattern = @"[e:$1]";
        const string dummyCharacter = @"\x01";

        text = text.Replace("\\:", dummyCharacter);
        
        var parsed = MatchRegex.Replace(text, replacePattern);
        
        parsed = parsed.Replace(dummyCharacter, ":");

        return orig(parsed, baseColor);
    }
}
