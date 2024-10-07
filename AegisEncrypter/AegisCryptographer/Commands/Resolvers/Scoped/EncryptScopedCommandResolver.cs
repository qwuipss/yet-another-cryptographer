using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Encrypt;
using AegisCryptographer.Exceptions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;
using static AegisCryptographer.Commands.CommandsArgumentsTokens;
using static AegisCryptographer.Commands.ExpectedCommandsTokens;
using static AegisCryptographer.Commands.CommandsTokens;

namespace AegisCryptographer.Commands.Resolvers.Scoped;

public class EncryptScopedCommandResolver(
    ICommandExecutionStringInfo commandExecutionStringInfo,
    IReader reader,
    IWriter writer) : BaseScopedCommandResolver(new RegexService(), commandExecutionStringInfo, reader, writer)
{
    public override ICommand Resolve()
    {
        var transformTargetToken = CommandExecutionStringInfo.CommandArgumentsCollection.Next(TransformTarget);

        return transformTargetToken switch
        {
            StringLongToken or StringShortToken =>
                GetTransformStringCommand(EncryptLongToken,
                    (str, cryptoStream) => new EncryptStringCommand(str, cryptoStream)),
            _ => throw new CommandInvalidArgumentException(transformTargetToken, EncryptLongToken)
        };
    }
}