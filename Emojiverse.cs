using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Emojiverse.Common.Chat;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public static string EmojisPath => Path.Combine(Main.SavePath, "Emojis");

    public override void Load() {
        if (!Directory.Exists(EmojisPath)) {
            Directory.CreateDirectory(EmojisPath);
        }

        CacheEmojiPacks();
    }

    public override void PostSetupContent() {
        ChatManager.Register<EmojiTagHandler>("e", "emoji");
    }

    private void CacheEmojiPacks() {
        var files = Directory.EnumerateFiles(EmojisPath, "*.zip", SearchOption.TopDirectoryOnly);

        foreach (var file in files) {
            using var zipStream = new FileStream(file, FileMode.Open);
            using var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);

            var icon = zipArchive.GetEntry("icon.png");
            var folder = zipArchive.GetEntry("emojis/");
        }
    }
}
