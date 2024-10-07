namespace AegisCryptographer.IO;

public interface IWriter
{
    void WriteLine();
    void WriteLine(string text);
    void Write(string text);
    void WriteException(Exception exception);
    void WriteEnterSecret();
    void WriteRepeatSecret();
}