using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Emojiverse.Graphics.Resources;

public sealed class Gif : IDisposable
{
    public readonly Texture2D[] Frames;

    public readonly int FrameRate;
    public readonly int FrameCount;
    
    public Texture2D CurrentFrame { get; private set; }
    
    public bool IsDisposed { get; private set; }

    internal bool OwnsTextures { get; set; }
    
    public Gif(Texture2D[] frames, int frameRate, int frameCount) {
        Frames = frames;
        FrameRate = frameRate;
        FrameCount = frameCount;
    }

    private void Dispose(bool disposing) {
        if (IsDisposed) {
            return;
        }

        if (disposing) {
            if (OwnsTextures) {
                foreach (var texture in Frames)
                    texture.Dispose();
            }
        }

        IsDisposed = true;
    }

    public void Dispose() {
        Dispose(true);
        
        GC.SuppressFinalize(this);
    }

    ~Gif() {
        Dispose(false);
    }
}
