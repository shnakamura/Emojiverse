using System.Diagnostics.CodeAnalysis;
using Emojiverse.Graphics;
using Emojiverse.Graphics.Resources;
using Emojiverse.Graphics.Snapshots;
using Emojiverse.IO;
using Emojiverse.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse.Chat;

public sealed class EmojiTagSnippet : TextSnippet
{
    private const int Size = 20;

    public Emoji Emoji { get; }

    public int Frame { get; private set; }
    public int FrameCounter { get; private set; }

    public EmojiTagSnippet(Emoji emoji) {
        Emoji = emoji;
    }

    public override void OnHover() {
        Main.instance.MouseText(Emoji.Name.SurroundWith(':'));
    }

    public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = new(), Color color = new(), float scale = 1) {
        var notDrawingOutline = color.R != 0 || color.G != 0 || color.B != 0;

        if (!justCheckingString && notDrawingOutline) {
            var texture = Asset<Texture2D>.Empty.Value;

            if (Emoji.Animated) {
                var gif = EmojiRepository.Assets.Request<GIF>(Emoji.Path).Value;

                FrameCounter++;

                if (FrameCounter >= 1f / gif.FrameRate) {
                    FrameCounter = 0;

                    if (Frame >= gif.Frames.Length - 1) {
                        Frame = 0;
                    }
                    else {
                        Frame++;
                    }
                }

                texture = gif.Frames[Frame];
            }
            else {
                var image = EmojiRepository.Assets.Request<Texture2D>(Emoji.Path).Value;

                texture = image;
            }

            var frame = texture.Frame();
            var origin = frame.Size() / 2f;

            var scaleMultiplier = EmojiverseConfig.Instance.EmojiDrawingScale;

            var area = new Vector2(Size) * scaleMultiplier;
            var offset = area * (1f / Size) / 2f + area / 2f + new Vector2(0f, 4f);
            var rectangle = new Rectangle((int)(position.X + offset.X), (int)(position.Y + offset.Y), (int)(Size * scaleMultiplier), (int)(Size * scaleMultiplier));

            var originalSnapshot = SpriteBatchSnapshot.Capture(spriteBatch);
            var modifiedSnapshot = originalSnapshot with {
                SortMode = SpriteSortMode.Texture,
                SamplerState = SamplerState.PointClamp
            };
            
            spriteBatch.End();
            spriteBatch.Begin(in modifiedSnapshot);

            spriteBatch.Draw(texture, rectangle, frame, Color.White, 0f, origin, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(in originalSnapshot);
        }

        size = new Vector2(Size);

        return true;
    }
}
