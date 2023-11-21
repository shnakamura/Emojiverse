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
    public static Dictionary<string, int> RepeatedNames { get; private set; }
    
    public static Dictionary<int, Emoji> EmojisById { get; private set; }
    public static Dictionary<string, int> IdsByAlias { get; private set; }
    
    public override void Load() {
        RepeatedNames = new Dictionary<string, int>();
        
        EmojisById = new Dictionary<int, Emoji>();
        IdsByAlias = new Dictionary<string, int>();
        
        UpdateEmojis(Main.AssetSourceController.ActiveResourcePackList);
        
        Main.AssetSourceController.OnResourcePackChange += UpdateEmojis;
    }

    public override void Unload() {
        RepeatedNames.Clear();
        RepeatedNames = null;
        
        EmojisById.Clear();
        EmojisById = null;
    
        IdsByAlias.Clear();
        IdsByAlias = null;
        
        Main.AssetSourceController.OnResourcePackChange -= UpdateEmojis; 
    }

    private static void UpdateEmojis(ResourcePackList list) {
        RepeatedNames.Clear();
        RepeatedNames.TrimExcess();
        
        EmojisById.Clear();
        EmojisById.TrimExcess();
        
        IdsByAlias.Clear();
        IdsByAlias.TrimExcess();
        
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
        
        var id = EmojisById.Count;
        var fixedPath = Path.ChangeExtension(path, null);
        var pack = Path.GetDirectoryName(Path.GetDirectoryName(path));
        
        var extension = Path.GetExtension(path);

        var emoji = new Emoji(name, alias, fixedPath, pack, id, extension == ".gif");
        
        EmojisById[id] = emoji;
        IdsByAlias[alias] = id;
    }

    public static bool TryGetEmoji(int id, [MaybeNullWhen(false)] out Emoji emoji) {
        return EmojisById.TryGetValue(id, out emoji);
    }

    public static bool TryGetId(string alias, [MaybeNullWhen(false)] out int id) {
        return IdsByAlias.TryGetValue(alias, out id);
    }

    public static bool HasEmoji(int id) {
        return TryGetEmoji(id, out _);
    }

    public static bool HasAnyEmoji() {
        return EmojisById.Count > 0;
    }

    public static IEnumerable<Emoji> EnumerateEmojis() {
        return EmojisById.Values;
    }
}
