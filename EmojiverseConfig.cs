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

    [Header("Chat")]
    [DefaultValue(1f)]
    public float DrawingScale { get; set; } = 1f;
    
    [DefaultValue(1)]
    public int DrawingResolution { get; set; } = 1;
}
