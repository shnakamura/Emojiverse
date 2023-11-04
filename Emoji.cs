using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Emojiverse;

public readonly struct Emoji
{
    public readonly string Alias;

    public Emoji(string alias) {
        Alias = alias;
    }
}

public sealed class EmojiSystem : ModSystem
{
    public static Dictionary<string, Texture2D> texturesByAlias = new();
    public static Dictionary<string, Emoji> emojisByAlias = new();

    public override void OnModLoad() {
        Main.QueueMainThreadAction(
            () => {
                var zipFiles = Directory.EnumerateFiles(Emojiverse.EmojiPath, "*.zip", SearchOption.TopDirectoryOnly);

                foreach (var zipFile in zipFiles) {
                    using var zipStream = new FileStream(zipFile, FileMode.Open);
                    using var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);

                    var icon = zipArchive.GetEntry("icon.png");
                    var images = zipArchive.GetEntry("images/");

                    var entries = images.Archive.Entries;
                    var emojis = new Emoji[entries.Count];

                    for (int i = 0; i < entries.Count; i++) {
                        var entry = entries[i];
                
                        var path = entry.FullName;
                        var extension = Path.GetExtension(path);

                        if (extension != ".png") {
                            continue;
                        }
                        
                        var alias = Path.GetFileNameWithoutExtension(path);
                
                        emojisByAlias[alias] = new Emoji(alias);

                        var memoryStream = new MemoryStream();
                        var fileStream = entry.Open();
                        
                        fileStream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var data = memoryStream.GetBuffer();
                        
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var texture = Texture2D.FromStream(Main.graphics.GraphicsDevice, memoryStream);

                        texturesByAlias[alias] = texture;
                    }
                }
            });

    }
}