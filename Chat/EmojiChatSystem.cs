using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.RegularExpressions;
using Emojiverse.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse.Chat;

public sealed class EmojiChatSystem : ModSystem
{
    private static readonly Regex MatchRegex = new(@":(\w+):", RegexOptions.Compiled);
    
    private static readonly FieldInfo handlersInfo = typeof(ChatManager).GetField("_handlers", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
    
    private static readonly string[] names = new[] {
        "e",
        "emoji",
        "emote"
    };
    
    public override void Load() {
        ChatManager.Register<EmojiTagHandler>(names);
        
        On_ChatManager.ParseMessage += ParseMessageHook;
    }

    public override void Unload() {
        if (handlersInfo.GetValue(null) is IDictionary dictionary) {
            foreach (var name in names) {
                dictionary.Remove(name);
            }
        }
    }
    
    private static List<TextSnippet> ParseMessageHook(On_ChatManager.orig_ParseMessage orig, string text, Color baseColor) {
        if (Main.gameMenu) {
            return orig(text, baseColor);
        }

        const string ReplacePattern = @"[e:$1]";

        var parsed = MatchRegex.Replace(text, match => {
            var content = match.Groups[1].Value;

            if (!EmojiLoader.TryGetId(content, out var id)) {
                return text;
            }
            
            return ReplacePattern.Replace("$1", id.ToString());
        });

        return orig(parsed, baseColor);
    }
}
