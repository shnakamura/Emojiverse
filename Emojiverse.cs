using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public static string EmojiPath { get; internal set; } = Path.Combine(Main.SavePath, "EmojiPacks");

    public override void Load() {
        EmojiPath = Path.Combine(Main.SavePath, "EmojiPacks");
        if (Directory.Exists(EmojiPath)) {
            return;
        }

        Directory.CreateDirectory(EmojiPath);
    }
}
