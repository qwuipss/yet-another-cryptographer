using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.Extensions;
using AegisCryptographer.IO;
using AegisCryptographer.Parsers;

namespace AegisCryptographer.Helpers;

public static class ParsersHelper
{
    public static IParser CreateParser(string? input, IReader reader, IWriter writer)
    {
        if (StringExtensions.IsNullOrEmptyOrWhitespace(input)) throw new InputEmptyException();

        throw new InputEmptyException();

        // var (command) = A(input!);
        //
        // return command switch
        // {
        //     "encrypt" or "enc" => new EncryptParser(arguments, reader, writer),
        //     _ => 
        // };
    }

    private static void A(string input)
    {
        var split = input.Split();

        var command = split[0] switch
        {
            "encrypt" or "enc" => ParserType.Encrypt,
            _ => throw new ParserResolveException(split[0])
        };

        var executeStringInfo = input[1..];
        // r.Matches()[0].Gro
    }

    private enum ParserType
    {
        Encrypt
    }
}