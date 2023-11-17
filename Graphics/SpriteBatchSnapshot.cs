using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Emojiverse.Graphics;

public readonly record struct SpriteBatchSnapshot(
    SpriteSortMode SortMode,
    BlendState BlendState,
    SamplerState SamplerState,
    DepthStencilState DepthStencilState,
    RasterizerState RasterizerState,
    Effect Effect,
    Matrix TransformMatrix
)
{
    public static SpriteBatchSnapshot Capture(SpriteBatch spriteBatch)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        return SpriteBatchCache.capture(spriteBatch);
    }
}
