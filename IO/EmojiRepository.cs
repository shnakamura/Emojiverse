using Emojiverse.IO.Readers;
using Emojiverse.IO.Sources;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using ReLogic.Content.Sources;
using ReLogic.Utilities;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse.IO;

public sealed class EmojiRepository : ModSystem
{
    internal static AssetRepository Assets { get; private set; }
    internal static ResourcePackContentSource Source { get; private set; }

    public override void Load() {
        On_Main.DoUpdate += DoUpdateHook;

        Main.AssetSourceController.OnResourcePackChange += UpdateSource;
    }

    public override void PostSetupContent() {
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

        Source.Update(Main.AssetSourceController.ActiveResourcePackList);
    }

    private static void UpdateSource(ResourcePackList list) {
        Source.Update(list);
    }

    private static void DoUpdateHook(On_Main.orig_DoUpdate orig, Main self, ref GameTime gameTime) {
        orig(self, ref gameTime);

        Assets.TransferCompletedAssets();
    }
}
