using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.Extensions;
using AegisCryptographer.IO;
using AegisCryptographer.Parsers;
using static AegisCryptographer.Commands.CommandNames;

namespace AegisCryptographer.Helpers;

public static class ParsersHelper
{
    public static IParser CreateParser(string? input, IReader reader, IWriter writer)
    {
        if (StringExtensions.IsNullOrEmptyOrWhitespace(input)) throw new InputEmptyException();

        var (argumentsString, flagsBundles) = RegexHelper.ExtractFlags(input!);
        var split = argumentsString.Split();

        if (split.Length is 0) throw new CommandArgumentStringMissingException();

        var commandExecutionStringInfo = new CommandExecutionStringInfo(new CommandArgumentsCollection(split[1..]),
            ResolveCommandFlags(flagsBundles));

        return split[0] switch
        {
            EncryptLongName or EncryptShortName => new EncryptParser(commandExecutionStringInfo, reader, writer),
            DecryptLongName or DecryptShortName => new DecryptParser(commandExecutionStringInfo, reader, writer),
            _ => throw new ParserResolveException(split[0])
        };
    }

    private static CommandFlagsCollection ResolveCommandFlags(IEnumerable<(string Flag, string Value)> flagsBundles)
    {
        var flagsCollection = new CommandFlagsCollection();

        flagsBundles.ForEach(flagBundle =>
        {
            var resolvedFlag = CommandFlagResolver.ResolveCommandFlag(flagBundle);

            if (flagsCollection.TryGetValue(resolvedFlag, out var existedFlag))
                throw new FlagDuplicateException(resolvedFlag.Key, existedFlag.Key);
        });

        return flagsCollection;
    }
}