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

public sealed class UIChatEmojiPreviewer : UIState
{
    private const int InitialDelayMilliseconds = 200; // Adjust as needed
    private const int RepatDelayMilliseconds = 50;   // Adjust as needed
    private int elapsedMilliseconds;
    
    private List<Emoji> emojiSuggestions;
    private readonly int maxElements = 10;
    private int selectedIndex;

    public override void Update(GameTime gameTime) {
        if (!Main.drawingPlayerChat) {
            return;
        }

        emojiSuggestions = EmojiLoader.Enumerate().ToList();
        
        if (Main.keyState.IsKeyDown(Keys.Tab))
        {
            selectedIndex = (selectedIndex + 1) % emojiSuggestions.Count;
        }
        else if (Main.keyState.IsKeyDown(Keys.Up) && selectedIndex > 0)
        {
            selectedIndex--;
        }
        else if (Main.keyState.IsKeyDown(Keys.Down) && selectedIndex < emojiSuggestions.Count - 1)
        {
            selectedIndex++;
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        if (!Main.drawingPlayerChat) {
            return;
        }

        var startIdx = Math.Max(0, selectedIndex - maxElements / 2);
        var endIdx = Math.Min(startIdx + maxElements, emojiSuggestions.Count);

        var menuHeight = maxElements * 20; 
        var rectangle = new Rectangle(78, Main.screenHeight - menuHeight - 40, Main.screenWidth / 4, menuHeight);

        spriteBatch.Draw(TextureAssets.MagicPixel.Value, rectangle, Color.Black * 0.75f);

        for (var i = startIdx; i < endIdx; i++) {
            var yPos = Main.screenHeight - menuHeight + (i - startIdx) * 20 - 44;

            var sRect = new Rectangle(78, yPos, Main.screenWidth / 4, 20);
            
            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch, 
                FontAssets.MouseText.Value,
                $"[e:{emojiSuggestions[i].Id}] :{emojiSuggestions[i].Name}:", 
                new Vector2(82, yPos), 
                Color.White,
                Color.Black,
                0f,
                default,
                Vector2.One
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
