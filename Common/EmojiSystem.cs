using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace Emojiverse.Common;

[Autoload(Side = ModSide.Client)]
public sealed class EmojiSystem : ModSystem
{
    internal static class EmojiModData<T> where T : Mod
    {
        public static List<Emoji> Emojis { get; internal set; } = new();

        public static bool IsLoaded;
    }

    public static Dictionary<string, int> RepeatedNameLookup { get; private set; } = new();

    public static List<Emoji> Emojis { get; private set; } = new();

    public override void PostSetupContent() {
        LoadEmojisFromMod(Mod, "Assets/Emojis");
    }

    public override void Unload() {
        UnloadEmojisFromMod(Mod);

        RepeatedNameLookup?.Clear();
        RepeatedNameLookup = null;

        Emojis?.Clear();
        Emojis = null;
    }

    public static void LoadEmojisFromMod<T>(T mod, string rootDirectory) where T : Mod {
        if (EmojiModData<T>.IsLoaded) {
            return;
        }
        
        foreach (var file in mod.GetFileNames()) {
            if (!file.EndsWith(".rawimg") || !file.Contains(rootDirectory)) {
                continue;
            }

            var path = Path.ChangeExtension(file, null);
            
            var texture = mod.Assets.Request<Texture2D>(path);

            var name = Path.GetFileNameWithoutExtension(path);
            var alias = name;

            if (RepeatedNameLookup.TryGetValue(name, out var count)) {
                alias += $"~{count}";
                RepeatedNameLookup[name]++;
            }
            else {
                RepeatedNameLookup[name] = 1;
            }

            var emoji = new Emoji(mod, texture, name, alias, Emojis.Count);

            Emojis.Add(emoji);
            
            EmojiModData<T>.Emojis.Add(emoji);
        }

        EmojiModData<T>.IsLoaded = true;
    }

    public static void UnloadEmojisFromMod<T>(T mod) where T : Mod {
        if (!EmojiModData<T>.IsLoaded) {
            return;
        }
        
        foreach (var emoji in EmojiModData<T>.Emojis) {
            Emojis.Remove(emoji);
        }
        
        EmojiModData<T>.Emojis?.Clear();
        EmojiModData<T>.Emojis = null;

        EmojiModData<T>.IsLoaded = false;
    }

    public static bool TryGetEmoji(int id, out Emoji emoji) {
        foreach (var iterator in Emojis) {
            if (iterator.Id == id) {
                emoji = iterator;
                return true;
            }
        }
        
        emoji = default;
        return false;
    }
    
    public static bool TryGetId(string alias, [NotNullWhen(true)] out int? id) {
        foreach (var iterator in Emojis) {
            if (iterator.Alias == alias) {
                id = iterator.Id;
                return true;
            }
        }

        id = null;
        return false;
    }
}
