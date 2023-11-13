using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Emojiverse.Common;

public sealed class ClientConfig : ModConfig
{
    public static ClientConfig Instance => ModContent.GetInstance<ClientConfig>();
    
    public override ConfigScope Mode { get; } = ConfigScope.ClientSide;

    [Header("Messages")]
    [DefaultValue(true)]
    public bool EnableWarningMessages { get; set; } = true;
}
