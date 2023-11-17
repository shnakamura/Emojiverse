﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.Graphics;

public static class SpriteBatchCache
{
    private const BindingFlags FlagsPrivateInstance = BindingFlags.NonPublic | BindingFlags.Instance;

    public static readonly FieldInfo SortMode;
    public static readonly FieldInfo BlendState;
    public static readonly FieldInfo SamplerState;
    public static readonly FieldInfo DepthStencilState;
    public static readonly FieldInfo RasterizerState;
    public static readonly FieldInfo Effect;
    public static readonly FieldInfo TransformMatrix;
    internal static readonly Func<SpriteBatch, SpriteBatchSnapshot> capture;

    static SpriteBatchCache() {
        SortMode = typeof(SpriteBatch).GetField("sortMode", FlagsPrivateInstance);
        BlendState = typeof(SpriteBatch).GetField("blendState", FlagsPrivateInstance);
        SamplerState = typeof(SpriteBatch).GetField("samplerState", FlagsPrivateInstance);
        DepthStencilState = typeof(SpriteBatch).GetField("depthStencilState", FlagsPrivateInstance);
        RasterizerState = typeof(SpriteBatch).GetField("rasterizerState", FlagsPrivateInstance);
        Effect = typeof(SpriteBatch).GetField("customEffect", FlagsPrivateInstance);
        TransformMatrix = typeof(SpriteBatch).GetField("transformMatrix", FlagsPrivateInstance);

        DynamicMethod method = new("SpritebatchSnapshotAccessor", typeof(SpriteBatchSnapshot), new Type[] { typeof(SpriteBatch) }, typeof(SpriteBatch));
        var il = method.GetILGenerator();
        var result = il.DeclareLocal(typeof(SpriteBatchSnapshot));
        method.DefineParameter(0, ParameterAttributes.None, "spritebatch");

        il.Emit(OpCodes.Ldloca, result);
        il.Emit(OpCodes.Ldarg_0); // spritebatch
        il.Emit(OpCodes.Ldfld, SortMode); // .sortMode
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
        il.Emit(OpCodes.Call, typeof(SpriteBatchSnapshot).GetConstructor(new Type[] { 
            typeof(SpriteSortMode), typeof(BlendState), typeof(SamplerState), typeof(DepthStencilState), typeof(RasterizerState), typeof(Effect), typeof(Matrix)
        }));
        il.Emit(OpCodes.Ldloc, result);
        il.Emit(OpCodes.Ret);

        capture = method.CreateDelegate<Func<SpriteBatch, SpriteBatchSnapshot>>();
    }

    internal static void Begin(this SpriteBatch spriteBatch, in SpriteBatchSnapshot snapshot) {
        spriteBatch.Begin(snapshot.SortMode, snapshot.BlendState, snapshot.SamplerState, snapshot.DepthStencilState, snapshot.RasterizerState, snapshot.Effect, snapshot.TransformMatrix);
    }
    internal static void Begin(this SpriteBatch spriteBatch, SpriteBatchSnapshot snapshot) {
        spriteBatch.Begin(snapshot.SortMode, snapshot.BlendState, snapshot.SamplerState, snapshot.DepthStencilState, snapshot.RasterizerState, snapshot.Effect, snapshot.TransformMatrix);
    }
}
