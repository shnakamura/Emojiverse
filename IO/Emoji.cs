using Microsoft.Xna.Framework.Graphics;

namespace Emojiverse.IO;

public readonly record struct Emoji(string Name, string Alias, string Path, string Pack, int Id, bool Animated);