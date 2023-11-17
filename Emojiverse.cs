using Emojiverse.IO.Readers;
using ReLogic.Content;
using ReLogic.Content.Sources;
using ReLogic.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public override IContentSource CreateDefaultContentSource() {
        var collection = Main.instance.Services.Get<AssetReaderCollection>();

        collection.RegisterReader(new GifReader(), ".gif");
        collection.RegisterReader(new JpgReader(), ".jpg", ".jpeg");

        return base.CreateDefaultContentSource();
    }
}
