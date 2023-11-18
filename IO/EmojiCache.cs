using System;
using System.Collections.Generic;
using System.IO;
using Emojiverse.IO.Readers;
using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Content.Sources;
using ReLogic.Utilities;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Assets;
using Microsoft.Xna.Framework;

namespace Emojiverse.IO;

public sealed class EmojiCache : ModSystem
{
    internal static AssetRepository Assets { get; private set; }
    internal static EmojiverseContentSource Source { get; private set; }
    
    private static readonly Dictionary<string, int> repeatedNamesCountByName = new();

    public override void Load() {
        var device = Main.instance.GraphicsDevice;
        var readers = Main.instance.Services.Get<AssetReaderCollection>();
        
        readers.RegisterReader(new GifReader(device), ".gif");
        readers.RegisterReader(new JpgReader(device), ".jpg", ".jpeg", ".jpe");
        
        Source = new EmojiverseContentSource();
        Assets = new AssetRepository(readers, new IContentSource[] { Source });

        Main.AssetSourceController.OnResourcePackChange += ReloadFromList;
        ReloadFromList(Main.AssetSourceController.ActiveResourcePackList);
        On_Main.DoUpdate += DoUpdateHook;
    }

    public override void PostSetupContent() {
        ReloadFromList(Main.AssetSourceController.ActiveResourcePackList);
    }

    private static void ReloadFromList(ResourcePackList list) {
        Source.Update(list);
        
        foreach (var pack in list.EnabledPacks) {
            foreach (var asset in pack.GetContentSource().EnumerateAssets()) {
                var directory = Path.GetDirectoryName(asset);
                var extension = Path.GetExtension(asset);

                if (directory != "Emojis") {
                    continue;
                }

                var name = Path.GetFileNameWithoutExtension(asset);
                var alias = name;

                if (!repeatedNamesCountByName.TryGetValue(name, out var count)) {
                    repeatedNamesCountByName[name] = 1;
                    continue;
                }

                alias += $"~{count}";
                repeatedNamesCountByName[name]++;
            }
        }
    }
    
    private static void DoUpdateHook(On_Main.orig_DoUpdate orig, Main self, ref GameTime gameTime) {
        orig(self, ref gameTime);
        
        Assets.TransferCompletedAssets();
    }
}
