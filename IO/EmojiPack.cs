using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace Emojiverse.IO;

public sealed class EmojiPack
{
    private const string ManifestPath = "manifest.json";
    private const string IconPath = "icon.png";

    private readonly ZipArchive archive;

    public readonly string Path;
    
    public string Name { get; private set; }
    public string Author { get; private set; }
    
    public EmojiPack(string path) {
        if (!File.Exists(path)) {
            throw new FileNotFoundException();
        }

        Path = path;

        var stream = File.OpenRead(Path);
        archive = new ZipArchive(stream, ZipArchiveMode.Read);

        LoadManifest();
    }

    private void LoadManifest() {
        var stream = GetFromMemory(ManifestPath);
        var reader = new StreamReader(stream);

        var data = JsonConvert.DeserializeObject<EmojiPackData>(reader.ReadToEnd());

        Name = data.Name;
        Author = data.Author;
    }

    private MemoryStream GetFromMemory(string path) {
        var entry = archive.GetEntry(path);

        if (entry == null) {
            throw new FileNotFoundException();
        }

        var stream = entry.Open();
        var memoryStream = new MemoryStream();

        stream.CopyTo(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }

    private struct EmojiPackData
    {
        public string Name;
        public string Author;
    }
}
