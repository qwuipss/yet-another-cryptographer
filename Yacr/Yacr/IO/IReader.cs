namespace Yacr.IO;

public interface IReader
{
    string? ReadLine();

    string ReadSecret();
}