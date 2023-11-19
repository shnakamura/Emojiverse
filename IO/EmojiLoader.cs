using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse.IO;

public sealed class EmojiLoader : ModSystem
{
    public static Dictionary<int, Emoji> Emojis { get; set; } = new();
    public static Dictionary<string, int> RepeatedNames { get; set; } = new();
    
    public override void Load() {
        UpdateEmojis(Main.AssetSourceController.ActiveResourcePackList);
        
        Main.AssetSourceController.OnResourcePackChange += UpdateEmojis;
    }

    public override void Unload() {
        Main.AssetSourceController.OnResourcePackChange -= UpdateEmojis; 
        
        Emojis.Clear();
        Emojis = null;
        
        RepeatedNames.Clear();
        RepeatedNames = null;
    }

    private static void UpdateEmojis(ResourcePackList list) {
        Emojis.Clear();
        RepeatedNames.Clear();
        
        foreach (var pack in list.EnabledPacks) {
            foreach (var asset in pack.GetContentSource().EnumerateAssets()) {
                Register(Path.Combine(pack.Name, Path.GetDirectoryName(asset), Path.GetFileNameWithoutExtension(asset)));
            } 
        }
    }

    private static void Register(string path) {
        var name = Path.GetFileNameWithoutExtension(path);
        var alias = path;   

        if (RepeatedNames.TryGetValue(name, out var count)) {
            alias += $"~{count}";
            RepeatedNames[name]++;
        }
        else {
            RepeatedNames[name] = 1;        
        }

        var id = Emojis.Count;

        Emojis[id] = new Emoji(alias, name, path, id);
    }

    public static bool TryGet(int id, [MaybeNullWhen(false)] out Emoji emoji) {
        return Emojis.TryGetValue(id, out emoji);
    }

    public static bool Has(int id) {
        return TryGet(id, out _);
    }

    public static IEnumerable<Emoji> Enumerate() {
        return Emojis.Values;
    }
}
