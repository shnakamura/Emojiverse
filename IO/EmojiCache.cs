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
    private static readonly Dictionary<string, int> repeatedNamesCountByName = new();
    
    public override void Load() {
        Main.AssetSourceController.OnResourcePackChange += ReloadFromList;
    }

    public override void PostSetupContent() {
        ReloadFromList(Main.AssetSourceController.ActiveResourcePackList);
    }

    private void ReloadFromList(ResourcePackList list) {
        foreach (var pack in list.EnabledPacks) {
            foreach (var asset in pack.GetContentSource().EnumerateAssets()) {
                var directory = Path.GetDirectoryName(asset);
                var extension = Path.GetExtension(asset);

                if (directory != "Emojis") {
                    continue;
                }

                var name = Path.GetFileNameWithoutExtension(asset);
                var alias = name;
                
                if (repeatedNamesCountByName.TryGetValue(name, out var count)) {
                    alias += $"~{count}";
                }

                repeatedNamesCountByName[name]++;
            }
        }
    }
}
