using AegisCryptographer.Collections;
using AegisCryptographer.Cryptography;
using AegisCryptographer.Cryptography.Algorithms;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Extensions;
using AegisCryptographer.IO;
using AegisCryptographer.Services;
using static AegisCryptographer.Commands.ExpectedCommandsTokens;

namespace AegisCryptographer.Commands.Resolvers.Scoped;

public abstract class BaseScopedCommandResolver(
    IRegexService regexService,
    ICommandExecutionStringInfo commandExecutionStringInfo,
    IReader reader,
    IWriter writer)
    : ICommandResolver
{
    private IRegexService RegexService { get; } = regexService;
    private IReader Reader { get; } = reader;
    private IWriter Writer { get; } = writer;
    protected ICommandExecutionStringInfo CommandExecutionStringInfo { get; } = commandExecutionStringInfo;

    public abstract ICommand Resolve();

    protected ICommand GetTransformStringCommand(string commandName, Func<string, ICryptoStream, ICommand> cryptoCommandCallback)
    {
        var transformString = RegexService.GetQuotesStringWithEscapedQuotes(
            CommandExecutionStringInfo.CommandArgumentsCollection.Next(StringToTransform));

        if (string.IsNullOrEmpty(transformString)) throw new CommandInvalidArgumentException(StringToTransform, commandName);

        CommandExecutionStringInfo.CommandArgumentsCollection.ThrowIfNotSealed();

        var secret = RequireSecretWithEnsure();
        var algorithm = ResolveCryptoAlgorithm(secret);

        return cryptoCommandCallback(transformString, new CryptoStream(algorithm));
    }

    private string RequireSecret()
    {
        return RequireSecret(Writer.WriteEnterSecret);
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

    private ICryptoAlgorithm ResolveCryptoAlgorithm(string secret)
    {
        //todo
        return new AesGcmAlgorithm(secret, new CryptoService());
    }

    private string RequireSecret(Action writeCallback)
    {
        string? input;

        do
        {
            writeCallback();
            input = Reader.ReadSecret();
        } while (input.IsNullOrEmptyOrWhitespace());

        return input;
    }
}