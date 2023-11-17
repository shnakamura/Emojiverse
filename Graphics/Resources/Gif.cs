using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.Graphics.Resources;

public class Gif
{
    public readonly Texture2D[] Frames;

    public readonly int FrameRate;
    public readonly int FrameCount;
    
    public Gif(Texture2D[] frames, int frameRate, int frameCount) {
        Frames = frames;
        FrameRate = frameRate;
        FrameCount = frameCount;
    }
}
