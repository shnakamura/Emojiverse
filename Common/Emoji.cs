using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace Emojiverse.Common;

public readonly record struct Emoji(Mod Mod, Asset<Texture2D> Texture, string Name, string Alias, int Id);
