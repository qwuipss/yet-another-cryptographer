using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Decrypt;
using AegisCryptographer.Configuration;
using AegisCryptographer.Cryptography.Algorithms;
using AegisCryptographer.Exceptions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;
using static AegisCryptographer.Commands.CommandsArgumentsTokens;
using static AegisCryptographer.Commands.ExpectedCommandsTokens;

namespace AegisCryptographer.Commands.Resolvers.Scoped;

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