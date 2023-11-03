using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public static readonly string EmojiPath = Path.Combine(Main.SavePath, "EmojiPacks");

    public override void Load() {
        if (Directory.Exists(EmojiPath)) {
            return;
        }
        
        Directory.CreateDirectory(EmojiPath);
    }
}
