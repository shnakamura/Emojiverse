using Emojiverse.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Common;

internal sealed class EmojiTagSnippet : TextSnippet
{
    public readonly Emoji Emoji;

    public EmojiTagSnippet(Emoji emoji) {
        Emoji = emoji;
    }

    public override void OnHover() {
        Main.instance.MouseText(Emoji.Name.SurroundWith(':'));
    }

    public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = new(), Color color = new(), float scale = 1) {
        const int Size = 20;

        if (!justCheckingString && (color.R != 0 || color.G != 0 || color.B != 0)) {
            var texture = Emoji.Texture.Value;

            var frame = texture.Frame();
            var origin = frame.Size() / 2f;

            var scaleMultiplier = EmojiverseConfig.Instance.EmojiDrawingScale;

            var area = new Vector2(Size) * scaleMultiplier;
            var offset = area * (1f / Size) / 2f + area / 2f + new Vector2(0f, 4f);
            var rectangle = new Rectangle((int)(position.X + offset.X), (int)(position.Y + offset.Y), (int)(Size * scaleMultiplier), (int)(Size * scaleMultiplier));

            spriteBatch.Draw(texture, rectangle, frame, Color.White, 0f, origin, SpriteEffects.None, 0f);
        }

        size = new Vector2(Size);

        return true;
    }
}
