using Yacr.Collections;
using Yacr.Commands.Decrypt;
using Yacr.Configuration;
using Yacr.Cryptography.Algorithms;
using Yacr.Exceptions;
using Yacr.IO;
using Yacr.Services;
using static Yacr.Commands.CommandsArgumentsTokens;
using static Yacr.Commands.ExpectedCommandsTokens;

namespace Yacr.Commands.Resolvers.Scoped;

public class DecryptScopedCommandResolver(
    IReader reader,
    IWriter writer,
    ICryptoAlgorithmResolver cryptoAlgorithmResolver,
    IRegexService regexService,
    IConfigurationProvider configurationProvider)
    : BaseScopedCommandResolver(reader, writer, cryptoAlgorithmResolver, regexService, configurationProvider)
{
    public override ICommand Resolve(ICommandExecutionStringInfo commandExecutionStringInfo)
    {
        var transformTargetToken = commandExecutionStringInfo.CommandArgumentsCollection.Next(TransformTarget);

        return transformTargetToken switch
        {
            StringLongToken or StringShortToken => GetTransformStringCommand(CommandsTokens.DecryptLongToken, commandExecutionStringInfo,
                (str, cryptoStream) => new DecryptStringCommand(str, cryptoStream)),
            _ => throw new CommandInvalidArgumentException(transformTargetToken, CommandsTokens.DecryptLongToken)
        };
    }
}