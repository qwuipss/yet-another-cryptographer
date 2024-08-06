namespace AegisCryptographer.IO;

public interface IReader
{
    public string? ReadLine();

    public string ReadSecret();
}