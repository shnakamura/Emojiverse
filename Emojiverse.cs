using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public static string EmojiPath => Path.Combine(Main.SavePath, "Emojis");

    public override void Load() {
        ChatManager.Register<EmojiTagHandler>("e", "emoji");

        if (Directory.Exists(EmojiPath)) {
            return;
        }

        Directory.CreateDirectory(EmojiPath);
    }
}
