using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Emojiverse.Common.Chat;
using Emojiverse.Common.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public static readonly string EmojiPath = Path.Combine(Main.SavePath, "EmojiPacks");

    private static readonly Dictionary<string, int> repeatedEmojisByName = new();

    public override void Load() {
        if (Directory.Exists(EmojiPath)) {
        CacheEmojiPacks();
            return;
        }
        
        Directory.CreateDirectory(EmojiPath);
        
    }

    private void CacheEmojiPacks() {
        var zipFiles = Directory.EnumerateFiles(EmojiPath, "*.zip", SearchOption.TopDirectoryOnly);

        foreach (var zipFile in zipFiles) {
            using var zipStream = new FileStream(zipFile, FileMode.Open);
            using var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);

            var images = zipArchive.GetEntry("images/");

            var entries = images.Archive.Entries;
 
            for (int i = 0; i < entries.Count; i++) {
                var entry = entries[i];

                var path = entry.FullName;
                var extension = Path.GetExtension(path);

                if (string.IsNullOrEmpty(extension) || extension != ".png") {
                    continue;
                }
                
                var name = Path.GetFileNameWithoutExtension(path);
                var alias = Path.GetFileNameWithoutExtension(path);

                if (repeatedEmojisByName.TryGetValue(name, out var count)) {
                    alias = $"{alias}~{count}";
                    repeatedEmojisByName[name]++;
                }
                else {
                    repeatedEmojisByName.Add(name, 1);
                }
                
                Logger.Debug($"Path: {path} @ Alias: {alias}");
            }
        }
    }
}
