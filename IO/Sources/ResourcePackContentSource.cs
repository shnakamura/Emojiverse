using System.Collections.Generic;
using System.IO;
using ReLogic.Content.Sources;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse.IO.Sources;

public sealed class ResourcePackContentSource : ContentSource
{
    private readonly Dictionary<string, IContentSource> sourcesByName = new();

    public void Update(ResourcePackList list) {
        sourcesByName.Clear();

        var assetsWithPackName = new List<string>();

        foreach (var pack in list.EnabledPacks) {
            var source = pack.GetContentSource();

            sourcesByName[pack.Name] = source;

            foreach (var asset in source.EnumerateAssets()) {
                var path = $"{pack.Name}/{Path.GetFileNameWithoutExtension(asset)}";
                path = path.Replace('\\', '/');
                
                assetsWithPackName.Add(path);

                
                ModContent.GetInstance<Emojiverse>().Logger.Debug($"Asset: {asset} @ Pack: {pack.Name} @ Path: {path}");
            }
        }

        SetAssetNames(assetsWithPackName);
    }

    public override Stream OpenStream(string fullAssetName) {
        var split = fullAssetName.IndexOf('\\');

        var pack = fullAssetName.Substring(0, split);
        var name = fullAssetName.Substring(split + 1);

        return sourcesByName[pack].OpenStream(name);
    }
}
