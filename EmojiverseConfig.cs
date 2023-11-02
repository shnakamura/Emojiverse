using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Emojiverse;

class EmojiverseConfig : ModConfig
{
    internal static EmojiverseConfig Instance => ModContent.GetInstance<EmojiverseConfig>();
    public override ConfigScope Mode => ConfigScope.ClientSide;

    public bool DisableFileWatcher { get; set; }
}