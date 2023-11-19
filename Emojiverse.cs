using Emojiverse.IO.Readers;
using Emojiverse.IO.Sources;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using ReLogic.Content.Sources;
using ReLogic.Utilities;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public static ResourcePackContentSource Source { get; private set; }
    public static AssetRepository Assets { get; private set; }

    public override void Load() {
        var device = Main.instance.GraphicsDevice;
        var readers = Main.instance.Services.Get<AssetReaderCollection>();

        readers.RegisterReader(new GifReader(device), ".gif");
        readers.RegisterReader(new JpgReader(device), ".jpg", ".jpeg", ".jpe");

        Source = new ResourcePackContentSource();
        Assets = new AssetRepository(
            readers,
            new IContentSource[] {
                Source
            }
        );
        
        UpdateSource(Main.AssetSourceController.ActiveResourcePackList);

        On_Main.DoUpdate += DoUpdateHook;
        
        Main.AssetSourceController.OnResourcePackChange += UpdateSource;
    }

    private static void UpdateSource(ResourcePackList list) {
        Source.Update(list);
    }

    private static void DoUpdateHook(On_Main.orig_DoUpdate orig, Main self, ref GameTime gameTime) {
        orig(self, ref gameTime);

        Assets.TransferCompletedAssets();
    }
}