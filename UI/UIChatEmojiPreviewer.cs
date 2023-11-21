using System;
using System.Collections.Generic;
using System.Text;
using Emojiverse.IO;
using Emojiverse.Utilities.Extensions;
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


    
    private int holdDelayTimer;
    private int selectedIndex;

    public List<Emoji> EmojiSuggestions { get; } = new();

    public override void Update(GameTime gameTime) {
        EmojiSuggestions.Clear();
        EmojiSuggestions.TrimExcess();

        var input = Main.chatText;
        var index = input.LastIndexOf(':');

        var isValid = index != -1 && !string.IsNullOrEmpty(input) && input.Length >= 3;
        var isIsolated = index - 1 <= -1 || char.IsWhiteSpace(input[index - 1]);
        
        if (!Main.drawingPlayerChat || !isValid || !isIsolated) {
            selectedIndex = 0;
            return;
        }

        var content = input.Substring(index + 1);

        var addedNames = new HashSet<Emoji>();

        foreach (var emoji in EmojiLoader.EnumerateEmojis()) {
            if (emoji.Alias.StartsWith(content)
                && addedNames.Add(emoji)) {
                EmojiSuggestions.Add(emoji);
            }
            else if (emoji.Alias.Contains(content)
                && !emoji.Alias.StartsWith(content)
                && addedNames.Add(emoji)) {
                EmojiSuggestions.Add(emoji);
            }
        }

        if ((Main.keyState.IsKeyDown(Keys.Tab) || Main.keyState.IsKeyDown(Keys.Enter)) && EmojiSuggestions.Count > 0) {
            var suggestion = EmojiSuggestions[selectedIndex];
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

        selectedIndex = (int)MathHelper.Clamp(selectedIndex, 0, EmojiSuggestions.Count - 1);

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        if (!Main.drawingPlayerChat || EmojiSuggestions.Count <= 0) {
            return;
        }
        
        var MaxElements = 10;

        var ElementHeight = 24;
        var ElementSpacing = 4;
        var ElementGap = 24;

        var menuWidth = Main.screenWidth / 2;
        var menuHeight = MaxElements * ElementHeight;

        if (EmojiSuggestions.Count < MaxElements) {
            menuHeight = EmojiSuggestions.Count * ElementHeight;
        }

        var menuTop = Main.screenHeight - menuHeight - 40;
        var menuLeft = 78;

        var menuRectangle = new Rectangle(menuLeft, menuTop, menuWidth, menuHeight);

        spriteBatch.Draw(TextureAssets.MagicPixel.Value, menuRectangle, Color.Black * 0.75f);

        var start = Math.Max(0, selectedIndex - MaxElements + 1);
        var end = Math.Min(start + MaxElements, EmojiSuggestions.Count);

        var font = FontAssets.MouseText.Value;

        for (var i = start; i < end; i++) {
            var emoji = EmojiSuggestions[i];

            var elementTop = Main.screenHeight - menuHeight + (i - start) * ElementHeight - 40;
            var elementLeft = menuLeft;

            if (i == selectedIndex) {
                var suggestionRectangle = new Rectangle(menuLeft, elementTop, menuWidth, ElementHeight);

                spriteBatch.Draw(TextureAssets.MagicPixel.Value, suggestionRectangle, Color.DarkGray * 0.25f);
            }

            var tagLeft = elementLeft + ElementSpacing;
            var tagTop = elementTop - ElementSpacing / 2f;
            
            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch,
                font,
                $"[e:{emoji.Id}]",
                new Vector2(tagLeft, tagTop),
                Color.White,
                Color.Black,
                0f,
                default,
                new Vector2(1f)
            );
            
            var aliasSize = font.MeasureString(emoji.Alias.SurroundWith(':')) * 0.8f;
            var aliasLeft = tagLeft + ElementGap;
            var aliasTop = elementTop + ElementSpacing;

            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch,
                font,
                emoji.Alias.SurroundWith(':'),
                new Vector2(aliasLeft, aliasTop),
                Color.White,
                Color.Black,
                0f,
                default,
                new Vector2(0.8f)
            );

            var packSize = font.MeasureString(emoji.Pack) * 0.6f;
            var packLeft = elementLeft + menuWidth - packSize.X - ElementSpacing * 2f;
            var packTop = elementTop + packSize.Y / 2f - ElementSpacing * 0.6f;
            
            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch,
                font,
                emoji.Pack,
                new Vector2(packLeft, packTop),
                Color.White,
                Color.Black,
                0f,
                default,
                new Vector2(0.6f)
            );
        }

        base.Draw(spriteBatch);
    }
}
