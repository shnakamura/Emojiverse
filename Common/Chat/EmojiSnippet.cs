using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Emojiverse.Common.Chat;

public sealed class EmojiSnippet : TextSnippet
{
    // private const float Size = 30f;
    // private const float SizeLimit = Size * 0.75f;
    
    private const float Padding = 4f;

    public override void OnHover() {
        Main.instance.MouseText("");
    }

    public override bool UniqueDraw(bool justCheckingString, [UnscopedRef] out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default, Color color = default, float scale = 1f) {
        var Size = 32f;
        var SizeLimit = 24f;
        
        if (!justCheckingString) {
            var texture = ModContent.Request<Texture2D>($"{nameof(Emojiverse)}/Assets/Textures/Missing").Value;
        
            var frame = texture.Frame();
            var origin = frame.Size() / 2f;
        
            var modifiedDrawScale = 1f;
        
            if ((float)frame.Width > SizeLimit || (float)frame.Height > SizeLimit) {
                modifiedDrawScale = ((frame.Width <= frame.Height) ? (SizeLimit / (float)frame.Height) : (SizeLimit / (float)frame.Width));
            }
        
            var finalDrawScale = scale * modifiedDrawScale;
            var offset = (texture.Size() * finalDrawScale) / 2f;
            
            spriteBatch.Draw(texture, position + offset, frame, Color.White, 0f, origin, finalDrawScale, SpriteEffects.None, 0f);
        }
        
        size = new Vector2(Size) * scale * 0.75f;
        
        return true;
    }

    public override float GetStringLength(DynamicSpriteFont font) {
        return 30 * Scale;
    }
}
