using System.Collections;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse.Common;

internal sealed class EmojiChatSystem : ModSystem
{
    private static readonly FieldInfo handlersInfo = typeof(ChatManager).GetField("_handlers", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

    private static readonly string[] names = {
        "e",
        "emoji",
        "emote"
    };

    public override void Load() {
        ChatManager.Register<EmojiTagHandler>(names);
    }

    public override void Unload() {
        if (handlersInfo.GetValue(null) is not IDictionary dictionary) {
            return;
        }
        
        foreach (var name in names) {
            dictionary.Remove(name);
        }
    }
}
