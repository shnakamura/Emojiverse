using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Emojiverse;

public sealed class EmojiverseConfig : ModConfig
{
    public static EmojiverseConfig Instance => ModContent.GetInstance<EmojiverseConfig>();
    
    public override ConfigScope Mode { get; } = ConfigScope.ClientSide;

    [Header("Messages")]
    [DefaultValue(true)]
    public bool EnableWarningMessages { get; set; } = true;
}
