using Emojiverse.Graphics.Snapshots;
using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.Utilities.Extensions;

public static class SpriteBatchExtensions
{
    public static void Begin(this SpriteBatch spriteBatch, in SpriteBatchSnapshot snapshot) {
        spriteBatch.Begin(snapshot.SortMode,
            snapshot.BlendState,
            snapshot.SamplerState,
            snapshot.DepthStencilState,
            snapshot.RasterizerState,
            snapshot.Effect,
            snapshot.TransformMatrix);
    }
}
