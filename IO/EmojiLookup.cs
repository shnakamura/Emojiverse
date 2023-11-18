using System.Collections.Generic;
using System.IO;
using Terraria.IO;
using Terraria.ModLoader;

namespace Emojiverse.IO;

public sealed class EmojiLookup : ModSystem
{
    private static readonly Dictionary<string, int> repeatedNamesCountByName = new();

    public override void PostSetupContent() {
        
    }
}
