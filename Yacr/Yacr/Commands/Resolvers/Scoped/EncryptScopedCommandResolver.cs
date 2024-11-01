using Yacr.Collections;
using Yacr.Commands.Encrypt;
using Yacr.Configuration;
using Yacr.Cryptography.Algorithms;
using Yacr.Exceptions;
using Yacr.IO;
using Yacr.Services;
using static Yacr.Commands.CommandsArgumentsTokens;
using static Yacr.Commands.ExpectedCommandsTokens;
using static Yacr.Commands.CommandsTokens;

namespace Yacr.Commands.Resolvers.Scoped;

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