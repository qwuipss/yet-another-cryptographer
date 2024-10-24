using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Encrypt;
using AegisCryptographer.Configuration;
using AegisCryptographer.Cryptography.Algorithms;
using AegisCryptographer.Exceptions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;
using static AegisCryptographer.Commands.CommandsArgumentsTokens;
using static AegisCryptographer.Commands.ExpectedCommandsTokens;
using static AegisCryptographer.Commands.CommandsTokens;

namespace AegisCryptographer.Commands.Resolvers.Scoped;

public class EncryptScopedCommandResolver(
    IReader reader,
    IWriter writer,
    ICryptoAlgorithmResolver cryptoAlgorithmResolver,
    IRegexService regexService,
    IConfigurationProvider configurationProvider) : BaseScopedCommandResolver(reader, writer, cryptoAlgorithmResolver, regexService, configurationProvider)
{
    public override ICommand Resolve(ICommandExecutionStringInfo commandExecutionStringInfo)
    {
        var transformTargetToken = commandExecutionStringInfo.CommandArgumentsCollection.Next(TransformTarget);

        return transformTargetToken switch
        {
            StringLongToken or StringShortToken =>
                GetTransformStringCommand(EncryptLongToken, commandExecutionStringInfo,
                    (str, cryptoStream) => new EncryptStringCommand(str, cryptoStream)),
            _ => throw new CommandInvalidArgumentException(transformTargetToken, EncryptLongToken)
        };
    }
}