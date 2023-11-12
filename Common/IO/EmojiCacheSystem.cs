using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse.Common.IO;

public sealed class EmojiCacheSystem : ModSystem
{
    private static readonly List<Emoji> emojis = new();

    public override void Load() {
        Main.AssetSourceController.OnResourcePackChange += ResourcePackChangeHook;
    }

    public static ReadOnlySpan<Emoji> ReadEmojis() {
        return CollectionsMarshal.AsSpan(emojis);
    }

    private static void ResourcePackChangeHook(ResourcePackList list) {
        emojis.Clear();

        var enabledPacks = list.EnabledPacks;

        foreach (var pack in enabledPacks) {
            var enumeratedAssets = pack.GetContentSource().EnumerateAssets();

            foreach (var asset in enumeratedAssets) {
                var directory = Path.GetDirectoryName(asset);
                var extension = Path.GetExtension(asset);

                if (directory != "Emojis" || extension != ".png") {
                    continue;
                }

                var name = Path.GetFileNameWithoutExtension(asset);
                var entry = new Emoji(pack.Name, name);

                emojis.Add(entry);
            }
        }
    }
}
