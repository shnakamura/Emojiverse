using System;
using System.Collections;
using System.Reflection;
using Terraria.UI.Chat;

namespace Emojiverse.Utilities;

public static class ChatManagerUtils
{
    private static readonly FieldInfo handlersInfo;

    static ChatManagerUtils() {
        handlersInfo = typeof(ChatManager).GetField("_handlers", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

        if (handlersInfo == null) {
            throw new MissingFieldException(nameof(ChatManager), "_handlers");
        }
    }

    /// <summary>
    ///     Unregisters all specified <see cref="ITagHandler" /> instances from <see cref="ChatManager" /> based on their names.
    /// </summary>
    /// <param name="names">The names of the <see cref="ITagHandler" /> instances to be removed.</param>
    public static void Unregister(params string[] names) {
        if (handlersInfo.GetValue(null) is not IDictionary dictionary) {
            return;
        }

        foreach (var name in names) {
            dictionary.Remove(name);
        }
    }
}
