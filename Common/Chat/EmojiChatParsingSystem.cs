using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

internal sealed class EmojiChatParsingSystem : ModSystem
{
    public override void OnModLoad() {
        On_ChatManager.ParseMessage += ParseMessageHook;
    }
    
    private static List<TextSnippet> ParseMessageHook(On_ChatManager.orig_ParseMessage orig, string text, Color baseColor) {
        const string pattern = @":(\w+):";
        const string replacement = "[e:$1]";

        var matches = Regex.Matches(text, pattern, RegexOptions.Compiled);
        var parsed = Regex.Replace(text, pattern, replacement);

        return orig(parsed, baseColor);
    }
}
