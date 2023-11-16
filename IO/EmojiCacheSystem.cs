using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse.IO;

public sealed class EmojiCacheSystem : ModSystem
{
    public static readonly List<Emoji> Emojis = new();

    public override void Load() {
        Main.AssetSourceController.OnResourcePackChange += ReloadFromList;
    }

    public override void PostSetupContent() {
        ReloadFromList(Main.AssetSourceController.ActiveResourcePackList);
    }

    private static void ReloadFromList(ResourcePackList list) {
        Emojis.Clear();

        var packs = list.EnabledPacks;

        foreach (var pack in packs) {
            var assets = pack.GetContentSource().EnumerateAssets();

            foreach (var asset in assets) {
                var directory = Path.GetDirectoryName(asset);
                var extension = Path.GetExtension(asset);

                if (directory != "Emojis") {
                    continue;
                }

                switch (extension) {
                    case ".png":
                        var name = Path.GetFileNameWithoutExtension(asset);
                        var entry = new Emoji(pack.Name, name);
                        
                        Emojis.Add(entry);
                        break;
                    case ".gif":
                        ModContent.GetInstance<Emojiverse>().Logger.Debug("GIFFFFFFFF");
                        break;
                    default:
                        throw new InvalidOperationException();
                        break;
                }
            }
        }
    }
}
