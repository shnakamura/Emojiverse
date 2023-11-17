using System;
using System.Collections.Generic;
using System.IO;
using Emojiverse.IO.Readers;
using ReLogic.Content;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse.IO;

public sealed class EmojiCache : ModSystem
{
    public static readonly List<Emoji> Emojis = new();

    public override void Load() {
        Main.AssetSourceController.OnResourcePackChange += ReloadFromList;
    }

    public override void PostSetupContent() {
        ReloadFromList(Main.AssetSourceController.ActiveResourcePackList);
    }

    private void ReloadFromList(ResourcePackList list) {
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

                var name = Path.GetFileNameWithoutExtension(asset);
                var entry = new Emoji(pack.Name, name);

                if (extension == ".gif") {
                    Mod.Logger.Debug("Animated image found");
                }
                else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg") {
                    Mod.Logger.Debug("Image found");
                }
                        
                Emojis.Add(entry);
            }
        }
    }
}
