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
        var pack = Path.GetDirectoryName(Path.GetDirectoryName(fullAssetName));
        var path = Path.Combine(Path.GetRelativePath(pack, fullAssetName));

        return sourcesByName[pack].OpenStream(path);
    }

    public void Clear() {
        sourcesByName.Clear();
        sourcesByName.TrimExcess();
    }
}
