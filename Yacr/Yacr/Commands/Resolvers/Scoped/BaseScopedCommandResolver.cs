using Yacr.Extensions;
using Yacr.Collections;
using Yacr.Configuration;
using Yacr.Cryptography;
using Yacr.Cryptography.Algorithms;
using Yacr.Exceptions;
using Yacr.IO;
using Yacr.Services;
using static Yacr.Commands.ExpectedCommandsTokens;

namespace Yacr.Commands.Resolvers.Scoped;

public abstract class BaseScopedCommandResolver(
    IReader reader,
    IWriter writer,
    ICryptoAlgorithmResolver cryptoAlgorithmResolver,
    IRegexService regexService,
    IConfigurationProvider configurationProvider)
    : IScopedCommandResolver
{
    private readonly IReader _reader = reader;
    private readonly IWriter _writer = writer;
    private readonly ICryptoAlgorithmResolver _cryptoAlgorithmResolver = cryptoAlgorithmResolver;
    private readonly IRegexService _regexService = regexService;
    private readonly IConfigurationProvider _configurationProvider = configurationProvider;

    public abstract ICommand Resolve(ICommandExecutionStringInfo commandExecutionStringInfo);

    protected ICommand GetTransformStringCommand(string commandName, ICommandExecutionStringInfo commandExecutionStringInfo,
        Func<string, ICryptoStream, ICommand> cryptoCommandCallback)
    {
        var transformString = _regexService.GetQuotesStringWithEscapedQuotes(
            commandExecutionStringInfo.CommandArgumentsCollection.Next(StringToTransform));

        if (string.IsNullOrEmpty(transformString)) throw new CommandInvalidArgumentException(StringToTransform, commandName);

        commandExecutionStringInfo.CommandArgumentsCollection.ThrowIfNotSealed();

        var secret = RequireSecretWithEnsure();
        var algorithm = _cryptoAlgorithmResolver.Resolve(secret, commandExecutionStringInfo.CommandFlagsCollection);

        return cryptoCommandCallback(transformString, new CryptoStream(algorithm, _configurationProvider));
    }

    private string RequireSecret()
    {
        return RequireSecret(_writer.WriteEnterSecret);
    }

    private string RequireSecretWithEnsure()
    {
        var secret = RequireSecret();
#if !DEBUG // disable password ensure in debug mode
        var repeatedSecret = RequireSecret(Writer.WriteRepeatSecret);

        if (secret != repeatedSecret) throw new SecretsMismatchException();
#endif

        return secret;
    }

    private string RequireSecret(Action writeCallback)
    {
        string? input;

        do
        {
            writeCallback();
            input = _reader.ReadSecret();
        } while (input.IsNullOrEmptyOrWhitespace());

        return input;
    }
}