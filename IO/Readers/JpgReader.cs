using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Content.Readers;
using Terraria;

namespace Emojiverse.IO.Readers;

public sealed class JpgReader : IAssetReader
{    
    public readonly GraphicsDevice Device;
    
    public JpgReader(GraphicsDevice device) {
        Device = device;
    }
    
    public async ValueTask<T> FromStream<T>(Stream stream, MainThreadCreationContext context) where T : class {
        if (typeof(T) != typeof(Texture2D)) {
            throw AssetLoadException.FromInvalidReader<JpgReader, T>();
        }

        await context;

        return (T)(object)Texture2D.FromStream(Device, stream);
    }
}
