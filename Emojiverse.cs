using Emojiverse.IO.Readers;
using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public Emojiverse() {
        var collection = Main.instance.Services.Get<AssetReaderCollection>();

        TryAddReader<GifReader>(collection, ".gif");
        TryAddReader<JpgReader>(collection, ".jpg", ".jpeg");
    }

    private static bool TryAddReader<T>(AssetReaderCollection collection, params string[] extensions) where T : IAssetReader, new() {
        var hasReader = false;
        
        foreach (var extension in extensions) {
            if (collection.TryGetReader(extension, out var reader) && reader is T) {
                hasReader = true;
                return true;
            }
        }

        if (!hasReader) {
            collection.RegisterReader(new T(), extensions);
            return true;
        }

        return false;
    }
}
