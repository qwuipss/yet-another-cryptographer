namespace AegisCryptographer.IO;

public interface IWriter
{
    public void WriteLine();
    public void WriteLine(string text);
    public void Write(string text);
    public void WriteException(Exception exception);
    public void WriteEnterSecret();
    public void WriteRepeatSecret();
}