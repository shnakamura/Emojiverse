using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Emojiverse.Common.UI;

public sealed class UIChatSuggestion : UIState
{
    public override void OnInitialize() {
        base.OnInitialize();
    }

    public override void Update(GameTime gameTime) {
        if (!Main.drawingPlayerChat) {
            return;
        }

        var names = new string[] {
            "ech", "sadcat", "eggsadcat", "ppsdct", "cry", "hand", "gun",
            "omfga", "omg", "ohmygod", "sad", "skullcry"
        };

        var text = Main.chatText;

        var index = text.LastIndexOf(':');

        if (!string.IsNullOrEmpty(text) && index != -1) {
            var pretext = text.Substring(index + 1);

            var sortedNames = new List<string>();
            var addedNames = new HashSet<string>();

            foreach (var name in names.OrderBy(n => n)) {
                if (name.StartsWith(pretext) && addedNames.Add(name)) {
                    sortedNames.Add(name);
                } else if (name.Contains(pretext) && !name.StartsWith(pretext) && addedNames.Add(name)) {
                    sortedNames.Add(name);
                }
            }

            foreach (var name in sortedNames) {
                Main.NewText($"Text: {pretext} @ Suggestion matched: {name}");
            }
        }
            
        
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        if (!Main.drawingPlayerChat) {
            return;
        }
        
        base.Draw(spriteBatch);
    }
}


public sealed class UIChatSuggestionSystem : ModSystem
{
    public UIChatSuggestion SuggestionState;
    public UserInterface SuggestionInterface;

    public override void OnModLoad() {
        SuggestionState = new UIChatSuggestion();
        SuggestionState.Activate();

        SuggestionInterface = new UserInterface();
        SuggestionInterface.SetState(SuggestionState);
    }

    public override void UpdateUI(GameTime gameTime) {
        SuggestionInterface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        var index = layers.FindIndex(x => x.Name == "Vanilla: Mouse Text");

        if (index != -1) {
            layers.Insert(index + 1, new LegacyGameInterfaceLayer("Emojiverse: Suggestion",
                delegate {
                    SuggestionInterface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
        }
    }
}
