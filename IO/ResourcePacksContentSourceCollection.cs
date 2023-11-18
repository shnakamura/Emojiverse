using System.Collections.Generic;
using System.IO;
using ReLogic.Content.Sources;
using Terraria.IO;

namespace Emojiverse.IO;

/// <summary>
/// A content source that retrieves assets from the currently enabled resource packs.<br />
/// Wraps asset names from 'path/assetName' to 'ResourcePack/path/assetName'.
/// </summary>
class ResourcePacksContentSourceCollection : ContentSource
{
    Dictionary<string, IContentSource> sources = new();

    public ResourcePacksContentSourceCollection() {
    }

    public void Update(ResourcePackList packs) {
        sources.Clear();

        List<string> assetsWithPackName = new();
        foreach (var pack in packs.EnabledPacks) {
            var source = sources[pack.Name] = pack.GetContentSource();

            foreach (var assetName in source.EnumerateAssets()) {
                // TODO: filter emojis only?
                assetsWithPackName.Add($"{pack.Name}/{assetName}");
            }
        }

        SetAssetNames(assetsWithPackName);
    }
    // probably no locks needed here, the pack the asset comes from is (probably) threadsafe
    public override Stream OpenStream(string fullAssetName) {
        int split = fullAssetName.IndexOf('\\');
        string packOrigin = fullAssetName.Substring(0, split);
        string assetName = fullAssetName.Substring(split + 1);
        return sources[packOrigin].OpenStream(assetName);
    }
}


