using System.Diagnostics.CodeAnalysis;
using Emojiverse.Graphics;
using Emojiverse.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Chat;

public sealed class EmojiTagSnippet : TextSnippet
{
    public readonly Emoji Emoji;
    
    public EmojiTagSnippet(Emoji emoji) {
        Emoji = emoji;
    }

    public override void OnHover() {
        Main.instance.MouseText(Emoji.Name);
    }

    public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = new(), Color color = new(), float scale = 1) {
        const int Size = 20;

        var notDrawingOutline = color.R != 0 || color.G != 0 || color.B != 0;

        if (!justCheckingString && notDrawingOutline) {
            var texture = Main.Assets.Request<Texture2D>("Emojis/" + Emoji.Name).Value;

            var frame = texture.Frame();
            var origin = frame.Size() / 2f;

            var area = new Vector2(Size);
            var offset = area * (1f / Size) / 2f + area / 2f + new Vector2(0f, 4f);
            var rectangle = new Rectangle((int)(position.X + offset.X), (int)(position.Y + offset.Y), (int)(Size), (int)(Size));

            var snapshot = SpriteBatchSnapshot.Capture(spriteBatch);
            var modifiedSnapshot = snapshot with {
                SortMode = SpriteSortMode.Texture,
                SamplerState = SamplerState.PointClamp
            };
            
            spriteBatch.End();
            spriteBatch.Begin(in modifiedSnapshot);
            
            spriteBatch.Draw(texture, rectangle, frame, Color.White, 0f, origin, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(in snapshot);
        }

        size = new Vector2(Size);

        return true;
    }
}
