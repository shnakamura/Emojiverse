using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        var sortMode = (SpriteSortMode)SpriteBatchCache.SortMode.GetValue(spriteBatch);
        var blendState = (BlendState)SpriteBatchCache.BlendState.GetValue(spriteBatch);
        var samplerState = (SamplerState)SpriteBatchCache.SamplerState.GetValue(spriteBatch);
        var depthStencilState = (DepthStencilState)SpriteBatchCache.DepthStencilState.GetValue(spriteBatch);
        var rasterizerState = (RasterizerState)SpriteBatchCache.RasterizerState.GetValue(spriteBatch);
        var effect = (Effect)SpriteBatchCache.Effect.GetValue(spriteBatch);
        var transformMatrix = (Matrix)SpriteBatchCache.TransformMatrix.GetValue(spriteBatch);

        return new SpriteBatchSnapshot(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
    }
}
