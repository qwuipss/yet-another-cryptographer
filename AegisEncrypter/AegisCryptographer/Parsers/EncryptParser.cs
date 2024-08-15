using AegisCryptographer.Collections;
using AegisCryptographer.Commands;
using AegisCryptographer.Commands.Encrypt;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.IO;

namespace AegisCryptographer.Parsers;

public class EncryptParser(ICommandExecutionStringInfo commandExecutionStringInfo, IReader reader, IWriter writer)
    : BaseParser(commandExecutionStringInfo, reader, writer)
{
    private const string CommandName = "encrypt";

    public override ICommand ParseCommand()
    {
        var commandSubType = CommandExecutionStringInfo.CommandArgumentsCollection.Next();

        return commandSubType switch
        {
            "string" or "str" => GetEvaluateStringCommand("encrypt string", CommandName,
                (str, cryptoStream) => new EncryptStringCommand(str, cryptoStream)),
            _ => throw new CommandInvalidArgumentException(commandSubType, CommandName)
        };
    }
}