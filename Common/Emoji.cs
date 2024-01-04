using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace Emojiverse.Common;

/// <summary>
///     Provides an instance of an emoji.
/// </summary>
/// <param name="Mod">The mod from where the emoji was loaded.</param>
/// <param name="Texture">The texture that the emoji uses.</param>
/// <param name="Name">The internal name of the emoji.</param>
/// <param name="Alias">The external name or alias of the emoji.</param>
/// <param name="Id">The identity of the emoji.</param>
public readonly record struct Emoji(Mod Mod, Asset<Texture2D> Texture, string Name, string Alias, int Id);
