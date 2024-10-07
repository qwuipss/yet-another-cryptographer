namespace AegisCryptographer.IO;

public interface IReader
{
    string? ReadLine();

    string ReadSecret();
}