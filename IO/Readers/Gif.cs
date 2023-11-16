using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.IO.Readers;

public sealed class Gif
{
    public Texture2D[] Frames { get; private set; }
    
    public int FrameRate { get; private set; }
    public int FrameCount { get; private set; }

    public Gif(Texture2D[] frames, int frameRate, int frameCount) {
        Frames = frames;
        FrameRate = frameRate;
        FrameCount = frameCount;
    }
}
