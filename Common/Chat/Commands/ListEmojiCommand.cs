using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Emojiverse.Common.Chat.Commands;

public sealed class ListEmojiCommand : ModCommand
{
    public override CommandType Type { get; } = CommandType.Chat;

    public override string Command { get; } = "listemojis";

    public override string Description { get; } = "Lists all available emojis from enabled resource packs";

    public override void Action(CommandCaller caller, string input, string[] args) {
        if (args.Length > 0) {
            throw new UsageException("No arguments were expected.");
        }

        foreach (var pair in EnumerateEmojis()) {
            caller.Reply($"[e:{pair.EmojiName}] {pair.EmojiName} from {pair.PackName}");
        }
    }

    // I love LolXD87, the Mexican.
    private IEnumerable<(string EmojiName, string PackName)> EnumerateEmojis() {
        foreach (var pack in Main.AssetSourceController.ActiveResourcePackList.EnabledPacks) {
            foreach (var packAsset in pack.GetContentSource().EnumerateAssets()) {
                static bool Check(string str, out string emojiName) {
                    emojiName = "";
                    var span = str.AsSpan();
                    if (!span.StartsWith("Emojis")) {
                        return false;
                    }

                    var separator = span[6];
                    if (separator == '/' || separator == '\\' || separator == Path.DirectorySeparatorChar || separator == Path.AltDirectorySeparatorChar) {
                        if (str.EndsWith(".png")) {
                            emojiName = str[7..^".png".Length];
                            return true;
                        }

                        if (str.EndsWith(".rawimg")) {
                            emojiName = str[7..^".rawimg".Length];
                        }
                    }

                    return false;
                }

                if (Check(packAsset, out var name)) {
                    yield return (name, pack.Name);
                }
            }
        }
    }
}
