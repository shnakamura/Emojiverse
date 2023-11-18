using System;
using System.Collections.Generic;
using System.IO;
using Emojiverse.Graphics.Resources;
using Emojiverse.IO.Readers;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Content.Sources;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Assets;

namespace Emojiverse.IO;

public sealed class EmojiCache : ModSystem
{
    private static readonly Dictionary<string, int> repeatedNamesCountByName = new();
    internal static AssetRepository packsAssets;
    static ResourcePacksContentSourceCollection source;

    public override void Load() {
        var gd = Main.instance.GraphicsDevice;
        AssetReaderCollection readers = new();
        readers.RegisterReader(new GifReader(), ".gif");
        readers.RegisterReader(new JpgReader(), ".jpg", ".jpeg");
        readers.RegisterReader(new PngReader(gd), ".png");
        readers.RegisterReader(new RawImgReader(gd), ".rawimg");
        readers.RegisterReader(new XnbReader(Main.instance.Services), ".xnb");
        source = new ResourcePacksContentSourceCollection();
        packsAssets = new AssetRepository(readers, new IContentSource[] { source });

        Main.AssetSourceController.OnResourcePackChange += ReloadFromList;
        PostSetupContent();
        On_Main.DoUpdate += On_Main_DoUpdate;
    }

    private void On_Main_DoUpdate(On_Main.orig_DoUpdate orig, Main self, ref Microsoft.Xna.Framework.GameTime gameTime) {
        orig(self, ref gameTime);
        packsAssets.TransferCompletedAssets();
    }

    public override void PostSetupContent() {
        ReloadFromList(Main.AssetSourceController.ActiveResourcePackList);
    }

    private void ReloadFromList(ResourcePackList list) {
        source.Update(list);
        foreach (var pack in list.EnabledPacks) {
            foreach (var asset in pack.GetContentSource().EnumerateAssets()) {
                var directory = Path.GetDirectoryName(asset);
                var extension = Path.GetExtension(asset);

                if (directory != "Emojis") {
                    continue;
                }

                var name = Path.GetFileNameWithoutExtension(asset);
                var alias = name;
<<<<<<< HEAD
                
=======

>>>>>>> a00dbebaff35d30426216b8dfb06cc3c9c227347
                if (!repeatedNamesCountByName.TryGetValue(name, out var count)) {
                    repeatedNamesCountByName[name] = 1;
                    continue;
                }
<<<<<<< HEAD
               
                alias += $"~{count}";
                
                repeatedNamesCountByName[name]++;
=======

>>>>>>> a00dbebaff35d30426216b8dfb06cc3c9c227347
            }
        }
    }
}


