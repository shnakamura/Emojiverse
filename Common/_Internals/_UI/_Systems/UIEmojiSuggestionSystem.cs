using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Emojiverse.Common;

[Autoload(Side = ModSide.Client)]
internal sealed class UIEmojiSuggestionSystem : ModSystem
{
    private static GameTime lastGameTime;

    public static UserInterface SuggestionInterface { get; private set; }

    public override void Load() {
        SuggestionInterface = new UserInterface();
    }

    public override void Unload() {
        SuggestionInterface?.CurrentState?.Deactivate();
        SuggestionInterface?.SetState(null);
        SuggestionInterface = null;
    }

    public override void UpdateUI(GameTime gameTime) {
        lastGameTime = gameTime;

        SuggestionInterface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        var index = layers.FindIndex(x => x.Name == "Vanilla: Player Chat");

        if (index == -1) {
            return;
        }

        layers.Insert(index + 1, new LegacyGameInterfaceLayer($"{nameof(Emojiverse)}:{nameof(UIEmojiSuggestion)}", DrawSuggestionInterface, InterfaceScaleType.UI));
    }

    public static void Enable() {
        SuggestionInterface.SetState(new UIEmojiSuggestion());
        SuggestionInterface.CurrentState.Activate();
    }

    public static void Disable() {
        SuggestionInterface?.CurrentState?.Deactivate();
        SuggestionInterface?.SetState(null);
    }

    private static bool DrawSuggestionInterface() {
        SuggestionInterface.Draw(Main.spriteBatch, lastGameTime);

        return true;
    }
}
