using System;
using Emojiverse.Common;
using Terraria.ModLoader;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public override object Call(params object[] args) {
        if (args == null) {
            throw new ArgumentNullException("Expected non-null arguments.");
        }
        
        if (args.Length == 0) {
            throw new ArgumentException("Expected non-empty arguments.");
        }

        if (args[0] is not string call) {
            throw new Exception($"Expected an argument of type {typeof(string)} when specifying the call, but received {args[1].GetType()} instead.");
        }

        if (args[1] is not Mod mod) {
            throw new Exception($"Expected an argument of type {typeof(Mod)} when passing the mod instance, but received {args[1].GetType()} instead.");
        }

        call = string.Join(string.Empty, call.Split(' '));
        call = call.ToLower();

        switch (call) {
            case "load":
            case "loademojis":
            case "loademojisfrom":
            case "loademojisfrommod":
            case "register":
            case "registerfrom:":
            case "registeremojisfrom":
            case "registeremojisfrommod":
                if (args[2] is not string rootDirectory) {
                    throw new Exception($"Expected an argument of type {typeof(string)} when specifying root directory, but received {args[2].GetType()} instead.");
                }

                EmojiSystem.LoadEmojisFromMod(mod, rootDirectory);
                return true;
            case "unload":
            case "unloademojis":
            case "unloademojisfrom":
            case "unloademojisfrommod":
            case "unregister":
            case "unregisterfrom:":
            case "unregisteremojisfrom":
            case "unregisteremojisfrommod":
                EmojiSystem.UnloadEmojisFromMod(mod);
                return true;
        }

        return false;
    }
}
