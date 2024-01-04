using System.Collections;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Emojiverse.Utilities;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse.Common;

internal sealed class EmojiChatSystem : ModSystem
{
    public static readonly string[] Tags = {
        "e",
        "emoji",
        "emote"
    };

    public override void Load() {
        ChatManager.Register<EmojiTagHandler>(Tags);
        
        IL_Main.DoUpdate_HandleChat += HandleChatPatch;

        On_Main.OpenPlayerChat += OpenPlayerChatHook;
        On_Main.ClosePlayerChat += ClosePlayerChatHook;
    }

    public override void Unload() {
        ChatManagerUtils.Unregister(Tags);
    }

    private static void HandleChatPatch(ILContext il) {
        var c = new ILCursor(il);

        if (!c.TryGotoNext(i => i.MatchLdstr(string.Empty))) {
            return;
        }

        c.Index--;

        var label = c.DefineLabel();

        c.EmitDelegate(() => UIEmojiSuggestionSystem.SuggestionInterface?.CurrentState is UIEmojiSuggestion state && state.EmojiSuggestions?.Count > 0);

        c.Emit(OpCodes.Brfalse, label);
        c.Emit(OpCodes.Ret);
        c.Emit(OpCodes.Nop);
        
        c.MarkLabel(label);
    }

    private static void OpenPlayerChatHook(On_Main.orig_OpenPlayerChat orig) {
        orig();

        UIEmojiSuggestionSystem.Enable();
    }

    private static void ClosePlayerChatHook(On_Main.orig_ClosePlayerChat orig) {
        orig();

        UIEmojiSuggestionSystem.Disable();
    }
}
