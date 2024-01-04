using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace Emojiverse.Common;

[Autoload(Side = ModSide.Client)]
public sealed class EmojiSystem : ModSystem
{
    private static class EmojiModData<T> where T : Mod
    {
        public static bool IsLoaded;
        public static List<Emoji> Emojis { get; internal set; } = new();
    }

    /// <summary>
    ///     Represents a lookup that provides how many times an emoji with X name has already been added.
    /// </summary>
    public static Dictionary<string, int> RepeatedNameLookup { get; private set; } = new();

    /// <summary>
    ///     Represents a collection of all the currently loaded emojis.
    /// </summary>
    public static List<Emoji> Emojis { get; private set; } = new();

    public override void PostSetupContent() {
        try {
            LoadEmojisFromMod(Mod, "Assets/Emojis");
        }
        catch (Exception exception) {
            Mod.Logger.Error(exception.Message, exception);
        }
    }

    public override void Unload() {
        try {
            UnloadEmojisFromMod(Mod);

            RepeatedNameLookup?.Clear();
            RepeatedNameLookup = null;

            Emojis?.Clear();
            Emojis = null;
        }
        catch (Exception exception) {
            Mod.Logger.Error(exception.Message, exception);
        }
    }

    /// <summary>
    ///     Attempts to load and register all emojis from a mod, given a root directory for search.
    /// </summary>
    /// <param name="mod">The mod instance from which emojis will be unloaded.</param>
    /// <param name="rootDirectory">The root directory for the search.</param>
    /// <typeparam name="T">The type of the mod.</typeparam>
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

            var emoji = new Emoji(mod, texture, name, alias, $"{mod.Name}:{name}");

            mod.Logger.Debug(emoji.Alias);

            Emojis.Add(emoji);

            EmojiModData<T>.Emojis.Add(emoji);
        }

        EmojiModData<T>.IsLoaded = true;
    }

    /// <summary>
    ///     Attempts to unload all emojis registered from a mod.
    /// </summary>
    /// <param name="mod">The mod instance from which emojis will be unloaded.</param>
    /// <typeparam name="T">The type of the mod.</typeparam>
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

    /// <summary>
    ///     Attempts to retrieve an emoji instance from a given identity.
    /// </summary>
    /// <param name="id">The identity search parameter.</param>
    /// <param name="emoji">The emoji found.</param>
    /// <returns>Whether an emoji with the identity specified exists or not.</returns>
    public static bool TryGetEmoji(string id, out Emoji emoji) {
        foreach (var iterator in Emojis) {
            if (iterator.Id == id) {
                emoji = iterator;
                return true;
            }
        }

        emoji = default;
        return false;
    }
}
