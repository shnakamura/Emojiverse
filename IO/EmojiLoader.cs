using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse.IO;

public sealed class EmojiLoader : ModSystem
{
    private static readonly Dictionary<string, int> repeatedNamesCountByName = new();

    public static List<Emoji> emojis = new();

    public override void Load() {
        UpdateEmojis(Main.AssetSourceController.ActiveResourcePackList);
        
        Main.AssetSourceController.OnResourcePackChange += UpdateEmojis;
    }

    public override void PostSetupContent() {
        UpdateEmojis(Main.AssetSourceController.ActiveResourcePackList);
    }

    private static void UpdateEmojis(ResourcePackList list) {
        repeatedNamesCountByName.Clear();
        
        foreach (var pack in list.EnabledPacks) {
            foreach (var asset in pack.GetContentSource().EnumerateAssets()) {
                var name = Path.GetFileNameWithoutExtension(asset);
                var alias = name;

                if (repeatedNamesCountByName.TryGetValue(name, out var count)) {
                    alias += $"~{count}";
                    repeatedNamesCountByName[name]++;
                }
                else {
                    repeatedNamesCountByName[name] = 1;
                }
                
                var emoji = new Emoji(alias, name, emojis.Count);
                
                var extension = Path.GetExtension(asset);
                var path = $"{pack.Name}/Emojis/{name}";
                path = path.Replace('\\', '/');

                emoji.Animated = extension == ".gif";
                emoji.Path = path;

                emojis.Add(emoji);
            } 
        }
    }

    public static Emoji Get(int id) {
        return emojis.Find(x => x.Id == id);
    }

    public static Emoji Get(string alias) {
        return emojis.Find(x => x.Alias == alias);
    }

    public static bool Has(int id) {
        return Get(id) != null;
    }

    public static bool Has(string alias) {
        return Get(alias) != null;
    }
}
