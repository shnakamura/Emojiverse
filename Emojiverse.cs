using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    internal static Dictionary<string, Texture2D> EmojiImageCache { get; set; } = new();
    
    public static string EmojiPath => Path.Combine(Main.SavePath, "Emojis");

    public override void Load() {
        if (Directory.Exists(EmojiPath)) {
            return;
        }

        Directory.CreateDirectory(EmojiPath);
    }

    public override void PostSetupContent() {
        ChatManager.Register<EmojiTagHandler>("e", "emoji");
    }
}
