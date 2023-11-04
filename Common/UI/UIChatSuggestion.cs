using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Emojiverse.Common.UI;

public sealed class UIChatSuggestion : UIState
{
    public override void Update(GameTime gameTime) {
        if (!Main.drawingPlayerChat) {
            return;
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
    public UserInterface SuggestionInterface;
    public UIChatSuggestion SuggestionState;

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
            layers.Insert(
                index + 1,
                new LegacyGameInterfaceLayer(
                    $"{nameof(Emojiverse)}: Suggestion",
                    delegate {
                        SuggestionInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI
                )
            );
        }
    }
}
