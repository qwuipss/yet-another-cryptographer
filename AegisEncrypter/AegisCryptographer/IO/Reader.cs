using System.Security;
using System.Text;

namespace AegisCryptographer.IO;

public class Reader(IWriter writer) : IReader
{
    private IWriter Writer { get; } = writer;

    static Reader()
    {
        Console.InputEncoding = Settings.Encoding;
    }

    public string? ReadLine()
    {
        Writer.Write("aegis> ");

        return Console.ReadLine()?.Trim();
    }

    public string ReadSecret()
    {
        var secretBuilder = new StringBuilder();

        ConsoleKeyInfo keyInfo;

        while ((keyInfo = Console.ReadKey()).Key is not ConsoleKey.Enter)
        {
            if (keyInfo.Key is ConsoleKey.Backspace)
            {
                if (secretBuilder.Length > 0)
                {
                    secretBuilder.Remove(secretBuilder.Length - 1, 1);
                    Console.Write("\b \b\b \b");
                }
                else
                {
                    Console.Write("\b \b");
                }

                continue;
            }

            if (keyInfo.KeyChar is '\0') continue;

            secretBuilder.Append(keyInfo.KeyChar);
#if !DEBUG // disable password masking in debug mode 
            Writer.Write("\b*");
#endif
        }

        Writer.WriteLine();

        return secretBuilder.ToString();
    }
}