using System.Collections.Generic;
using System.IO;
using ReLogic.Content.Sources;
using Terraria.IO;

namespace Emojiverse.IO.Sources;

/// <summary>
///     A content source that retrieves assets from the currently enabled resource packs.<br/>
///     Wraps asset names from 'Path/Asset' to 'ResourcePack/ath/assetName'.
/// </summary>
internal sealed class ResourcePackContentSource : ContentSource
{
    private readonly Dictionary<string, IContentSource> sourcesByName = new();

    public void Update(ResourcePackList list) {
        sourcesByName.Clear();

        var assetsWithPackName = new List<string>();

        foreach (var pack in list.EnabledPacks) {
            var source = pack.GetContentSource();

            sourcesByName[pack.FileName] = source;

            foreach (var asset in source.EnumerateAssets()) {
                assetsWithPackName.Add($"{pack.FileName}/{asset}");
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
