using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Emojiverse;

public sealed class Emojiverse : Mod
{
    public static string EmojiPath => Path.Combine(Main.SavePath, "Emojis");

    public override void Load() {
        On_ChatManager.ParseMessage += ParseMessageHook;
        
        if (Directory.Exists(EmojiPath)) {
            return;
        }

        Directory.CreateDirectory(EmojiPath);
    }

    private List<TextSnippet> ParseMessageHook(On_ChatManager.orig_ParseMessage orig, string text, Color baseColor) {
        const string pattern = "^:(.*):$";
        
        var snippets = orig(text, baseColor);
        var newSnippets = new List<TextSnippet>(snippets);

        foreach (var snippet in snippets) {
            var snippetText = snippet.Text;
            var match = Regex.Match(snippetText, pattern);

            if (!match.Success) {
                continue;
            }
            
            var name = match.Groups[1].Value;
                
            newSnippets.Add(new EmojiSnippet(name) {
                CheckForHover = true,
                DeleteWhole = true
            });
        }
        
        return newSnippets;
    }
}
