using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

public sealed class EmojiSnippet : TextSnippet
{
    private const string PngExtension = ".png";

    private const float Size = 20f;
    private const float Padding = 4f;

    public readonly string Name;

    public EmojiSnippet(string name) {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        Name = name;
    }

    public override void OnHover() {
        Main.instance.MouseText(Name);
    }

    public override bool UniqueDraw(bool justCheckingString, [UnscopedRef] out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default, Color color = default, float scale = 1f) {
        size = new Vector2(Size + Padding) * Scale;

        if (justCheckingString) {
            return false;
        }

        return true;
    }

    public override float GetStringLength(DynamicSpriteFont font) {
        return Size * Scale;
    }
}
