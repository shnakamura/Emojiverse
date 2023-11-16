namespace Emojiverse.IO.Readers;

public interface IReader<T>
{
    T Read<T>();
}
