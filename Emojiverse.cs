using Emojiverse.Common.Chat;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public override void PostSetupContent() {
        ChatManager.Register<EmojiTagHandler>("e", "emoji", "emote");
    }
}
