using System;
using Emojiverse.Common;
using Terraria.ModLoader;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public override object Call(params object[] args) {
        ArgumentNullException.ThrowIfNull(args, nameof(args));

        if (args.Length == 0) {
            throw new ArgumentException("");
        }

        if (args[0] is not string command) {
            throw new InvalidOperationException();
        }

        if (args[1] is not Mod mod) {
            throw new InvalidOperationException();
        }

        switch (command.ToLower()) {
            case "load":
            case "loademojis":
            case "loademojisfrommod":
                if (args[2] is not string rootDirectory) {
                    throw new InvalidOperationException();
                }
                EmojiSystem.LoadEmojisFromMod(mod, rootDirectory);
                return true;
            case "unload":
            case "unloademojis":
            case "unloademojisfrommod":
                EmojiSystem.UnloadEmojisFromMod(mod);
                return true;
        }

        return false;
    }
}