using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Decrypt;
using AegisCryptographer.Exceptions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;
using static AegisCryptographer.Commands.CommandsArgumentsTokens;
using static AegisCryptographer.Commands.ExpectedCommandsTokens;

namespace AegisCryptographer.Commands.Resolvers.Scoped;

public class DecryptScopedCommandResolver(
    ICommandExecutionStringInfo commandExecutionStringInfo,
    IReader reader,
    IWriter writer) : BaseScopedCommandResolver(new RegexService(), commandExecutionStringInfo, reader, writer)
{
    public override ICommand Resolve()
    {
        var transformTargetToken = CommandExecutionStringInfo.CommandArgumentsCollection.Next(TransformTarget);

        return transformTargetToken switch
        {
            StringLongToken or StringShortToken => GetTransformStringCommand(CommandsTokens.DecryptLongToken,
                (str, cryptoStream) => new DecryptStringCommand(str, cryptoStream)),
            _ => throw new CommandInvalidArgumentException(transformTargetToken, CommandsTokens.DecryptLongToken)
        };
    }
}