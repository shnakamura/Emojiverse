using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

public sealed class EmojiSnippet : TextSnippet
{
    private const float Size = 20f;
    
    public readonly Asset<Texture2D> Asset;
    public readonly string Alias;

    public EmojiSnippet(Asset<Texture2D> asset, string alias) {
        ArgumentNullException.ThrowIfNull(asset, nameof(asset));
        ArgumentNullException.ThrowIfNull(alias, nameof(alias));

        Asset = asset;
        Alias = alias;
    }

    public override void OnHover() {
        Main.instance.MouseText(Alias);
    }

    public override bool UniqueDraw(bool justCheckingString, [UnscopedRef] out Vector2 size, SpriteBatch spriteBatch, Vector2 position = new(), Color color = new(), float scale = 1) {
        var notDrawingOutline = color.R != 0 || color.G != 0 || color.B != 0;

        if (!justCheckingString && notDrawingOutline) {
            var texture = Asset.Value;
            var frame = texture.Frame();
            var origin = frame.Size() / 2f;

            var area = new Vector2(Size);
            var offset = area * (1f / Size) / 2f + area / 2f + new Vector2(0f, 4f);
            var rectangle = new Rectangle((int)(position.X + offset.X), (int)(position.Y + offset.Y), (int)Size, (int)Size);

            spriteBatch.Draw(texture, rectangle, frame, color, 0f, origin, SpriteEffects.None, 0f);
        }

        size = new Vector2(Size);

        return true;
    }
}
