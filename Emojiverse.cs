<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.IO;
using Emojiverse.IO.Readers;
using Ionic.Zip;
=======
﻿using Emojiverse.Graphics.Resources;
using Emojiverse.IO;
using Emojiverse.IO.Readers;
>>>>>>> a00dbebaff35d30426216b8dfb06cc3c9c227347
using ReLogic.Content;
using ReLogic.Content.Sources;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public override IContentSource CreateDefaultContentSource() {
        var collection = Main.instance.Services.Get<AssetReaderCollection>();

        collection.RegisterReader(new GifReader(), ".gif");
        collection.RegisterReader(new JpgReader(), ".jpg", ".jpeg");

        return new EmojiverseContentSource();
    }
}

public sealed class EmojiverseContentSource : ContentSource
{
    public EmojiverseContentSource() {
        if (Main.dedServ) {
            return;
        }
        
        var list = Main.AssetSourceController.ActiveResourcePackList;
        var names = new List<string>();
        
        foreach (var pack in list.EnabledPacks) {
            foreach (var asset in pack.GetContentSource().EnumerateAssets()) {
                var name = asset.Replace("Emojis/", $"{pack.FileName}/");
                
                names.Add(name);
                
                ModContent.GetInstance<Emojiverse>().Logger.Info($"Emoji asset set with name `{name}.`");
            }
        }

        SetAssetNames(names);
    }
    
    public override Stream OpenStream(string assetName) {
        try {
            if (!File.Exists(assetName)) {
                var path = Path.GetDirectoryName(assetName);
                
                var entries = new Dictionary<string, ZipEntry>();
                var file = ZipFile.Read(path);
                
                if (!entries.TryGetValue(path, out var value)) {
                    throw AssetLoadException.FromMissingAsset(path);
                }
                
                var memoryStream = new MemoryStream((int)value.UncompressedSize);
                
                lock (file) {
                    value.Extract((Stream)memoryStream);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                
                return memoryStream;
            }
            else {
                return File.OpenRead(assetName);
            }
        }
        catch (Exception innerException) {
            throw AssetLoadException.FromMissingAsset(assetName, innerException);
        }
    }
    public override void Load() {
        base.Load();
    }
}
