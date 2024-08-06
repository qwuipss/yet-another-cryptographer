using System.Text;

namespace AegisCryptographer.IO;

public class Reader(IWriter writer) : IReader
{
    static Reader()
    {
        Console.InputEncoding = Config.Encoding;
    }

    public string? ReadLine()
    {
        writer.Write("aegis> ");

        return Console.ReadLine();
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

                    writer.Write(" ");
                    Console.CursorLeft -= 1;
                }
                else
                {
                    Console.CursorLeft += 1;
                }

                continue;
            }

            if (!char.IsLetterOrDigit(keyInfo.KeyChar))
            {
                continue;
            }

            Console.CursorLeft -= 1;
            writer.Write("*");

            secretBuilder.Append(keyInfo.KeyChar);
        }

        writer.WriteLine();

        return secretBuilder.ToString();
    }
}