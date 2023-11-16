using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Emojiverse.IO.Readers;

public sealed class JpgReader : IReader<Texture2D>
{
    public string[] Extensions { get; } = new[] {
        ".jpg",
        ".jpeg"
    };
    
    public Texture2D Read(string path) {
        using var stream = new FileStream(path, FileMode.Open);
        var device = Main.graphics.GraphicsDevice;
        
        return Texture2D.FromStream(device, stream);
    }
}
