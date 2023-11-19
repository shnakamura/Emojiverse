using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace Emojiverse.Graphics.Snapshots;

public sealed class SpriteBatchCache : ModSystem
{
    private const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance;
    
    public static readonly FieldInfo SortMode = typeof(SpriteBatch).GetField("sortMode", Flags);
    public static readonly FieldInfo BlendState = typeof(SpriteBatch).GetField("blendState", Flags);
    public static readonly FieldInfo SamplerState = typeof(SpriteBatch).GetField("samplerState", Flags);
    public static readonly FieldInfo DepthStencilState = typeof(SpriteBatch).GetField("depthStencilState", Flags);
    public static readonly FieldInfo RasterizerState = typeof(SpriteBatch).GetField("rasterizerState", Flags);
    public static readonly FieldInfo Effect = typeof(SpriteBatch).GetField("customEffect", Flags);
    public static readonly FieldInfo TransformMatrix = typeof(SpriteBatch).GetField("transformMatrix", Flags);
    
    public static Func<SpriteBatch, SpriteBatchSnapshot> Capture { get; private set; }

    public override void Load() {
        var method = new DynamicMethod(
            "SpritebatchSnapshotAccessor",
            typeof(SpriteBatchSnapshot),
            new[] {
                typeof(SpriteBatch)
            },
            typeof(SpriteBatch)
        );
        
        method.DefineParameter(0, ParameterAttributes.None, "spriteBatch");

        var il = method.GetILGenerator();
        var result = il.DeclareLocal(typeof(SpriteBatchSnapshot));

        il.Emit(OpCodes.Ldloca, result);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, SortMode); 
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, BlendState);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, SamplerState);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, DepthStencilState);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, RasterizerState);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, Effect);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, TransformMatrix);
        il.Emit(
            OpCodes.Call,
            typeof(SpriteBatchSnapshot).GetConstructor(
                new[] {
                    typeof(SpriteSortMode),
                    typeof(BlendState),
                    typeof(SamplerState),
                    typeof(DepthStencilState),
                    typeof(RasterizerState),
                    typeof(Effect),
                    typeof(Matrix)
                }
            )
        );
        il.Emit(OpCodes.Ldloc, result);
        il.Emit(OpCodes.Ret);

        Capture = method.CreateDelegate<Func<SpriteBatch, SpriteBatchSnapshot>>();
    }
    public override void Unload() {
        Capture = null;
    }
}
