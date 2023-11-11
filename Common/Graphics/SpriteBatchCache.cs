using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.Common.Graphics;

public sealed class SpriteBatchCache
{
    private const BindingFlags FlagsPrivateInstance = BindingFlags.NonPublic | BindingFlags.Instance;

    public static readonly FieldInfo SortMode;
    public static readonly FieldInfo BlendState;
    public static readonly FieldInfo SamplerState;
    public static readonly FieldInfo DepthStencilState;
    public static readonly FieldInfo RasterizerState;
    public static readonly FieldInfo Effect;
    public static readonly FieldInfo TransformMatrix;

    static SpriteBatchCache() {
        SortMode = typeof(SpriteBatch).GetField("sortMode", FlagsPrivateInstance);
        BlendState = typeof(SpriteBatch).GetField("blendState", FlagsPrivateInstance);
        SamplerState = typeof(SpriteBatch).GetField("samplerState", FlagsPrivateInstance);
        DepthStencilState = typeof(SpriteBatch).GetField("depthStencilState", FlagsPrivateInstance);
        RasterizerState = typeof(SpriteBatch).GetField("rasterizerState", FlagsPrivateInstance);
        Effect = typeof(SpriteBatch).GetField("customEffect", FlagsPrivateInstance);
        TransformMatrix = typeof(SpriteBatch).GetField("transformMatrix", FlagsPrivateInstance);
    }
}