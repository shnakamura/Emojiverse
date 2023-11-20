using System;
using System.Collections.Generic;
using System.Text;
using Emojiverse.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Emojiverse.UI;

public sealed class UIChatEmojiPreviewer : UIState
{
    private const int KeyInitialDelay = 30;
    private const int KeyRepeatDelay = 2;

    private const int MaxElements = 10;

    private const int ElementHeight = 20;
    private const int ElementPadding = 4;
    
    private int holdDelayTimer;
    private int selectedIndex;

    public List<Emoji> Suggestions { get; } = new();

    public override void Update(GameTime gameTime) {
        Suggestions.Clear();
        Suggestions.TrimExcess();

        var input = Main.chatText;
        var index = input.LastIndexOf(':');

        if (!Main.drawingPlayerChat || string.IsNullOrEmpty(input) || index == -1 || input.Length < 3) {
            return;
        }

        var content = input.Substring(index + 1);

        var addedNames = new HashSet<Emoji>();

        foreach (var emoji in EmojiLoader.EnumerateEmojis()) {
            if (emoji.Name.StartsWith(content)
                && addedNames.Add(emoji)) {
                Suggestions.Add(emoji);
            }
            else if (emoji.Name.Contains(content)
                && !emoji.Name.StartsWith(content)
                && addedNames.Add(emoji)) {
                Suggestions.Add(emoji);
            }
        }

        if ((Main.keyState.IsKeyDown(Keys.Tab) || Main.keyState.IsKeyDown(Keys.Enter)) && Suggestions.Count > 0) {
            var suggestion = Suggestions[selectedIndex];
            var tagText = $"[e:{suggestion.Id}]";

            var stringBuilder = new StringBuilder(input);
            stringBuilder.Remove(index, content.Length + 1);
            stringBuilder.Insert(index, tagText);

            Main.chatText = stringBuilder.ToString();
        }

        if (Main.keyState.IsKeyDown(Keys.Down)) {
            if (holdDelayTimer == 0 || (holdDelayTimer > KeyInitialDelay && holdDelayTimer % KeyRepeatDelay == 0)) {
                selectedIndex++;
            }

            holdDelayTimer++;
        }
        else if (Main.keyState.IsKeyDown(Keys.Up)) {
            if (holdDelayTimer == 0 || (holdDelayTimer > KeyInitialDelay && holdDelayTimer % KeyRepeatDelay == 0)) {
                selectedIndex--;
            }

            holdDelayTimer++;
        }
        else {
            holdDelayTimer = 0;
        }

        selectedIndex = (int)MathHelper.Clamp(selectedIndex, 0, Suggestions.Count - 1);

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        var input = Main.chatText;
        var index = input.LastIndexOf(':');

        if (!Main.drawingPlayerChat || Suggestions.Count <= 0) {
            return;
        }

        var menuWidth = Main.screenWidth / 2;
        var menuHeight = MaxElements * ElementHeight;

        if (Suggestions.Count < MaxElements) {
            menuHeight = Suggestions.Count * ElementHeight;
        }

        var menuTop = Main.screenHeight - menuHeight - 40;
        var menuLeft = 78;

        var menuRectangle = new Rectangle(menuLeft, menuTop, menuWidth, menuHeight);

        spriteBatch.Draw(TextureAssets.MagicPixel.Value, menuRectangle, Color.Black * 0.75f);

        var start = Math.Max(0, selectedIndex - MaxElements + 1);
        var end = Math.Min(start + MaxElements, Suggestions.Count);

        var font = FontAssets.MouseText.Value;

        for (var i = start; i < end; i++) {
            var suggestion = Suggestions[i];

            var elementTop = Main.screenHeight - menuHeight + (i - start) * ElementHeight - 40;
            var elementLeft = 82;

            var tagText = $"[e:{suggestion.Id}]";
            var aliasText = $":{suggestion.Alias}:";

            if (i == selectedIndex) {
                var suggestionRectangle = new Rectangle(menuLeft, elementTop, menuWidth, ElementHeight);

                spriteBatch.Draw(TextureAssets.MagicPixel.Value, suggestionRectangle, Color.DarkGray * 0.75f);
            }

            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch,
                font,
                tagText,
                new Vector2(elementLeft, elementTop - ElementPadding),
                Color.White,
                Color.Black,
                0f,
                default,
                new Vector2(1f)
            );

            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch,
                font,
                aliasText,
                new Vector2(elementLeft + 30, elementTop),
                Color.White,
                Color.Black,
                0f,
                default,
                new Vector2(1f)
            );
        }

        base.Draw(spriteBatch);
    }
}
