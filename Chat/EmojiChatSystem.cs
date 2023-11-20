using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse.Chat;

public sealed class EmojiChatSystem : ModSystem
{
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
        return orig(text, baseColor);
    }
}
