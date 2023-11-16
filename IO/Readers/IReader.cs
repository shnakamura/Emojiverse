namespace Emojiverse.IO.Readers;

public interface IReader<T>
{
    string[] Extensions { get; }
    
    T Read(string path);
}
