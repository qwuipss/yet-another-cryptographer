using System.Text;
using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Flags;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.Extensions;
using AegisCryptographer.Helpers;
using AegisCryptographer.IO;
using static AegisCryptographer.Commands.CommandTokens;

namespace AegisCryptographer.Parsers;

public class ParsersResolver(ICommandFlagsResolver flagsResolver, IReader reader, IWriter writer) : IParsersResolver
{
    private ICommandFlagsResolver FlagsResolver { get; } = flagsResolver;
    private IReader Reader { get; } = reader;
    private IWriter Writer { get; } = writer;

    public IParser Resolve(string? input)
    {
        if (StringExtensions.IsNullOrEmptyOrWhitespace(input)) throw new InputEmptyException();

        var commandExecutionStringInfo = input!.ToCommandExecutionStringInfo(FlagsResolver);
        var parserToken = commandExecutionStringInfo.CommandArgumentsCollection.Next();
        
        return parserToken switch
        {
            EncryptLongToken or EncryptShortToken => new EncryptParser(commandExecutionStringInfo, Reader, Writer),
            DecryptLongToken or DecryptShortToken => new DecryptParser(commandExecutionStringInfo, Reader, Writer),
            _ => throw new ParserResolveException(parserToken)
        };
    }
}