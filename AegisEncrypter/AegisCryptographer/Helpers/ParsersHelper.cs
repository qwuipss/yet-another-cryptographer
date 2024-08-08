using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Decrypt;
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
        
        var (argumentsString, flagsCollection) = RegexHelper.ExtractExecuteStringFlags(input!);
        var split = argumentsString.Split();

        if (split.Length is 0)
        {
            throw new CommandArgumentStringMissingException();
        }

        var commandExecutionStringInfo =
            new CommandExecutionStringInfo(new CommandArgumentsCollection(split[1..]), flagsCollection);
        
        return split[0] switch
        {
            "encrypt" or "enc" => new EncryptParser(commandExecutionStringInfo, reader, writer),
            "decrypt" or "dec" => new DecryptParser(commandExecutionStringInfo, reader, writer),
            _ => throw new ParserResolveException(split[0])
        };
    }
}