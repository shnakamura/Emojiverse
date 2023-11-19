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
    public readonly Emoji Emoji;

    private float frameCounter;
    private int frameIndex;
    
    public EmojiTagSnippet(Emoji emoji) {
        Emoji = emoji;
    }

    public override void OnHover() {
        Main.instance.MouseText(Emoji.Name.SurroundWith(':'));
    }

    public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = new(), Color color = new(), float scale = 1) {
        const int Size = 20;

        var notDrawingOutline = color.R != 0 || color.G != 0 || color.B != 0;
        
        size = new Vector2(Size);

        if (!justCheckingString && notDrawingOutline) {
            if (Emoji.Animated) {
                var gif = Emojiverse.Assets.Request<Gif>(Emoji.Path, AssetRequestMode.ImmediateLoad).Value;

                frameCounter++;

                if (frameCounter >= 1f / gif.FrameRate) {
                    frameCounter = 0f;

                    if (frameIndex >= gif.Frames.Length - 1) {
                        frameIndex = 0;
                        return true;
                    }
                    else {
                        frameIndex++;
                    }
                }
                
                var texture = gif.Frames[frameIndex];
                    
                var frame = texture.Frame();
                var origin = frame.Size() / 2f;

                var area = new Vector2(Size);
                var offset = area * (1f / Size) / 2f + area / 2f + new Vector2(0f, 4f);
                var rectangle = new Rectangle((int)(position.X + offset.X), (int)(position.Y + offset.Y), (int)(Size), (int)(Size));
                    
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
            else {
                var texture = Emojiverse.Assets.Request<Texture2D>(Emoji.Path, AssetRequestMode.ImmediateLoad).Value;

                var frame = texture.Frame();
                var origin = frame.Size() / 2f;

                var area = new Vector2(Size);
                var offset = area * (1f / Size) / 2f + area / 2f + new Vector2(0f, 4f);
                var rectangle = new Rectangle((int)(position.X + offset.X), (int)(position.Y + offset.Y), (int)(Size), (int)(Size));

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
        }


        return true;
    }
}
