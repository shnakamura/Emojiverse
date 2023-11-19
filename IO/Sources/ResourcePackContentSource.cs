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

        var paths = new List<string>();

        foreach (var pack in list.EnabledPacks) {
            var source = pack.GetContentSource();

            sourcesByName[pack.Name] = source;

            foreach (var asset in source.EnumerateAssets()) {
                paths.Add(Path.Combine(pack.Name, asset));
            }
        }
        
        SetAssetNames(paths);
    }

    public override Stream OpenStream(string fullAssetName) {
        var split = fullAssetName.IndexOf('\\');

        var pack = fullAssetName.Substring(0, split);
        var name = fullAssetName.Substring(split + 1);
        
        return sourcesByName[pack].OpenStream(name);
    }

    public void Clear() {
        sourcesByName.Clear();
        sourcesByName.TrimExcess();
    }
}
