using Yacr.Exceptions;

namespace Yacr.IO;

public interface IWriter
{
    void WriteLine();
    void WriteLine(string text);
    void Write(string text);
    void WriteIntentionalException(IntentionalException exception);
    void WriteEnterSecret();
    void WriteRepeatSecret();
}