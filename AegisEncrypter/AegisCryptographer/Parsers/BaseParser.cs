using AegisCryptographer.Collections;
using AegisCryptographer.Commands;
using AegisCryptographer.Cryptography;
using AegisCryptographer.Cryptography.Algorithms;
using AegisCryptographer.Exceptions;
using AegisCryptographer.Exceptions.Parsers;
using AegisCryptographer.Extensions;
using AegisCryptographer.Helpers;
using AegisCryptographer.IO;

namespace AegisCryptographer.Parsers;

public abstract class BaseParser(ICommandExecutionStringInfo commandExecutionStringInfo, IReader reader, IWriter writer)
    : IParser
{
    protected ICommandExecutionStringInfo CommandExecutionStringInfo { get; } = commandExecutionStringInfo;
    private IReader Reader { get; } = reader;
    private IWriter Writer { get; } = writer;

    public abstract ICommand ParseCommand();

    protected ICommand GetEvaluateStringCommand(string argumentName, string commandName,
        Func<string, ICryptoStream, ICommand> cryptoCommandCallback)
    {
        var extractedStrings = RegexHelper.ExtractQuotesStringWithEscapedQuotes(
            CommandExecutionStringInfo.CommandArgumentsCollection[1..]).ToList();

        if (extractedStrings.Count is 0) throw new CommandInvalidArgumentException(argumentName, commandName);

        var argumentString = extractedStrings[0];

        if (string.IsNullOrEmpty(argumentString)) throw new CommandInvalidArgumentException(argumentName, commandName);

        var secret = RequireSecretWithEnsure();
        var algorithm = ResolveCryptoAlgorithm(secret);

        return cryptoCommandCallback(argumentString, new CryptoStream(algorithm));
    }

    protected string RequireSecret()
    {
        return RequireSecret(Writer.WriteEnterSecret);
    }

    protected string RequireSecretWithEnsure()
    {
        var secret = RequireSecret();
#if !DEBUG // disable password ensure in debug mode
        var repeatedSecret = RequireSecret(Writer.WriteRepeatSecret);

        if (secret != repeatedSecret)
        {
            throw new SecretsMismatchException();
        }
#endif

        return secret;
    }

    private ICryptoAlgorithm ResolveCryptoAlgorithm(string secret)
    {
        //todo
        return new AesGcmAlgorithm(secret);
    }

    private string RequireSecret(Action writeCallback)
    {
        string? input;

        do
        {
            writeCallback();
            input = Reader.ReadSecret();
        } while (StringExtensions.IsNullOrEmptyOrWhitespace(input));

        return input;
    }
}