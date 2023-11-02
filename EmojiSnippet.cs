using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace Emojiverse;

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

        var directory = new DirectoryInfo(Emojiverse.EmojiPath);
        var matchingFiles = directory.GetFiles(Name + ".*");

        if (matchingFiles.Length <= 0) {
            return false;
        }

        var path = matchingFiles[0].FullName;
        var extension = Path.GetExtension(path);

        if (extension == PngExtension) {
            using var stream = new FileStream(path, FileMode.Open);
            
            var texture = Texture2D.FromStream(Main.graphics.GraphicsDevice, stream);
            var rectangle = new Rectangle((int)position.X, (int)position.Y, (int)Size, (int)Size);
            
            spriteBatch.Draw(texture, rectangle, texture.Frame(), Color.White, 0f, default, SpriteEffects.None, 0);
        }

        return true;
    }

    public override float GetStringLength(DynamicSpriteFont font) {
        return Size * Scale;
    }
}
