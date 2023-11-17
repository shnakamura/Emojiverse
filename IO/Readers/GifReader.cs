using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Emojiverse.Graphics.Resources;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Content.Readers;
using Terraria;

namespace Emojiverse.IO.Readers;

public sealed class GifReader : IAssetReader
{
    public async ValueTask<T> FromStream<T>(Stream stream, MainThreadCreationContext context) where T : class {
        if (typeof(T) != typeof(Gif)) {
            throw AssetLoadException.FromInvalidReader<GifReader, T>();
        }

        using var image = Image.FromStream(stream);

        var dimension = new FrameDimension(image.FrameDimensionsList[0]);
        var frameCount = image.GetFrameCount(dimension);

        var frames = new Texture2D[frameCount];

        for (var i = 0; i < frameCount; i++) {
            using var memoryStream = new MemoryStream();

            image.Save(memoryStream, ImageFormat.Png);
            image.SelectActiveFrame(dimension, i);

            memoryStream.Seek(0, SeekOrigin.Begin);

            await context;

            frames[i] = Texture2D.FromStream(Main.graphics.GraphicsDevice, memoryStream);
        }

        var frameDelayInfo = image.GetPropertyItem(0x5100);
        var frameDelay = BitConverter.ToInt32(frameDelayInfo.Value, 0) * 10;
        var frameRate = frameCount * 1000 / frameDelay;

        var frameWidth = image.Width;
        var frameHeight = image.Height;

        var gif = new Gif(frames, frameCount, frameRate, frameWidth, frameHeight);

        return (T)(object)gif;
    }
}
