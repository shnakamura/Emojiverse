using System;
using System.Collections.Generic;
using System.Linq;
using Emojiverse.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Emojiverse.UI;

// Completely disregard this file, as of now. It will be the last addition before v0.3.

public sealed class UIChatEmojiPreviewer : UIState
{
    private const int MaxElements = 10;
    
    private const int ElementHeight = 20;
    private const int ElementPadding = 4;

    public List<Emoji> Suggestions { get; private set; } = new();
    
    private int selectedIndex;

    public override void Update(GameTime gameTime) {
        Suggestions.Clear();
        Suggestions.TrimExcess();
        
        var input = Main.chatText;
        var index = input.LastIndexOf(':');
        
        if (!Main.drawingPlayerChat && !string.IsNullOrEmpty(input) && index != -1 && input.Length >= 3) {
            return;
        }

        var content = input.Substring(index + 1);

        var addedNames = new HashSet<Emoji>();

        foreach (var emoji in EmojiLoader.Enumerate()) {
            if (emoji.Name.StartsWith(content) 
                && addedNames.Add(emoji)) {
                Suggestions.Add(emoji);
            } else if (emoji.Name.Contains(content) 
                && !emoji.Name.StartsWith(content) 
                && addedNames.Add(emoji)) {
                Suggestions.Add(emoji);
            }
        }
    
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        if (!Main.drawingPlayerChat && Suggestions.Count > 0) {
            return;
        }

        var menuWidth = Main.screenWidth / 2;
        var menuHeight = MaxElements * ElementHeight;

        if (Suggestions.Count < MaxElements) {
            menuHeight = Suggestions.Count * ElementHeight;
        }

        var menuTop = Main.screenHeight - menuHeight - 40;
        var menuLeft = 78;
        
        var rectangle = new Rectangle(menuLeft, menuTop, menuWidth, menuHeight);

        spriteBatch.Draw(TextureAssets.MagicPixel.Value, rectangle, Color.Black * 0.75f);
        
        var startIdx = Math.Max(0, selectedIndex - MaxElements / 2);
        var endIdx = Math.Min(startIdx + MaxElements, Suggestions.Count);

        var font = FontAssets.MouseText.Value;
        
        for (var i = startIdx; i < endIdx; i++) {
            var elementTop = Main.screenHeight - menuHeight + (i - startIdx) * ElementHeight - 40;
            var elementLeft = 82;
            
            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch, 
                font,
                $"[e:{Suggestions[i].Id}]", 
                new Vector2(elementLeft, elementTop), 
                Color.White,
                Color.Black,
                0f,
                default,
                new Vector2(1f)
            );

            var tagSize = new Vector2(20f);
            
            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch, 
                font,
                $":{Suggestions[i].Alias}:", 
                new Vector2(elementLeft + tagSize.X, elementTop), 
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

public sealed class UIChatEmojiPreviewerSystem : ModSystem
{
    public UIChatEmojiPreviewer State { get; private set; }
    public UserInterface UserInterface { get; private set; }

    public override void Load() {
        State = new UIChatEmojiPreviewer();
        State.Activate();

        UserInterface = new UserInterface();
        UserInterface.SetState(State);
    }

    public override void UpdateUI(GameTime gameTime) {
        UserInterface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        var layer = layers.FindIndex(x => x.Name == "Vanilla: Player Chat");

        if (layer == -1) {
            return;
        }

        layers.Insert(
            layer + 1,
            new LegacyGameInterfaceLayer(
                "Emojiverse:ChatEmojiPreviewer",
                delegate {
                    UserInterface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI
            )
        );
    }
}
