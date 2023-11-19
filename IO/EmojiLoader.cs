using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse.IO;

public sealed class EmojiLoader : ModSystem
{
    private static readonly Dictionary<string, int> repeatedNamesCountByName = new();

    public override void Load() {
        Main.AssetSourceController.OnResourcePackChange += UpdateEmojis;
        
        UpdateEmojis(Main.AssetSourceController.ActiveResourcePackList);
    }

    private static void UpdateEmojis(ResourcePackList list) {
        foreach (var pack in list.EnabledPacks) {
            foreach (var asset in pack.GetContentSource().EnumerateAssets()) {
                var path = $"{pack.FileName}/{asset}";
                
            } 
        }
    }
}
