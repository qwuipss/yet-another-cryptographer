using System.Security;
using System.Text;
using Yacr.Configuration;

namespace Yacr.IO;

public class Reader : IReader
{
    private readonly IWriter _writer;

    public Reader(IWriter writer, IConfigurationProvider configurationProvider)
    {
        _writer = writer;
        Console.InputEncoding = configurationProvider.Encoding;
    }

    public string? ReadLine()
    {
        _writer.Write("yacr> ");
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
            _writer.Write("\b*");
        }

        _writer.WriteLine();

        return secretBuilder.ToString();
    }
}