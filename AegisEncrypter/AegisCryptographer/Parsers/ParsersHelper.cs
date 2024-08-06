using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.Extensions;
using AegisCryptographer.IO;

namespace AegisCryptographer.Parsers;

public static class ParsersHelper
{
    public static IParser CreateParser(string? input, IReader reader, IWriter writer)
    {
        if (StringExtensions.IsNullOrEmptyOrWhitespace(input))
        {
            throw new InputEmptyException();
        }

        var split = input!.Trim().Split();
        var command = split[0];
        var arguments = split[1..].ToCommandArgumentsCollection();

        return command switch
        {
            "encrypt" or "enc" => new EncryptParser(arguments, reader, writer),
            _ => throw new ParserResolveException(command)
        };
    }
}