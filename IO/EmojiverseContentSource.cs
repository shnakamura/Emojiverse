using System.Collections.Generic;
using System.IO;
using ReLogic.Content.Sources;
using Terraria.IO;

namespace Emojiverse.IO;

/// <summary>
/// A content source that retrieves assets from the currently enabled resource packs.<br />
/// Wraps asset names from 'path/assetName' to 'ResourcePack/path/assetName'.
/// </summary>
internal sealed class EmojiverseContentSource : ContentSource
{
    private readonly Dictionary<string, IContentSource> sources = new();

    public void Update(ResourcePackList list) {
        sources.Clear();

        var assetsWithPackName = new List<string>();
        
        foreach (var pack in list.EnabledPacks) {
            var source = pack.GetContentSource();
            
            sources[pack.Name] = source;
            
            foreach (var assetName in source.EnumerateAssets()) {
                assetsWithPackName.Add($"{pack.Name}/{assetName}");
            }
        }

        SetAssetNames(assetsWithPackName);
    }
    
    public override Stream OpenStream(string fullAssetName) {
        var split = fullAssetName.IndexOf('\\');
        
        var pack = fullAssetName.Substring(0, split);
        var name = fullAssetName.Substring(split + 1);
        
        return sources[pack].OpenStream(name);
    }
}


