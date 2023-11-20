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
    public static Dictionary<int, Emoji> Emojis { get; private set; }
    public static Dictionary<string, int> RepeatedNames { get; private set; }
    
    public override void Load() {
        Emojis = new Dictionary<int, Emoji>();
        RepeatedNames = new Dictionary<string, int>();
        
        UpdateEmojis(Main.AssetSourceController.ActiveResourcePackList);
        
        Main.AssetSourceController.OnResourcePackChange += UpdateEmojis;
    }

    public override void Unload() {
        Emojis.Clear();
        Emojis = null;
        
        RepeatedNames.Clear();
        RepeatedNames = null;
        
        Main.AssetSourceController.OnResourcePackChange -= UpdateEmojis; 
    }

    private static void UpdateEmojis(ResourcePackList list) {
        Emojis.Clear();
        Emojis.TrimExcess();
        
        RepeatedNames.Clear();
        RepeatedNames.TrimExcess();
        
        foreach (var pack in list.EnabledPacks) {
            foreach (var asset in pack.GetContentSource().EnumerateAssets()) {
                Register(Path.Combine(pack.Name, Path.GetDirectoryName(asset), Path.GetFileName(asset)));
            } 
        }
    }

    private static void Register(string path) {
        var name = Path.GetFileNameWithoutExtension(path);
        var alias = name;   

        if (RepeatedNames.TryGetValue(name, out var count)) {
            alias += $"~{count}";
            RepeatedNames[name]++;
        }
        else {
            RepeatedNames[name] = 1;        
        }

        var fixedPath = Path.ChangeExtension(path, null);
        
        var id = Emojis.Count;
        var extension = Path.GetExtension(path);
        
        Emojis[id] = new Emoji(alias, name, fixedPath, id, extension == ".gif");
    }

    public static bool TryGet(int id, [MaybeNullWhen(false)] out Emoji emoji) {
        return Emojis.TryGetValue(id, out emoji);
    }

    public static bool Has(int id) {
        return TryGet(id, out _);
    }

    public static bool HasAny() {
        return Emojis.Count > 0;
    }

    public static IEnumerable<Emoji> Enumerate() {
        return Emojis.Values;
    }
}
