using System;
using System.Collections.Generic;
using System.IO;
using Emojiverse.Common.IO;
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

        foreach (var pair in EmojiCacheSystem.ReadEmojis()) {
            caller.Reply($"[e:{pair.Name}] {pair.Name} from {pair.Pack}");
        }
    }
}
