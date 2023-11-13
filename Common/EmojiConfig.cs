using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Emojiverse.Common;

public sealed class EmojiConfig : ModConfig
{
    public static EmojiConfig Instance => ModContent.GetInstance<EmojiConfig>();
    
    public override ConfigScope Mode { get; } = ConfigScope.ClientSide;

    [DefaultValue(true)]
    public bool DisplayWarning = true;
}
