using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    public override void Unload() {
        if (typeof(ChatManager).GetField("_handlers", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)?.GetValue(null) is IDictionary dict) {
            foreach (string name in new string[] { "e", "emoji", "emote" }) {
                dict.Remove(name);
            }
        }
        Main.NewText("A");
    }
}
