using AegisCryptographer.Collections;
using AegisCryptographer.Commands;
using AegisCryptographer.Commands.Decrypt;
using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.IO;

namespace AegisCryptographer.Parsers;

public class DecryptParser(ICommandExecutionStringInfo commandExecutionStringInfo, IReader reader, IWriter writer)
    : BaseParser(commandExecutionStringInfo, reader, writer)
{
    private const string CommandName = "decrypt";

    public override ICommand ParseCommand()
    {
        var commandSubType = CommandExecutionStringInfo.CommandArgumentsCollection[0];

        return commandSubType switch
        {
            "string" or "str" => GetCryptoActionStringCommand("decrypt string", CommandName,
                (str, cryptoStream) => new DecryptStringCommand(str, cryptoStream)),
            _ => throw new CommandInvalidArgumentException(commandSubType,
                CommandName)
        };
    }
}