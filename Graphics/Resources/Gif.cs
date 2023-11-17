using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.Graphics.Resources;

public sealed class Gif
{
    public Texture2D[] Frames { get; private set; }
    
    public int FrameWidth { get; private set; }
    public int FrameHeight { get; private set; }
    
    public int FrameRate { get; private set; }
    public int FrameCount { get; private set; }

    public Gif(Texture2D[] frames, int frameRate, int frameCount, int frameWidth, int frameHeight) {
        Frames = frames;
        FrameRate = frameRate;
        FrameCount = frameCount;
    }
}
