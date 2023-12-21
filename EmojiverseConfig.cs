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
    [Slider]
    [Range(0.5f, 1f)]
    [DefaultValue(1f)]
    public float EmojiDrawingScale { get; set; } = 1f;
}
