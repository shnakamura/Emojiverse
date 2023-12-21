using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Emojiverse.Common;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    private static class EmojiverseModData<T> where T : Mod
    {
        public static readonly List<Emoji> Emojis = new();
    }
    
    public static Dictionary<string, int> RepeatedNames { get; private set; }
    
    public static Dictionary<int, Emoji> EmojisById { get; private set; }
    public static Dictionary<string, int> IdsByAlias { get; private set; }
    
    public override void Load() {
        RepeatedNames = new Dictionary<string, int>();
        
        EmojisById = new Dictionary<int, Emoji>();
        IdsByAlias = new Dictionary<string, int>();
    }

    public override void PostSetupContent() {
        LoadEmojisFromMod(this);
    }

    public override void Unload() {
        UnloadEmojisFromMod(this);
        
        RepeatedNames?.Clear();
        RepeatedNames = null;

        EmojisById?.Clear();
        EmojisById = null;

        IdsByAlias?.Clear();
        IdsByAlias = null;
    }

    public static void LoadEmojisFromMod<T>(T mod) where T : Mod {
        foreach (var file in mod.GetFileNames()) {
            if (!file.EndsWith(".rawimg") && !file.EndsWith(".png")) {
                continue;
            }

            var path = Path.ChangeExtension(file, null);

            var texture = mod.Assets.Request<Texture2D>(path);
            
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
            var emoji = new Emoji(mod, texture, name, alias, id);
        
            EmojisById[id] = emoji;
            IdsByAlias[alias] = id;
            
            EmojiverseModData<T>.Emojis.Add(emoji);
        }
    }

    public static void UnloadEmojisFromMod<T>(T mod) where T : Mod {
        foreach (var emoji in EmojiverseModData<T>.Emojis) {
            EmojisById.Remove(emoji.Id);
            IdsByAlias.Remove(emoji.Alias);
        }
        
        EmojisById.TrimExcess();
        IdsByAlias.TrimExcess();
        
        EmojiverseModData<T>.Emojis?.Clear();
    }
    
    public static bool TryGetEmoji(int id, out Emoji emoji) {
        return EmojisById.TryGetValue(id, out emoji);
    }

    public static bool TryGetId(string alias, out int id) {
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