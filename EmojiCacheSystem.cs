using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace Emojiverse;

class EmojiCacheSystem : ModSystem
{
    private const string PngExtension = ".png";
    private const string GifExtension = ".gif";
    static DirectoryInfo emojiVerseDirectory;
    static FileSystemWatcher fileWatcher;
    static ConcurrentDictionary<string, Asset<Texture2D>> emojiImages = new();
    static CancellationTokenSource fileWatcherCancelTokenSource;

    public static Asset<Texture2D> GetEmojiImage(string name) {
        if (!emojiImages.TryGetValue(name, out var asset) && LoadImage(name) is { } newImage) {
            emojiImages[name] = asset = newImage;
        }
        return asset;
    }

    static Asset<Texture2D> LoadImage(string name) {
        var directory = new DirectoryInfo(Emojiverse.EmojiPath);
        var matchingFiles = directory.GetFiles(name + ".*");

        if (matchingFiles.Length <= 0) {
            return null;
        }

        var path = matchingFiles[0].FullName;
        var extension = Path.GetExtension(path);

        if (extension == PngExtension) {
            MemoryStream ms = new(File.ReadAllBytes(path));
            return ModContent.GetInstance<Emojiverse>().Assets.CreateUntracked<Texture2D>(ms, name + extension, AssetRequestMode.AsyncLoad);
        }
        return null;
    }

    internal static void RemoveImage(string name) {
        if (name == null) 
            return;
        emojiImages.TryRemove(name, out var asset);
        if (asset != null)
            Main.QueueMainThreadAction(asset.Dispose);
    }

    public override void Load() {
        emojiVerseDirectory = new DirectoryInfo(Emojiverse.EmojiPath);
        PostUpdateEverything();
    }

    public override void Unload() {
        base.Unload();
        CancelAndDispose();
        if (emojiImages != null) {
            foreach ((var key, var asset) in emojiImages)
                Main.QueueMainThreadAction(asset.Dispose);
            emojiImages.Clear();
        }
        emojiImages = null;
        emojiVerseDirectory = null;
    }

    public override void PostUpdateEverything() {
        base.PostUpdateEverything();
        if (EmojiverseConfig.Instance.DisableFileWatcher) {
            CancelAndDispose();
        }
        else if (fileWatcher == null) {
            Restart();
        }
    }

    private void CancelAndDispose() {
        if (fileWatcherCancelTokenSource != null && !fileWatcherCancelTokenSource.IsCancellationRequested) {
            try { fileWatcherCancelTokenSource.Cancel(); }
            catch (ObjectDisposedException) { }
            fileWatcherCancelTokenSource.Dispose();
        }
        fileWatcherCancelTokenSource = null;
        if (fileWatcher != null) {
            fileWatcher.Dispose();
        }
        fileWatcher = null;
    }

    private void Restart() {
        CancelAndDispose();
        fileWatcherCancelTokenSource = new();

        fileWatcher = new(emojiVerseDirectory.FullName);

        Task.Run(()=> WatcherTask(fileWatcherCancelTokenSource.Token));
    }

    static void WatcherTask(CancellationToken token) {
        Console.WriteLine("Emojiverse file watcher started");

        try {
            while (!token.IsCancellationRequested) {
                var result = fileWatcher.WaitForChanged(WatcherChangeTypes.All, 9000);
                if (result.TimedOut || token.IsCancellationRequested) {
                    continue;
                }
                if (result.ChangeType == WatcherChangeTypes.Renamed) {
                    // the old file would also have to be deleted from cache
                    RemoveImage(Path.GetFileNameWithoutExtension(result.OldName));
                }
                RemoveImage(Path.GetFileNameWithoutExtension(result.Name));
            }
        }
        catch {
            // maybe log?
        }

        Console.WriteLine("Emojiverse file watcher ended");
    }

    private void FileWatcher_Error(object sender, ErrorEventArgs e) {
        Debug.Assert(ReferenceEquals(sender, fileWatcher), "nullfying because of a different object?");
        fileWatcher?.Dispose();
        fileWatcher = null;
        // log when there's an error here?
    }
}
